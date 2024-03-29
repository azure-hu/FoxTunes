﻿using FoxTunes.Interfaces;
using ManagedBass;
using ManagedBass.Mix;
using System;
using System.Collections.Generic;

namespace FoxTunes
{
    public class BassDirectSoundStreamOutput : BassStreamOutput
    {
        private BassDirectSoundStreamOutput()
        {
            this.MixerChannelHandles = new HashSet<int>();
        }

        public BassDirectSoundStreamOutput(BassDirectSoundStreamOutputBehaviour behaviour, BassOutputStream stream) : this()
        {
            this.Behaviour = behaviour;
            this.Rate = behaviour.Output.Rate;
            this.Channels = BassDirectSoundDevice.Info.Outputs;
            this.Flags = BassFlags.Default;
            if (this.Behaviour.Output.Float)
            {
                this.Flags |= BassFlags.Float;
            }
        }

        public override string Name
        {
            get
            {
                return "Direct Sound";
            }
        }

        public override string Description
        {
            get
            {
                return string.Format(
                    "{0} ({1}/{2}/{3})",
                    this.Name,
                    BassUtils.DepthDescription(this.Flags),
                    MetaDataInfo.SampleRateDescription(this.Rate),
                    MetaDataInfo.ChannelDescription(this.Channels)
                );
            }
        }

        public BassDirectSoundStreamOutputBehaviour Behaviour { get; private set; }

        public int Device
        {
            get
            {
                return BassDirectSoundDevice.Device;
            }
        }

        public override int Rate { get; protected set; }

        public override int Channels { get; protected set; }

        public override BassFlags Flags { get; protected set; }

        public override int ChannelHandle { get; protected set; }

        public HashSet<int> MixerChannelHandles { get; protected set; }

        public override bool CheckFormat(int rate, int channels)
        {
            return true;
        }

        public override void Connect(IBassStreamComponent previous)
        {
            Logger.Write(this, LogLevel.Debug, "Creating BASS MIX stream with rate {0} and {1} channels.", this.Rate, this.Channels);
            this.ChannelHandle = BassMix.CreateMixerStream(this.Rate, this.Channels, this.Flags);
            if (this.ChannelHandle == 0)
            {
                BassUtils.Throw();
            }
            Logger.Write(this, LogLevel.Debug, "Adding stream to the mixer: {0}", previous.ChannelHandle);
            BassUtils.OK(BassMix.MixerAddChannel(this.ChannelHandle, previous.ChannelHandle, BassFlags.Default | BassFlags.MixerBuffer));
            this.MixerChannelHandles.Add(previous.ChannelHandle);
        }


        public override bool IsPlaying
        {
            get
            {
                return Bass.ChannelIsActive(this.ChannelHandle) == PlaybackState.Playing;
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsPaused
        {
            get
            {
                return Bass.ChannelIsActive(this.ChannelHandle) == PlaybackState.Paused;
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsStopped
        {
            get
            {
                return Bass.ChannelIsActive(this.ChannelHandle) == PlaybackState.Stopped;
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override int Latency
        {
            get
            {
                return 0;
            }
        }

        public override void Play()
        {
            if (this.IsPlaying)
            {
                return;
            }
            Logger.Write(this, LogLevel.Debug, "Playing channel: {0}", this.ChannelHandle);
            try
            {
                BassUtils.OK(Bass.ChannelPlay(this.ChannelHandle, true));
            }
            catch (Exception e)
            {
                this.OnError(e);
            }
        }

        public override void Pause()
        {
            if (this.IsPaused)
            {
                return;
            }
            Logger.Write(this, LogLevel.Debug, "Pausing channel: {0}", this.ChannelHandle);
            try
            {
                BassUtils.OK(Bass.ChannelPause(this.ChannelHandle));
            }
            catch (Exception e)
            {
                this.OnError(e);
            }
        }

        public override void Resume()
        {
            if (this.IsPlaying)
            {
                return;
            }
            Logger.Write(this, LogLevel.Debug, "Resuming channel: {0}", this.ChannelHandle);
            try
            {
                BassUtils.OK(Bass.ChannelPlay(this.ChannelHandle, false));
            }
            catch (Exception e)
            {
                this.OnError(e);
            }
        }

        public override void Stop()
        {
            if (this.IsStopped)
            {
                return;
            }
            Logger.Write(this, LogLevel.Debug, "Stopping channel: {0}", this.ChannelHandle);
            try
            {
                BassUtils.OK(Bass.ChannelStop(this.ChannelHandle));
            }
            catch (Exception e)
            {
                this.OnError(e);
            }
        }

        public override int GetData(float[] buffer)
        {
            var length = default(uint);
            switch (buffer.Length)
            {
                case 128:
                    length = FFT256;
                    break;
                case 256:
                    length = FFT512;
                    break;
                case 512:
                    length = FFT1024;
                    break;
                default:
                    throw new NotImplementedException();
            }
            foreach (var channelHandle in this.MixerChannelHandles)
            {
                return BassMix.ChannelGetData(channelHandle, buffer, unchecked((int)length));
            }
            return 0;
        }

        protected override void OnDisposing()
        {
            if (this.ChannelHandle != 0)
            {
                Logger.Write(this, LogLevel.Debug, "Freeing BASS stream: {0}", this.ChannelHandle);
                BassUtils.OK(Bass.StreamFree(this.ChannelHandle));
            }
        }
    }
}
