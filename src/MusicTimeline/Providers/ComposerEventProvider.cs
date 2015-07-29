using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.MusicTimeline.Utilities;
using NathanHarrenstein.MusicTimeline.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.EDTF;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class ComposerEventProvider
    {       
        public static IList GetComposerEvents(DataProvider dataProvider, IList<ComposerEraViewModel> musicEras, Timeline.Timeline timeline)
        {
            var eventList = new List<ComposerEventViewModel>();

            foreach (var composer in dataProvider.Composers)
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
                    background = new LinearGradientBrush { StartPoint = new Point(0, 0.5), EndPoint = new Point(1, 0.5) };

                    for (int i = 0; i < composerEraCount; i++)
                    {
                        ((LinearGradientBrush)background).GradientStops.Add(new GradientStop(((SolidColorBrush)composerEras[i].Background).Color, i / (composerEraCount - 1)));
                    }
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
                    GetBorn(composer),
                    GetDied(composer),
                    composer,
                    background,
                    Brushes.White,
                    composerEras,
                    GetCommand(composer, timeline), null);

                eventList.Add(composerEvent);
            }

            return eventList.OrderBy(e => e.Dates.Earliest()).ToList();
        }

        private static string GetBorn(Composer composer)
        {
            if (composer.BirthLocation != null)
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).Start}; {composer.BirthLocation.Name}";
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).Start.ToString();
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

        private static string GetDied(Composer composer)
        {
            if (composer.DeathLocation != null)
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).End}; {composer.DeathLocation.Name}";
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).End.ToString();
        }

        
    }
}