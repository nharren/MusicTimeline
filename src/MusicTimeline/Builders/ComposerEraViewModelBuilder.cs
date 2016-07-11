using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.ViewModels;
using System.Collections.Generic;
using System.EDTF;
using System.Windows;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class ComposerEraViewModelBuilder
    {

        private static readonly Dictionary<string, SolidColorBrush> eraBrushes = new Dictionary<string, SolidColorBrush> {
            { "Medieval", new SolidColorBrush(Color.FromRgb(153, 153, 153)) },
            { "Renaissance", new SolidColorBrush(Color.FromRgb(155, 128, 181)) },
            { "Baroque", new SolidColorBrush(Color.FromRgb(204, 77, 77)) },
            { "Classical", new SolidColorBrush(Color.FromRgb(51, 151, 193)) },
            { "Romantic", new SolidColorBrush(Color.FromRgb(69, 168, 90)) },
            { "20th Century", new SolidColorBrush(Color.FromRgb(160, 118, 88)) },
            { "21st Century", new SolidColorBrush(Color.FromRgb(74, 142, 165)) }
        };

        public static List<ComposerEraViewModel> Build(IEnumerable<Era> eras)
        {
            var composerEraViewModels = new List<ComposerEraViewModel>();

            foreach (var era in eras)
            {
                var musicEra = new ComposerEraViewModel(era.Name, ExtendedDateTimeInterval.Parse(era.Dates), eraBrushes[era.Name], Brushes.White);

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