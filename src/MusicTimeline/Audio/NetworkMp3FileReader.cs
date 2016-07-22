using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.MusicTimeline.Audio
{

    class Mp3Index
    {
        public long FilePosition { get; set; }
        public int SamplePosition { get; set; }
        public int SampleCount { get; set; }
        public int ByteCount { get; set; }
    }

    /// <summary>
    /// Class for reading from MP3 files
    /// </summary>
    public class NetworkMp3FileReader : WaveStream
    {
        private WaveFormat waveFormat;
        private Stream mp3Stream;

        private int frameLengthInBytes;

        /// <summary>
        /// The MP3 wave format (n.b. NOT the output format of this stream - see the WaveFormat property)
        /// </summary>
        public Mp3WaveFormat Mp3WaveFormat { get; private set; }

        private NetworkXingHeader xingHeader;

        private int tocIndex;

        private int sampleRate;
        private int bytesPerSample;

        private INetworkMp3FrameDecompressor decompressor;

        private byte[] decompressBuffer;
        private int decompressBufferOffset;
        private int decompressLeftovers;

        NetworkMp3Frame intialFrame;

        private object repositionLock = new object();

        /// <summary>
        /// Opens MP3 from a stream rather than a file
        /// Will not dispose of this stream itself
        /// </summary>
        /// <param name="inputStream"></param>
        public NetworkMp3FileReader(Stream inputStream)
        {
            // Calculated as a double to minimize rounding errors
            double bitRate;

            mp3Stream = inputStream;

            NetworkMp3Frame mp3Frame = intialFrame = NetworkMp3Frame.LoadFromStream(mp3Stream, true);
            sampleRate = mp3Frame.SampleRate;
            frameLengthInBytes = mp3Frame.FrameLength;
            bitRate = mp3Frame.BitRate;
            xingHeader = NetworkXingHeader.LoadXingHeader(mp3Frame);

            Mp3WaveFormat = new Mp3WaveFormat(sampleRate, mp3Frame.ChannelMode == ChannelMode.Mono ? 1 : 2, frameLengthInBytes, (int)bitRate);
            decompressor = new NetworkAcmMp3FrameDecompressor(Mp3WaveFormat); // new DmoMp3FrameDecompressor(this.Mp3WaveFormat); 
            waveFormat = decompressor.OutputFormat;
            bytesPerSample = (decompressor.OutputFormat.BitsPerSample) / 8 * decompressor.OutputFormat.Channels;
            // no MP3 frames have more than 1152 samples in them
            // some MP3s I seem to get double
            decompressBuffer = new byte[1152 * bytesPerSample * 2];
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Reads the next mp3 frame
        /// </summary>
        /// <returns>Next mp3 frame, or null if EOF</returns>
        public NetworkMp3Frame ReadNextFrame()
        {
            return ReadNextFrame(true);
        }

        /// <summary>
        /// Reads the next mp3 frame
        /// </summary>
        /// <returns>Next mp3 frame, or null if EOF</returns>
        private NetworkMp3Frame ReadNextFrame(bool readData)
        {
            NetworkMp3Frame frame = null;
            try
            {
                frame = NetworkMp3Frame.LoadFromStream(mp3Stream, readData);
                if (frame != null)
                {
                    tocIndex++;
                }
            }
            catch (EndOfStreamException)
            {
                // suppress for now - it means we unexpectedly got to the end of the stream
                // half way through
            }
            return frame;
        }

        /// <summary>
        /// This is the length in bytes of data available to be read out from the Read method
        /// (i.e. the decompressed MP3 length)
        /// n.b. this may return 0 for files whose length is unknown
        /// </summary>
        public override long Length
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// <see cref="WaveStream.WaveFormat"/>
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        /// <summary>
        /// <see cref="Stream.Position"/>
        /// </summary>
        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        bool initialFrame = true;

        /// <summary>
        /// Reads decompressed PCM data from our MP3 file.
        /// </summary>
        public override int Read(byte[] sampleBuffer, int offset, int numBytes)
        {
            int bytesRead = 0;
            lock (repositionLock)
            {
                if (decompressLeftovers != 0)
                {
                    int toCopy = Math.Min(decompressLeftovers, numBytes);
                    Array.Copy(decompressBuffer, decompressBufferOffset, sampleBuffer, offset, toCopy);
                    decompressLeftovers -= toCopy;

                    if (decompressLeftovers == 0)
                    {
                        decompressBufferOffset = 0;
                    }
                    else
                    {
                        decompressBufferOffset += toCopy;
                    }

                    bytesRead += toCopy;
                    offset += toCopy;
                }

                while (bytesRead < numBytes)
                {
                    NetworkMp3Frame frame;

                    if (initialFrame)
                    {
                        frame = intialFrame;
                        initialFrame = false;
                    }
                    else
                    {
                        frame = ReadNextFrame();
                    }
                    

                    if (frame != null)
                    {
                        int decompressed = decompressor.DecompressFrame(frame, decompressBuffer, 0);
                        int toCopy = Math.Min(decompressed, numBytes - bytesRead);
                        Array.Copy(decompressBuffer, 0, sampleBuffer, offset, toCopy);

                        if (toCopy < decompressed)
                        {
                            decompressBufferOffset = toCopy;
                            decompressLeftovers = decompressed - toCopy;
                        }
                        else
                        {
                            // no lefovers
                            decompressBufferOffset = 0;
                            decompressLeftovers = 0;
                        }

                        offset += toCopy;
                        bytesRead += toCopy;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Debug.Assert(bytesRead <= numBytes, "MP3 File Reader read too much");
            return bytesRead;
        }

        /// <summary>
        /// Xing header if present
        /// </summary>
        public NetworkXingHeader XingHeader
        {
            get { return xingHeader; }
        }

        /// <summary>
        /// Disposes this WaveStream
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (mp3Stream != null)
                {
                    mp3Stream = null;
                }
                if (decompressor != null)
                {
                    decompressor.Dispose();
                    decompressor = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
