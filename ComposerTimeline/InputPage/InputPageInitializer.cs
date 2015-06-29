using Database;
using Luminescence.Xiph;
using NathanHarrenstein.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;

namespace NathanHarrenstein.ComposerTimeline
{
    public static class InputPageInitializer
    {
        public static void Initialize(InputPage inputPage)
        {
            InitializeDataSets();
            InitializeListBoxes(inputPage);

            Func<object, string> stringSelector = o =>
            {
                return ((Location)o).Name;
            };

            inputPage.ComposerBirthLocationAutoCompleteBox.StringSelector = inputPage.ComposerDeathLocationAutoCompleteBox.StringSelector = inputPage.RecordingLocationAutoCompleteBox.StringSelector = stringSelector;

            var xamlString = "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:data=\"clr-namespace:Database;assembly=Database\" DataType=\"{x:Type data:Location}\">"
                           + "<TextBlock Text=\"{Binding Name}\" />"
                           + "</DataTemplate>";
            var stringReader = new StringReader(xamlString);
            var xmlReader = XmlReader.Create(stringReader);
            inputPage.ComposerBirthLocationAutoCompleteBox.SuggestionTemplate = inputPage.ComposerDeathLocationAutoCompleteBox.SuggestionTemplate = inputPage.RecordingLocationAutoCompleteBox.SuggestionTemplate = (DataTemplate)XamlReader.Load(xmlReader);
        }

        private static Recording GetRecording(string selectedFilePath)
        {
            var flacTagger = new FlacTagger(selectedFilePath);
            var tags = flacTagger.GetAllTags();
            List<string> recordingIDStrings;

            if (tags.TryGetValue("RecordingID", out recordingIDStrings))
            {
                recordingIDStrings.FirstOrDefault();
            }

            App.DataProvider.Recordings.Load();

            if (recordingIDStrings != null)
            {
                var recordingID = int.Parse(recordingIDStrings[0]);

                return App.DataProvider.Recordings.Local.First(r => r.ID == recordingID);
            }

            return null;
        }

        private static void InitializeDataSets()
        {
            App.DataProvider.Composers.Load();
            App.DataProvider.Nationalities.Load();
            App.DataProvider.Eras.Load();
            App.DataProvider.Locations.Load();
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