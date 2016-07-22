using NAudio.Wave;
using NAudio.Wave.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    /// <summary>
    /// MP3 Frame Decompressor using ACM
    /// </summary>
    public class NetworkAcmMp3FrameDecompressor : INetworkMp3FrameDecompressor
    {
        private readonly AcmStream conversionStream;
        private readonly WaveFormat pcmFormat;
        private bool disposed;

        /// <summary>
        /// Creates a new ACM frame decompressor
        /// </summary>
        /// <param name="sourceFormat">The MP3 source format</param>
        public NetworkAcmMp3FrameDecompressor(WaveFormat sourceFormat)
        {
            this.pcmFormat = AcmStream.SuggestPcmFormat(sourceFormat);
            try
            {
                conversionStream = new AcmStream(sourceFormat, pcmFormat);
            }
            catch (Exception)
            {
                disposed = true;
                GC.SuppressFinalize(this);
                throw;
            }
        }

        /// <summary>
        /// Output format (PCM)
        /// </summary>
        public WaveFormat OutputFormat { get { return pcmFormat; } }

        /// <summary>
        /// Decompresses a frame
        /// </summary>
        /// <param name="frame">The MP3 frame</param>
        /// <param name="dest">destination buffer</param>
        /// <param name="destOffset">Offset within destination buffer</param>
        /// <returns>Bytes written into destination buffer</returns>
        public int DecompressFrame(NetworkMp3Frame frame, byte[] dest, int destOffset)
        {
            if (frame == null)
            {
                throw new ArgumentNullException("frame", "You must provide a non-null Mp3Frame to decompress");
            }

            Array.Copy(frame.RawData, conversionStream.SourceBuffer, frame.FrameLength);

            int sourceBytesConverted = 0;
            int converted = conversionStream.Convert(frame.FrameLength, out sourceBytesConverted);

            if (sourceBytesConverted != frame.FrameLength)
            {
                throw new InvalidOperationException(String.Format("Couldn't convert the whole MP3 frame (converted {0}/{1})",
                    sourceBytesConverted, frame.FrameLength));
            }

            Array.Copy(conversionStream.DestBuffer, 0, dest, destOffset, converted);

            return converted;
        }

        /// <summary>
        /// Resets the MP3 Frame Decompressor after a reposition operation
        /// </summary>
        public void Reset()
        {
            conversionStream.Reposition();
        }

        /// <summary>
        /// Disposes of this MP3 frame decompressor
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                if (conversionStream != null)
                    conversionStream.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Finalizer ensuring that resources get released properly
        /// </summary>
        ~NetworkAcmMp3FrameDecompressor()
        {
            System.Diagnostics.Debug.Assert(false, "NetworkAcmMp3FrameDecompressor Dispose was not called");
            Dispose();
        }
    }
}
