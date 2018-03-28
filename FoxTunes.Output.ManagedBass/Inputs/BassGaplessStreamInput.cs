﻿using FoxTunes.Interfaces;
using ManagedBass;
using ManagedBass.Gapless;
using System.Collections.Generic;
using System.Linq;

namespace FoxTunes
{
    public class BassGaplessStreamInput : BassStreamInput
    {
        public BassGaplessStreamInput(int rate, int channels, BassFlags flags)
        {
            this.Rate = rate;
            this.Channels = channels;
            this.Flags = flags;
        }

        public override int Rate { get; protected set; }

        public override int Channels { get; protected set; }

        public override BassFlags Flags { get; protected set; }

        public override int ChannelHandle { get; protected set; }

        public IEnumerable<int> Queue
        {
            get
            {
                var count = default(int);
                return BassGapless.GetChannels(out count);
            }
        }

        public int CurrentChannelHandle
        {
            get
            {
                return this.Queue.FirstOrDefault();
            }
        }

        public override void Connect(IBassStreamComponent previous)
        {
            Logger.Write(this, LogLevel.Debug, "Initializing BASS GAPLESS.");
            BassUtils.OK(BassGapless.Init());
            BassUtils.OK(BassGapless.SetConfig(BassGaplessAttriubute.KeepAlive, true));
            Logger.Write(this, LogLevel.Debug, "Creating BASS GAPLESS stream with rate {0} and {1} channels.", this.Rate, this.Channels);
            this.ChannelHandle = BassGapless.StreamCreate(this.Rate, this.Channels, this.Flags);
            if (this.ChannelHandle == 0)
            {
                BassUtils.Throw();
            }
        }

        public override bool CheckFormat(int rate, int channels)
        {
            return this.Rate == rate && this.Channels == channels;
        }

        public override bool Contains(int channelHandle)
        {
            return this.Queue.Contains(channelHandle);
        }

        public override int Position(int channelHandle)
        {
            var count = default(int);
            var channelHandles = BassGapless.GetChannels(out count);
            return channelHandles.IndexOf(channelHandle);
        }

        public override bool Add(int channelHandle)
        {
            if (this.Queue.Contains(channelHandle))
            {
                Logger.Write(this, LogLevel.Debug, "Stream is already enqueued: {0}", channelHandle);
                return false;
            }
            Logger.Write(this, LogLevel.Debug, "Adding stream to the queue: {0}", channelHandle);
            BassUtils.OK(BassGapless.ChannelEnqueue(channelHandle));
            return true;
        }

        public override bool Remove(int channelHandle)
        {
            if (!this.Queue.Contains(channelHandle))
            {
                Logger.Write(this, LogLevel.Debug, "Stream is not enqueued: {0}", channelHandle);
                return false;
            }
            Logger.Write(this, LogLevel.Debug, "Removing stream from the queue: {0}", channelHandle);
            BassUtils.OK(BassGapless.ChannelRemove(channelHandle));
            return true;
        }

        public override void Reset()
        {
            Logger.Write(this, LogLevel.Debug, "Resetting the queue.");
            foreach (var channelHandle in this.Queue)
            {
                this.Remove(channelHandle);
            }
            this.ClearBuffer();
        }

        protected override void OnDisposing()
        {
            if (this.ChannelHandle != 0)
            {
                Logger.Write(this, LogLevel.Debug, "Freeing BASS GAPLESS stream: {0}", this.ChannelHandle);
                BassUtils.OK(Bass.StreamFree(this.ChannelHandle));
            }
            Logger.Write(this, LogLevel.Debug, "Releasing BASS GAPLESS.");
            BassUtils.OK(BassGapless.Free());
        }
    }
}
