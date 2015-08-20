using NathanHarrenstein.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.MusicTimeline.Utilities;
using NathanHarrenstein.MusicTimeline.ViewModels;
using System;
using System.Collections.Generic;
using System.EDTF;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class ComposerEventViewModelBuilder
    {
        public static List<ComposerEventViewModel> Build(IEnumerable<Composer> composers, IEnumerable<ComposerEraViewModel> musicEras, Timeline.Timeline timeline)
        {
            var eventList = new List<ComposerEventViewModel>();

            foreach (var composer in composers)
            {
                var background = (Brush)null;
                var composerEras = new List<ComposerEraViewModel>();

                foreach (var era in composer.Eras)
                {
                    foreach (var musicEra in musicEras)
                    {
                        if (era.Name == musicEra.Label)
                        {
                            composerEras.Add(musicEra);
                        }
                    }
                }

                var composerEraCount = composerEras.Count;

                if (composerEraCount > 1)
                {
                    var linearGradientBrush = new LinearGradientBrush();
                    linearGradientBrush.StartPoint = new Point(0, 0.5);
                    linearGradientBrush.EndPoint = new Point(1, 0.5);

                    for (int i = 0; i < composerEraCount; i++)
                    {
                        linearGradientBrush.GradientStops.Add(new GradientStop(((SolidColorBrush)composerEras[i].Background).Color, i / (composerEraCount - 1)));
                    }

                    background = linearGradientBrush;
                }
                else if (composerEraCount == 1)
                {
                    background = composerEras[0].Background;
                }
                else
                {
                    background = Brushes.Black;
                }

                var composerEvent = new ComposerEventViewModel(
                    NameUtility.ToFirstLast(composer.Name),
                    ExtendedDateTimeInterval.Parse(composer.Dates),
                    composer,
                    background,
                    Brushes.White,
                    composerEras,
                    GetCommand(composer, timeline));

                eventList.Add(composerEvent);
            }

            return eventList.OrderBy(e => e.Dates.Earliest()).ToList();
        }

        private static DelegateCommand GetCommand(Composer composer, Timeline.Timeline timeline)
        {
            Action<object> command = o =>
            {
                Application.Current.Properties["SelectedComposer"] = composer.Name;
                Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
                Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

                ((NavigationWindow)Application.Current.MainWindow).Navigate(new Uri("pack://application:,,,/Views/ComposerPage.xaml"));
            };

            return new DelegateCommand(command);
        }
    }
}