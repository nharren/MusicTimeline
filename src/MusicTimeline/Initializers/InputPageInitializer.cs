using Luminescence.Xiph;
using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Utilities;
using NathanHarrenstein.MusicTimeline.Views;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.MusicTimeline.Initializers
{
    public static class InputPageInitializer
    {
        public static void Initialize(InputPage inputPage)
        {
            InitializeDataSources();
            InitializeListBoxes(inputPage);
            InitializeAutoCompleteBoxStringSelectors(inputPage);
            InitializeAutoCompleteBoxSuggestionTemplates(inputPage);
        }

        private static Recording GetRecordingFromFilePath(string selectedFilePath)
        {
            var flacTagger = new FlacTagger(selectedFilePath);
            var recordingIDTag = flacTagger.GetAllTags().FirstOrDefault(t => t.Key == "RecordingId");
            string recordingIDString = null;

            if (recordingIDTag.Value.Count > 0)
            {
                recordingIDString = recordingIDTag.Value.First();
            }

            int recordingID;

            if (int.TryParse(recordingIDString, out recordingID))
            {
                return App.DataProvider.Recordings.Local.First(r => r.ID == recordingID);
            }

            return null;
        }

        private static void InitializeAutoCompleteBoxStringSelectors(InputPage inputPage)
        {
            var stringSelector = new Func<object, string>(o => ((Location)o).Name);

            inputPage.ComposerBirthLocationAutoCompleteBox.StringSelector = stringSelector;
            inputPage.ComposerDeathLocationAutoCompleteBox.StringSelector = stringSelector;
        }

        private static void InitializeAutoCompleteBoxSuggestionTemplates(InputPage inputPage)
        {
            var suggestionTemplate = (DataTemplate)inputPage.FindResource("SuggestionTemplate");

            inputPage.ComposerBirthLocationAutoCompleteBox.SuggestionTemplate = suggestionTemplate;
            inputPage.ComposerDeathLocationAutoCompleteBox.SuggestionTemplate = suggestionTemplate;
        }

        private static void InitializeDataSources()
        {
            App.DataProvider.Composers.Load();
            App.DataProvider.Nationalities.Load();
            App.DataProvider.Eras.Load();
            App.DataProvider.Locations.Load();
            App.DataProvider.Recordings.Load();
            App.DataProvider.Performers.Load();
        }

        private static void InitializeListBoxes(InputPage inputPage)
        {
            inputPage.ComposerListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(App.DataProvider.Composers.Local, null, "Name"));
            inputPage.ComposerNationalityListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(App.DataProvider.Nationalities.Local, null, "Name"));
            inputPage.ComposerEraListBox.ItemsSource = App.DataProvider.Eras.Local;
            inputPage.ComposerBirthLocationAutoCompleteBox.Suggestions = App.DataProvider.Locations.Local;
            inputPage.ComposerDeathLocationAutoCompleteBox.Suggestions = App.DataProvider.Locations.Local;
        }
    }
}