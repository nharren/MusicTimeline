using System;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline
{
    public class Link
    {
        public Uri Icon { get; set; }

        public string Label { get; set; }

        public ICommand Click { get; set; }
    }
}