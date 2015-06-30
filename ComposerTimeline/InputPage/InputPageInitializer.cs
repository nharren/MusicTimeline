using Database;
using Luminescence.Xiph;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NathanHarrenstein.ComposerTimeline
{
    public static class InputPageInitializer
    {
        public static void Initialize(InputPage inputPage)
        {
            InitializeDataSources();
            InitializeListBoxes(inputPage);
            InitializeAutoCompleteBoxes(inputPage);
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

        private static void InitializeAutoCompleteBoxes(InputPage inputPage)
        {
            var stringSelector = new Func<object, string>(o => ((Location)o).Name);

            inputPage.ComposerBirthLocationAutoCompleteBox.StringSelector = stringSelector;
            inputPage.ComposerDeathLocationAutoCompleteBox.StringSelector = stringSelector;
            inputPage.RecordingLocationAutoCompleteBox.StringSelector = stringSelector;

            var suggestionTemplate = (DataTemplate)inputPage.FindResource("SuggestionTemplate");

            inputPage.ComposerBirthLocationAutoCompleteBox.SuggestionTemplate = suggestionTemplate;
            inputPage.ComposerDeathLocationAutoCompleteBox.SuggestionTemplate = suggestionTemplate;
            inputPage.RecordingLocationAutoCompleteBox.SuggestionTemplate = suggestionTemplate;
        }

        private static void InitializeDataSources()
        {
            App.DataProvider.Composers.Load();
            App.DataProvider.Nationalities.Load();
            App.DataProvider.Eras.Load();
            App.DataProvider.Locations.Load();
            App.DataProvider.Recordings.Load();
        }

        private static void InitializeListBoxes(InputPage inputPage)
        {
            var collectionViewSource = new CollectionViewSource();
            collectionViewSource.Source = App.DataProvider.Composers.Local;
            collectionViewSource.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            var composersBinding = new Binding();
            composersBinding.Source = collectionViewSource;
            inputPage.ComposerListBox.SetBinding(ItemsControl.ItemsSourceProperty, composersBinding);

            inputPage.ComposerNationalityListBox.ItemsSource = App.DataProvider.Nationalities.Local;
            inputPage.ComposerEraListBox.ItemsSource = App.DataProvider.Eras.Local;
            inputPage.ComposerBirthLocationAutoCompleteBox.Suggestions = App.DataProvider.Locations.Local;
            inputPage.ComposerDeathLocationAutoCompleteBox.Suggestions = App.DataProvider.Locations.Local;
            inputPage.RecordingLocationAutoCompleteBox.Suggestions = App.DataProvider.Locations.Local;
        }
    }
}