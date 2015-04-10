using NathanHarrenstein.ComposerTimeline.Data;
using NathanHarrenstein.Services;
using NathanHarrenstein.Timeline;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ExtendedDateTimeFormat;
using System.Linq;
using System.Windows.Media;

namespace NathanHarrenstein.ComposerTimeline.UI.Initializers
{
    public static class ComposerTimelineInitializer
    {
        public static void Initialize(ComposerTimeline timeline)
        {
            timeline.Start = 500;
            timeline.End = DateTime.Now.Year;
            timeline.Eras = GetEras();
            timeline.EraSettings = GetEraSettings();

            GetEvents(timeline.Eras);
        }

        private static IEnumerable<Era> GetEras()
        {
            using (var dataProvider = new Database.DataProvider())
            {
                return dataProvider.Eras.Select(e => new Era { Name = e.Name } );
            }
        }

        private static List<EraSettings> GetEraSettings()
        {
            var eraSettings = new List<EraSettings>();

            var medievalEraSettings = new EraSettings();
            medievalEraSettings.Era = "Medieval";
            medievalEraSettings.Brush = new SolidColorBrush(Color.FromRgb(153, 153, 153));           // #FF999999
            medievalEraSettings.TextBrush = Brushes.White;

            var renaissanceEraSettings = new EraSettings();
            renaissanceEraSettings.Era = "Renaissance";
            renaissanceEraSettings.Brush = new SolidColorBrush(Color.FromRgb(155, 128, 181));        // #FF9B80B5
            renaissanceEraSettings.TextBrush = Brushes.White;

            var baroqueEraSettings = new EraSettings();
            baroqueEraSettings.Era = "Baroque";
            baroqueEraSettings.Brush = new SolidColorBrush(Color.FromRgb(204, 77, 77));              // #FFCC4D4D
            baroqueEraSettings.TextBrush = Brushes.White;

            var classicalEraSettings = new EraSettings();
            classicalEraSettings.Era = "Classical";
            classicalEraSettings.Brush = new SolidColorBrush(Color.FromRgb(51, 151, 193));           // #FF3397C1
            classicalEraSettings.TextBrush = Brushes.White;

            var romanticEraSettings = new EraSettings();
            romanticEraSettings.Era = "Romantic";
            romanticEraSettings.Brush = new SolidColorBrush(Color.FromRgb(69, 168, 90));             // #FF45A85A
            romanticEraSettings.TextBrush = Brushes.White;

            var twentiethCenturyEraSettings = new EraSettings();
            twentiethCenturyEraSettings.Era = "20th Century";
            twentiethCenturyEraSettings.Brush = new SolidColorBrush(Color.FromRgb(205, 173, 74));    // #FFCDAD4A
            twentiethCenturyEraSettings.TextBrush = Brushes.White;

            var twentyFirstCenturyEraSettings = new EraSettings();
            twentyFirstCenturyEraSettings.Era = "21st Century";
            twentyFirstCenturyEraSettings.Brush = new SolidColorBrush(Color.FromRgb(219, 109, 138)); // #FFDB6D8A
            twentyFirstCenturyEraSettings.TextBrush = Brushes.White;

            eraSettings.Add(medievalEraSettings);
            eraSettings.Add(renaissanceEraSettings);
            eraSettings.Add(baroqueEraSettings);
            eraSettings.Add(classicalEraSettings);
            eraSettings.Add(romanticEraSettings);
            eraSettings.Add(twentiethCenturyEraSettings);
            eraSettings.Add(twentyFirstCenturyEraSettings);

            return eraSettings;
        }

        private static void GetEvents(List<Era> timelineEras)
        {
            using (var dataProvider = new Database.DataProvider())
            {
                foreach (var composer in dataProvider.Composers)
                {
                    var composerEras = composer.Eras;
                    var eraName = "Unknown";

                    if (composer.Eras.Count == 0)
                    {
                        eraName = era.Value;
                    }

                    var composerEvent = new ComposerEvent();

                    composerEvent.Composer = composer;
                    composerEvent.Name = composer.Name;
                    composerEvent.Dates = composer.Dates;

                    var eraSearch = composerEras.FirstOrDefault(e => e.Name == eraName);

                    if (eraSearch != null)
                    {
                        eraSearch.Events.Add(composerEvent);
                    }
                } 
            }

            foreach (var era in eras)
            {
                era.Events.Sort((a, b) => { return a.Start.CompareTo(b.Start); });
            }
        }
    }
}