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

        internal static void BindComposers(InputPage inputPage, IList<Composer> composers)
        {
            if (composers.Count == 1)
            {
                var composer = composers.First();

                var composerNameBinding = new Binding("Name");
                composerNameBinding.Source = composer;
                composerNameBinding.Mode = BindingMode.TwoWay;
                composerNameBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                inputPage.ComposerNameTextBox.SetBinding(TextBox.TextProperty, composerNameBinding);
                inputPage.ComposerNameTextBox.IsEnabled = true;

                var composerDatesBinding = new Binding("Dates");
                composerDatesBinding.Source = composer;
                composerDatesBinding.Mode = BindingMode.TwoWay;
                composerDatesBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                inputPage.ComposerDatesTextBox.SetBinding(TextBox.TextProperty, composerDatesBinding);
                inputPage.ComposerDatesTextBox.IsEnabled = true;

                if (composer.BirthLocation != null)
                {
                    inputPage.ComposerBirthLocationAutoCompleteBox.Text = composer.BirthLocation.Name;
                }
                else
                {
                    inputPage.ComposerBirthLocationAutoCompleteBox.Text = null;
                }

                inputPage.ComposerBirthLocationAutoCompleteBox.IsEnabled = true;

                if (composer.DeathLocation != null)
                {
                    inputPage.ComposerDeathLocationAutoCompleteBox.Text = composer.DeathLocation.Name;
                }
                else
                {
                    inputPage.ComposerDeathLocationAutoCompleteBox.Text = null;
                }

                inputPage.ComposerDeathLocationAutoCompleteBox.IsEnabled = true;

                inputPage.ComposerNationalityListBox.SelectedItems.Clear();

                foreach (var nationality in composer.Nationalities)
                {
                    inputPage.ComposerNationalityListBox.SelectedItems.Add(nationality);
                }

                inputPage.ComposerNationalityListBox.IsEnabled = true;

                inputPage.ComposerEraListBox.SelectedItems.Clear();

                foreach (var era in composer.Eras)
                {
                    inputPage.ComposerEraListBox.SelectedItems.Add(era);
                }

                inputPage.ComposerEraListBox.IsEnabled = true;

                inputPage.ComposerInfluenceListBox.ItemsSource = new List<Composer>(composer.Influences);
                inputPage.ComposerInfluenceListBox.IsEnabled = true;

                var composerImagesBinding = new Binding("ComposerImages");
                composerImagesBinding.Source = composer;
                composerImagesBinding.Converter = new ComposerImageConverter();
                inputPage.ComposerImageListBox.SetBinding(ItemsControl.ItemsSourceProperty, composerImagesBinding);
                inputPage.ComposerImageListBox.IsEnabled = true;

                var composerLinksBinding = new Binding("ComposerLinks");
                composerLinksBinding.Source = composer;
                composerLinksBinding.Converter = new ComposerLinkConverter();
                inputPage.ComposerLinkListBox.SetBinding(ItemsControl.ItemsSourceProperty, composerLinksBinding);
                inputPage.ComposerLinkListBox.IsEnabled = true;

                var composerBiographyBinding = new Binding("Biography");
                composerBiographyBinding.Source = composer;
                composerBiographyBinding.Mode = BindingMode.TwoWay;
                composerBiographyBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                inputPage.ComposerBiographyTextBox.SetBinding(TextBox.TextProperty, composerBiographyBinding);
                inputPage.ComposerBiographyTextBox.IsEnabled = true;

                inputPage.CompositionCollectionListBox.ItemsSource = new List<CompositionCollection>(composer.CompositionCollections);
                inputPage.CompositionListBox.ItemsSource = composer.Compositions;
            }
            else if (composers.Count > 1)
            {
                inputPage.ComposerNameTextBox.IsEnabled = false;
                inputPage.ComposerNameTextBox.Text = "<< Multiple Selection >>";

                inputPage.ComposerDatesTextBox.IsEnabled = false;
                inputPage.ComposerDatesTextBox.Text = "<< Multiple Selection >>";

                inputPage.ComposerBirthLocationAutoCompleteBox.IsEnabled = false;
                inputPage.ComposerBirthLocationAutoCompleteBox.Text = "<< Multiple Selection >>";

                inputPage.ComposerDeathLocationAutoCompleteBox.IsEnabled = false;
                inputPage.ComposerDeathLocationAutoCompleteBox.Text = "<< Multiple Selection >>";

                inputPage.ComposerNationalityListBox.IsEnabled = false;
                inputPage.ComposerNationalityListBox.SelectedItems.Clear();

                inputPage.ComposerEraListBox.IsEnabled = false;
                inputPage.ComposerEraListBox.SelectedItems.Clear();

                inputPage.ComposerInfluenceListBox.IsEnabled = false;
                inputPage.ComposerInfluenceListBox.ItemsSource = null;

                inputPage.ComposerImageListBox.IsEnabled = false;
                inputPage.ComposerImageListBox.ItemsSource = null;

                inputPage.ComposerLinkListBox.IsEnabled = false;
                inputPage.ComposerLinkListBox.ItemsSource = null;

                inputPage.ComposerBiographyTextBox.IsEnabled = false;
                inputPage.ComposerBiographyTextBox.Text = "<< Multiple Selection >>";

                IEnumerable<CompositionCollection> commonCompositionCollections = composers[0].CompositionCollections;

                for (int i = 1; i < composers.Count; i++)
                {
                    commonCompositionCollections = commonCompositionCollections.Intersect(composers[i].CompositionCollections);
                }

                inputPage.CompositionCollectionListBox.ItemsSource = commonCompositionCollections;

                IEnumerable<Composition> commonCompositions = composers[0].Compositions;

                for (int i = 1; i < composers.Count; i++)
                {
                    commonCompositions = commonCompositions.Intersect(composers[i].Compositions);
                }

                inputPage.CompositionListBox.ItemsSource = commonCompositions;
            }
            else
            {
                inputPage.ComposerNameTextBox.IsEnabled = false;
                inputPage.ComposerNameTextBox.Text = string.Empty;

                inputPage.ComposerDatesTextBox.IsEnabled = false;
                inputPage.ComposerDatesTextBox.Text = string.Empty;

                inputPage.ComposerBirthLocationAutoCompleteBox.IsEnabled = false;
                inputPage.ComposerBirthLocationAutoCompleteBox.Text = string.Empty;

                inputPage.ComposerDeathLocationAutoCompleteBox.IsEnabled = false;
                inputPage.ComposerDeathLocationAutoCompleteBox.Text = string.Empty;

                inputPage.ComposerNationalityListBox.IsEnabled = false;
                inputPage.ComposerNationalityListBox.SelectedItems.Clear();

                inputPage.ComposerEraListBox.IsEnabled = false;
                inputPage.ComposerEraListBox.SelectedItems.Clear();

                inputPage.ComposerInfluenceListBox.IsEnabled = false;
                inputPage.ComposerInfluenceListBox.ItemsSource = null;

                inputPage.ComposerImageListBox.IsEnabled = false;
                inputPage.ComposerImageListBox.ItemsSource = null;

                inputPage.ComposerLinkListBox.IsEnabled = false;
                inputPage.ComposerLinkListBox.ItemsSource = null;

                inputPage.ComposerBiographyTextBox.IsEnabled = false;
                inputPage.ComposerBiographyTextBox.Text = string.Empty;

                inputPage.CompositionCollectionListBox.ItemsSource = null;
                inputPage.CompositionListBox.ItemsSource = null;
            }
        }

        internal static void BindComposition(InputPage inputPage, Composition composition)
        {
            if (composition == null)
            {
                inputPage.CompositionNameTextBox.IsEnabled = false;
                inputPage.CompositionNameTextBox.Text = null;

                inputPage.CompositionNicknameTextBox.IsEnabled = false;
                inputPage.CompositionNicknameTextBox.Text = null;

                inputPage.CompositionDatesTextBox.IsEnabled = false;
                inputPage.CompositionDatesTextBox.Text = null;

                inputPage.CompositionNameTextBox.IsEnabled = false;
                inputPage.CompositionNameTextBox.Text = null;

                inputPage.CompositionCatalogPrefixListBox.IsEnabled = false;
                inputPage.CompositionCatalogPrefixListBox.ItemsSource = null;

                inputPage.CompositionCatalogNumberTextBox.IsEnabled = false;
                inputPage.CompositionCatalogNumberTextBox.Text = null;

                inputPage.MovementListBox.IsEnabled = false;
                inputPage.MovementListBox.ItemsSource = null;

                return;
            }

            var compositionNameBinding = new Binding("Name");
            compositionNameBinding.Mode = BindingMode.TwoWay;
            compositionNameBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            compositionNameBinding.Source = composition;

            inputPage.CompositionNameTextBox.SetBinding(TextBox.TextProperty, compositionNameBinding);
            inputPage.CompositionNameTextBox.IsEnabled = true;

            var compositionNicknameBinding = new Binding("Nickname");
            compositionNicknameBinding.Mode = BindingMode.TwoWay;
            compositionNicknameBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            compositionNicknameBinding.Source = composition;

            inputPage.CompositionNicknameTextBox.SetBinding(TextBox.TextProperty, compositionNicknameBinding);
            inputPage.CompositionNicknameTextBox.IsEnabled = true;

            var compositionDatesBinding = new Binding("Dates");
            compositionDatesBinding.Mode = BindingMode.TwoWay;
            compositionDatesBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            compositionDatesBinding.Source = composition;

            inputPage.CompositionDatesTextBox.SetBinding(TextBox.TextProperty, compositionDatesBinding);
            inputPage.CompositionDatesTextBox.IsEnabled = true;

            inputPage.CompositionCatalogPrefixListBox.ItemsSource = composition.Composers
                .SelectMany(c => c.CompositionCatalogs)
                .Select(cc => cc.Prefix)
                .ToList();
            inputPage.CompositionCatalogPrefixListBox.IsEnabled = true;

            var catalogNumber = composition.CatalogNumbers
                .FirstOrDefault(cn => cn.CompositionCatalog.Prefix == (string)inputPage.CompositionCatalogPrefixListBox.SelectedItem);
            inputPage.CompositionCatalogNumberTextBox.Text = catalogNumber == null ? null : catalogNumber.Number;
            inputPage.CompositionCatalogNumberTextBox.IsEnabled = true;

            inputPage.MovementListBox.ItemsSource = composition.Movements;
        }

        internal static void BindCompositionCollection(InputPage inputPage, CompositionCollection compositionCollection)
        {
            if (compositionCollection == null)
            {
                inputPage.CompositionCollectionNameTextBox.IsEnabled = false;
                inputPage.CompositionCollectionNameTextBox.Text = null;

                inputPage.CompositionCollectionCatalogPrefixListBox.IsEnabled = false;
                inputPage.CompositionCollectionCatalogPrefixListBox.ItemsSource = null;

                inputPage.CompositionCollectionCatalogNumberTextBox.IsEnabled = false;
                inputPage.CompositionCollectionCatalogNumberTextBox.Text = null;

                inputPage.CompositionListBox.ItemsSource = new List<Composition>(inputPage.CurrentComposers.SelectMany(c => c.Compositions));

                return;
            }

            var compositionCollectionNameBinding = new Binding("Name");
            compositionCollectionNameBinding.Mode = BindingMode.TwoWay;
            compositionCollectionNameBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            compositionCollectionNameBinding.Source = compositionCollection;

            inputPage.CompositionCollectionNameTextBox.SetBinding(TextBox.TextProperty, compositionCollectionNameBinding);
            inputPage.CompositionCollectionNameTextBox.IsEnabled = true;

            inputPage.CompositionCollectionCatalogPrefixListBox.ItemsSource = new List<string>(compositionCollection.Composers.SelectMany(c => c.CompositionCatalogs).Select(cc => cc.Prefix));
            inputPage.CompositionCollectionCatalogPrefixListBox.IsEnabled = true;

            var catalogNumber = compositionCollection.CatalogNumbers
                .FirstOrDefault(cn => cn.CompositionCatalog.Prefix == (string)inputPage.CompositionCollectionCatalogPrefixListBox.SelectedItem);
            inputPage.CompositionCollectionCatalogNumberTextBox.Text = catalogNumber == null ? null : catalogNumber.Number;
            inputPage.CompositionCollectionCatalogNumberTextBox.IsEnabled = true;

            inputPage.CompositionListBox.ItemsSource = new List<Composition>(compositionCollection.Compositions);
            inputPage.CompositionListBox.IsEnabled = true;
        }

        internal static void BindMovement(InputPage inputPage, Movement movement)
        {
            var movementNumberBinding = new Binding("Number");
            movementNumberBinding.Mode = BindingMode.TwoWay;
            movementNumberBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            movementNumberBinding.Source = movement;
            inputPage.MovementNumberBox.SetBinding(TextBox.TextProperty, movementNumberBinding);

            var movementNameBinding = new Binding("Name");
            movementNameBinding.Mode = BindingMode.TwoWay;
            movementNameBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            movementNameBinding.Source = movement;
            inputPage.MovementNameAutoCompleteBox.SetBinding(AutoCompleteBox.TextProperty, movementNameBinding);

            inputPage.RecordingListBox.ItemsSource = movement.Recordings;
        }

        internal static void BindRecording(InputPage inputPage, Recording recording)
        {
            inputPage.RecordingPerformerListBox.ItemsSource = recording.Performers;

            if (recording.Album != null)
            {
                inputPage.RecordingAlbumAutoCompleteBox.Text = recording.Album.Name;
            }

            var trackNumberBinding = new Binding("TrackNumber");
            trackNumberBinding.Mode = BindingMode.TwoWay;
            trackNumberBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            trackNumberBinding.Source = recording;
            inputPage.RecordingTrackNumberBox.SetBinding(TextBox.TextProperty, trackNumberBinding);

            var datesBinding = new Binding("Dates");
            datesBinding.Mode = BindingMode.TwoWay;
            datesBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            datesBinding.Source = recording;
            inputPage.RecordingDatesTextBox.SetBinding(TextBox.TextProperty, datesBinding);

            if (recording.Location != null)
            {
                inputPage.RecordingLocationAutoCompleteBox.Text = recording.Location.Name;
            }
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