using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.ViewModels;
using System.Collections.Generic;
using System.EDTF;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.Providers
{
    public static class ComposerEraProvider
    {
        public static List<ComposerEraViewModel> GetEras(DataProvider dataProvider)
        {
            var eras = new List<ComposerEraViewModel>();

            foreach (var era in dataProvider.Eras)
            {
                var background = (SolidColorBrush)null;

                if (era.Name == "Medieval")
                {
                    background = new SolidColorBrush(Color.FromRgb(153, 153, 153));            // #FF999999
                }
                else if (era.Name == "Renaissance")
                {
                    background = new SolidColorBrush(Color.FromRgb(155, 128, 181));            // #FF9B80B5
                }
                else if (era.Name == "Baroque")
                {
                    background = new SolidColorBrush(Color.FromRgb(204, 77, 77));              // #FFCC4D4D
                }
                else if (era.Name == "Classical")
                {
                    background = new SolidColorBrush(Color.FromRgb(51, 151, 193));             // #FF3397C1
                }
                else if (era.Name == "Romantic")
                {
                    background = new SolidColorBrush(Color.FromRgb(69, 168, 90));              // #FF45A85A
                }
                else if (era.Name == "20th Century")
                {
                    background = new SolidColorBrush(Color.FromRgb(205, 173, 74));             // #FFCDAD4A
                }
                else if (era.Name == "21st Century")
                {
                    background = new SolidColorBrush(Color.FromRgb(219, 109, 138));            // #FFDB6D8A
                }

                var musicEra = new ComposerEraViewModel(era.Name, ExtendedDateTimeInterval.Parse(era.Dates), background, Brushes.White);

                eras.Add(musicEra);
            }

            return eras;
        }
    }
}