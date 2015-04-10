using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline
{
    public class InfluenceData
    {
        public ICommand Click { get; set; }

        public Composer Composer { get; set; }

        public bool IsEnabled { get; set; }

        public string Name { get; set; }
    }
}