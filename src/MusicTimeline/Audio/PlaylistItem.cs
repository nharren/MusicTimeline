using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class PlaylistItem
    {
        public Func<WaveStream> GetStream { get; set; }
        public Dictionary<string, string> Metadata { get; private set; } = new Dictionary<string, string>();
    }
}
