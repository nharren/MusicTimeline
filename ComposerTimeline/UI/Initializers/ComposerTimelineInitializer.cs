using NathanHarrenstein.ComposerTimeline.Data;
using NathanHarrenstein.ComposerTimeline.Database;
using NathanHarrenstein.Services;
using NathanHarrenstein.Timeline;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace NathanHarrenstein.ComposerTimeline.UI.Initializers
{
    public static class ComposerTimelineInitializer
    {
        public static void Initialize(ComposerTimeline timeline)
        {
            timeline.Start = Properties.Settings.Default.ComposerTimelineStart;
            timeline.End = Properties.Settings.Default.ComposerTimelineEnd;
            timeline.Eras = GetEras();
            timeline.EraSettings = GetEraSettings();

            GetEvents(timeline.Eras);
        }

        private static List<Era> GetEras()
        {
            var eras = new List<Era>();

            var medievalEra = new Era();
            var renaissanceEra = new Era();
            var baroqueEra = new Era();
            var classicalEra = new Era();
            var romanticEra = new Era();
            var modernEra = new Era();

            medievalEra.Name = "Medieval";
            medievalEra.Start = 1000;
            medievalEra.End = 1400;

            renaissanceEra.Name = "Renaissance";
            renaissanceEra.Start = 1400;
            renaissanceEra.End = 1600;

            baroqueEra.Name = "Baroque";
            baroqueEra.Start = 1600;
            baroqueEra.End = 1750;

            classicalEra.Name = "Classical";
            classicalEra.Start = 1750;
            classicalEra.End = 1830;

            romanticEra.Name = "Romantic";
            romanticEra.Start = 1830;
            romanticEra.End = 1900;

            modernEra.Name = "Modern";
            modernEra.Start = 1900;
            modernEra.End = 2015;

            eras.Add(medievalEra);
            eras.Add(renaissanceEra);
            eras.Add(baroqueEra);
            eras.Add(classicalEra);
            eras.Add(romanticEra);
            eras.Add(modernEra);

            return eras;
        }

        private static List<EraSettings> GetEraSettings()
        {
            var eraSettings = new List<EraSettings>();

            var medievalEraSettings = new EraSettings();
            var renaissanceEraSettings = new EraSettings();
            var baroqueEraSettings = new EraSettings();
            var classicalEraSettings = new EraSettings();
            var romanticEraSettings = new EraSettings();
            var modernEraSettings = new EraSettings();

            medievalEraSettings.Era = "Medieval";
            medievalEraSettings.Brush = new SolidColorBrush(Color.FromRgb(175, 175, 175));
            medievalEraSettings.TextBrush = Brushes.White;

            renaissanceEraSettings.Era = "Renaissance";
            renaissanceEraSettings.Brush = new SolidColorBrush(Color.FromRgb(181, 126, 220));
            renaissanceEraSettings.TextBrush = Brushes.White;

            baroqueEraSettings.Era = "Baroque";
            baroqueEraSettings.Brush = new SolidColorBrush(Color.FromRgb(208, 81, 75));
            baroqueEraSettings.TextBrush = Brushes.White;

            classicalEraSettings.Era = "Classical";
            classicalEraSettings.Brush = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            classicalEraSettings.TextBrush = Brushes.White;

            romanticEraSettings.Era = "Romantic";
            romanticEraSettings.Brush = new SolidColorBrush(Color.FromRgb(90, 170, 90));
            romanticEraSettings.TextBrush = Brushes.White;

            modernEraSettings.Era = "Modern";
            modernEraSettings.Brush = new SolidColorBrush(Color.FromRgb(160, 120, 90));
            modernEraSettings.TextBrush = Brushes.White;

            eraSettings.Add(medievalEraSettings);
            eraSettings.Add(renaissanceEraSettings);
            eraSettings.Add(baroqueEraSettings);
            eraSettings.Add(classicalEraSettings);
            eraSettings.Add(romanticEraSettings);
            eraSettings.Add(modernEraSettings);

            return eraSettings;
        }

        private static void GetEvents(List<Era> eras)
        {
            var db = new DataProvider();

            foreach (var composer in db.Composers)
            {
                var era = composer.ComposerProperties.FirstOrDefault(x => x.Key == "Era");
                var eraName = "Unknown";

                if (era != null && !string.IsNullOrEmpty(era.Value) && eras.Exists(e => e.Name == era.Value))
                {
                    eraName = era.Value;
                }

                var composerEvent = new ComposerEvent();

                composerEvent.Composer = composer;

                var composerNameProperty = composer.ComposerProperties.FirstOrDefault(x => x.Key == "Name");

                if (composerNameProperty != null)
                {
                    composerEvent.Name = composerNameProperty.Value;
                }
                else
                {
                    var logger = new Logger();

                    logger.Log("The Composer with the ID, " + composer.ID + ", does not have a \"Name\" property.", "ComposerTimeline.log");
                }

                var composerBirthdayProperty = composer.ComposerProperties.FirstOrDefault(x => x.Key == "Birthday");

                if (composerBirthdayProperty != null && !string.IsNullOrEmpty(composerBirthdayProperty.Value))
                {
                    double composerBirthday;

                    if (double.TryParse(composerBirthdayProperty.Value, out composerBirthday))
                    {
                        composerEvent.Start = composerBirthday;
                    }
                    else
                    {
                        var logger = new Logger();

                        logger.Log(composerEvent.Name + "'s birthday could not be read.", "ComposerTimeline.log");
                    }
                }
                else
                {
                    var logger = new Logger();

                    logger.Log(composerEvent.Name + " does not have a birthday.", "ComposerTimeline.log");
                }

                var composerDeathdayProperty = composer.ComposerProperties.FirstOrDefault(x => x.Key == "Deathday");

                if (composerDeathdayProperty != null && !string.IsNullOrEmpty(composerDeathdayProperty.Value))
                {
                    double composerDeathday;

                    if (double.TryParse(composerDeathdayProperty.Value, out composerDeathday))
                    {
                        composerEvent.End = composerDeathday;
                    }
                    else
                    {
                        var logger = new Logger();

                        logger.Log(composerEvent.Name + "'s deathday could not be read.", "ComposerTimeline.log");
                    }
                }
                else if (composerEvent.Start != 0d)
                {
                    composerEvent.End = Properties.Settings.Default.ComposerTimelineEnd;
                }
                else
                {
                    var logger = new Logger();

                    logger.Log(composerEvent.Name + " does not have a deathday.", "ComposerTimeline.log");
                }

                var eraSearch = eras.FirstOrDefault(e => e.Name == eraName);

                if (eraSearch != null)
                {
                    eraSearch.Events.Add(composerEvent);
                }
            }

            foreach (var era in eras)
            {
                era.Events.Sort((a, b) => { return a.Start.CompareTo(b.Start); });
            }
        }
    }
}