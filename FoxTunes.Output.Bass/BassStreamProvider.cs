﻿using FoxTunes.Interfaces;
using ManagedBass;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoxTunes
{
    public class BassStreamProvider : StandardComponent, IBassStreamProvider
    {
        public const byte PRIORITY_HIGH = 0;

        public const byte PRIORITY_NORMAL = 100;

        public const byte PRIORITY_LOW = 255;

        public BassStreamProvider()
        {
            this.Semaphore = new SemaphoreSlim(1, 1);
        }

        public SemaphoreSlim Semaphore { get; private set; }

        public IBassOutput Output { get; private set; }

        public IBassStreamFactory StreamFactory { get; private set; }

        public IBassStreamPipelineManager PipelineManager { get; private set; }

        public virtual byte Priority
        {
            get
            {
                return PRIORITY_NORMAL;
            }
        }

        public virtual BassStreamProviderFlags Flags
        {
            get
            {
                return BassStreamProviderFlags.None;
            }
        }

        public override void InitializeComponent(ICore core)
        {
            this.Output = core.Components.Output as IBassOutput;
            this.StreamFactory = ComponentRegistry.Instance.GetComponent<IBassStreamFactory>();
            this.PipelineManager = ComponentRegistry.Instance.GetComponent<IBassStreamPipelineManager>();
            if (this.StreamFactory != null)
            {
                this.StreamFactory.Register(this);
            }
            base.InitializeComponent(core);
        }

        public virtual bool CanCreateStream(PlaylistItem playlistItem)
        {
            return true;
        }

        public virtual Task<int> CreateStream(PlaylistItem playlistItem)
        {
            var flags = BassFlags.Decode;
            if (this.Output != null && this.Output.Float)
            {
                flags |= BassFlags.Float;
            }
            return this.CreateStream(playlistItem, flags);
        }

#if NET40
        public virtual Task<int> CreateStream(PlaylistItem playlistItem, BassFlags flags)
#else
        public virtual async Task<int> CreateStream(PlaylistItem playlistItem, BassFlags flags)
#endif
        {
#if NET40
            this.Semaphore.Wait();
#else
            await this.Semaphore.WaitAsync();
#endif
            try
            {
                var channelHandle = default(int);
                if (this.Output != null && this.Output.PlayFromMemory)
                {
                    channelHandle = BassInMemoryHandler.CreateStream(playlistItem.FileName, 0, 0, flags);
                    if (channelHandle == 0)
                    {
                        Logger.Write(this, LogLevel.Warn, "Failed to load file into memory: {0}", playlistItem.FileName);
                    }
                }
                else
                {
                    channelHandle = Bass.CreateStream(playlistItem.FileName, 0, 0, flags);
                }
#if NET40
                return TaskEx.FromResult(channelHandle);
#else
                return channelHandle;
#endif
            }
            finally
            {
                this.Semaphore.Release();
            }
        }

        public virtual void FreeStream(PlaylistItem playlistItem, int channelHandle)
        {
            Logger.Write(this, LogLevel.Debug, "Freeing stream: {0}", channelHandle);
            Bass.StreamFree(channelHandle); //Not checking result code as it contains an error if the application is shutting down.
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed || !disposing)
            {
                return;
            }
            this.OnDisposing();
            this.IsDisposed = true;
        }

        protected virtual void OnDisposing()
        {
            //Nothing to do.
        }

        ~BassStreamProvider()
        {
            Logger.Write(this, LogLevel.Error, "Component was not disposed: {0}", this.GetType().Name);
            try
            {
                this.Dispose(true);
            }
            catch
            {
                //Nothing can be done, never throw on GC thread.
            }
        }

        public class BassStreamProviderKey : IEquatable<BassStreamProviderKey>
        {
            public BassStreamProviderKey(string fileName, int channelHandle)
            {
                this.FileName = fileName;
                this.ChannelHandle = channelHandle;
            }

            public string FileName { get; private set; }

            public int ChannelHandle { get; private set; }

            public virtual bool Equals(BassStreamProviderKey other)
            {
                if (other == null)
                {
                    return false;
                }
                if (object.ReferenceEquals(this, other))
                {
                    return true;
                }
                if (!string.Equals(this.FileName, other.FileName, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                if (this.ChannelHandle != other.ChannelHandle)
                {
                    return false;
                }
                return true;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as BassStreamProviderKey);
            }

            public override int GetHashCode()
            {
                var hashCode = default(int);
                unchecked
                {
                    if (!string.IsNullOrEmpty(this.FileName))
                    {
                        hashCode += this.FileName.GetHashCode();
                    }
                    hashCode += this.ChannelHandle.GetHashCode();
                }
                return hashCode;
            }

            public static bool operator ==(BassStreamProviderKey a, BassStreamProviderKey b)
            {
                if ((object)a == null && (object)b == null)
                {
                    return true;
                }
                if ((object)a == null || (object)b == null)
                {
                    return false;
                }
                if (object.ReferenceEquals((object)a, (object)b))
                {
                    return true;
                }
                return a.Equals(b);
            }

            public static bool operator !=(BassStreamProviderKey a, BassStreamProviderKey b)
            {
                return !(a == b);
            }
        }
    }
}
