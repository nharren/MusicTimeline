using Database;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline
{
    public class Influence
    {
        public ICommand Click
        {
            get; set;
        }

        public Composer Composer
        {
            get; set;
        }

        public bool IsEnabled
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }
    }
}