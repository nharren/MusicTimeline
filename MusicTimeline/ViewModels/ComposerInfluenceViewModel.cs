using NathanHarrenstein.MusicDB;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.ViewModels
{
    public class ComposerInfluenceViewModel
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