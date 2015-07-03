using NathanHarrenstein.MusicDb;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.Models
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