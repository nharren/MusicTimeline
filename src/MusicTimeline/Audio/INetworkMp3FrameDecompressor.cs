﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    /// <summary>
    /// Interface for MP3 frame by frame decoder
    /// </summary>
    public interface INetworkMp3FrameDecompressor : IDisposable
    {
        /// <summary>
        /// Decompress a single MP3 frame
        /// </summary>
        /// <param name="frame">Frame to decompress</param>
        /// <param name="dest">Output buffer</param>
        /// <param name="destOffset">Offset within output buffer</param>
        /// <returns>Bytes written to output buffer</returns>
        int DecompressFrame(NetworkMp3Frame frame, byte[] dest, int destOffset);

        /// <summary>
        /// Tell the decoder that we have repositioned
        /// </summary>
        void Reset();

        /// <summary>
        /// PCM format that we are converting into
        /// </summary>
        WaveFormat OutputFormat { get; }
    }
}
