using Database;
using NathanHarrenstein.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.ComposerTimeline.Data
{
    public class ComposerEvent : Event
    {
        public Composer Composer { get; set; }
    }
}