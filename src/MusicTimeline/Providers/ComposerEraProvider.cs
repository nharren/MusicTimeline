using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.ViewModels;
using System.Collections.Generic;
using System.EDTF;
using System.Windows.Media;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class ComposerEraProvider
    {
        public static List<ComposerEraViewModel> GetEras(DataProvider dataProvider)
        {
            var eras = dataProvider.Eras.ToList();
            var composerEraViewModels = new List<ComposerEraViewModel>();

            foreach (var era in eras)
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

                composerEraViewModels.Add(musicEra);
            }

            Process(composerEraViewModels);

            return composerEraViewModels;
        }

        private static void Process(IList<ComposerEraViewModel> composerEraViewModels)
        {
            var count = composerEraViewModels.Count;

            for (int i = 0; i < count - 1; i++)
            {
                var era1StartDate = composerEraViewModels[i].Dates.Earliest();
                var era1EndDate = composerEraViewModels[i].Dates.Latest();
                
                var era2StartDate = composerEraViewModels[i + 1].Dates.Earliest();
                var era2EndDate = composerEraViewModels[i + 1].Dates.Latest();

                if (era2StartDate < era1EndDate)
                {
                    var transitionBrush = new LinearGradientBrush();
                    transitionBrush.StartPoint = new Point(0, 0.5);
                    transitionBrush.EndPoint = new Point(1, 0.5);
                    transitionBrush.GradientStops.Add(new GradientStop(((SolidColorBrush)composerEraViewModels[i].Background).Color, 0));
                    transitionBrush.GradientStops.Add(new GradientStop(((SolidColorBrush)composerEraViewModels[i + 1].Background).Color, 1));

                    var transitionEra = new ComposerEraViewModel(null, new ExtendedDateTimeInterval(era2StartDate, era1EndDate), transitionBrush, Brushes.White);

                    composerEraViewModels[i].Dates = new ExtendedDateTimeInterval(era1StartDate, era2StartDate);
                    composerEraViewModels[i + 1].Dates = new ExtendedDateTimeInterval(era1EndDate, era2EndDate);

                    composerEraViewModels.Add(transitionEra);
                }
            }
        }
    }
}