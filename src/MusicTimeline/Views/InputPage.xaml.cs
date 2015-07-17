using Luminescence.Xiph;
using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Extensions;
using NathanHarrenstein.MusicTimeline.Providers;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class InputPage : Page
    {
        private ObservableCollection<Composer> _selectedComposers;
        private Composition _selectedComposition;
        private CompositionCollection _selectedCompositionCollection;
        private Movement _selectedMovement;
        private Recording _selectedRecording;
        private DataProvider _dataProvider;

        public InputPage()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Initialize();
            }
        }

        #region Initialization

        private void Initialize()
        {
            _dataProvider = new DataProvider();

            InitializeDataSources();
            InitializeListBoxes();
            InitializeAutoCompleteBoxStringSelectors();
            InitializeAutoCompleteBoxSuggestionTemplates();
        }

        private void InitializeAutoCompleteBoxStringSelectors()
        {
            var stringSelector = new Func<object, string>(o => ((Location)o).Name);

            ComposerBirthLocationAutoCompleteBox.StringSelector = stringSelector;
            ComposerDeathLocationAutoCompleteBox.StringSelector = stringSelector;
        }

        private void InitializeAutoCompleteBoxSuggestionTemplates()
        {
            var suggestionTemplate = (DataTemplate)FindResource("SuggestionTemplate");

            ComposerBirthLocationAutoCompleteBox.SuggestionTemplate = suggestionTemplate;
            ComposerDeathLocationAutoCompleteBox.SuggestionTemplate = suggestionTemplate;
        }

        private void InitializeDataSources()
        {
            _dataProvider.Composers.Load();
            _dataProvider.Eras.Load();
            _dataProvider.Locations.Load();
            _dataProvider.Nationalities.Load();
        }

        private void InitializeListBoxes()
        {
            ComposerListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_dataProvider.Composers.Local, null, "Name"));
            ComposerNationalityListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_dataProvider.Nationalities.Local, null, "Name"));
            ComposerEraListBox.ItemsSource = _dataProvider.Eras.Local;
            ComposerBirthLocationAutoCompleteBox.Suggestions = _dataProvider.Locations.Local;
            ComposerDeathLocationAutoCompleteBox.Suggestions = _dataProvider.Locations.Local;
        }

        #endregion Initialization

        #region Composer Section Methods

        public void ClearComposerSection()
        {
            ComposerNameTextBox.Text = string.Empty;
            ComposerDatesTextBox.Text = string.Empty;
            ComposerBirthLocationAutoCompleteBox.Text = string.Empty;
            ComposerDeathLocationAutoCompleteBox.Text = string.Empty;
            ComposerNationalityListBox.SelectedItems.Clear();
            ComposerEraListBox.SelectedItems.Clear();
            ComposerInfluenceListBox.ItemsSource = null;
            ComposerImageListBox.ItemsSource = null;
            ComposerLinkListBox.ItemsSource = null;
            ComposerBiographyTextBox.Text = string.Empty;
        }

        public void DisableComposerSection()
        {
            ComposerNameTextBox.IsEnabled = false;
            ComposerDatesTextBox.IsEnabled = false;
            ComposerBirthLocationAutoCompleteBox.IsEnabled = false;
            ComposerDeathLocationAutoCompleteBox.IsEnabled = false;
            ComposerNationalityListBox.IsEnabled = false;
            ComposerEraListBox.IsEnabled = false;
            ComposerInfluenceListBox.IsEnabled = false;
            ComposerImageListBox.IsEnabled = false;
            ComposerLinkListBox.IsEnabled = false;
            ComposerBiographyTextBox.IsEnabled = false;
        }

        public void EnableComposerSection()
        {
            ComposerNameTextBox.IsEnabled = true;
            ComposerDatesTextBox.IsEnabled = true;
            ComposerBirthLocationAutoCompleteBox.IsEnabled = true;
            ComposerDeathLocationAutoCompleteBox.IsEnabled = true;
            ComposerNationalityListBox.IsEnabled = true;
            ComposerEraListBox.IsEnabled = true;
            ComposerInfluenceListBox.IsEnabled = true;
            ComposerImageListBox.IsEnabled = true;
            ComposerLinkListBox.IsEnabled = true;
            ComposerBiographyTextBox.IsEnabled = true;
        }

        public void LoadComposerSection()
        {
            if (_selectedComposers.Count == 1)
            {
                ComposerNationalityListBox.SelectedItems.Clear();
                ComposerEraListBox.SelectedItems.Clear();

                var composer = _selectedComposers.First();

                ComposerNameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(composer, "Name"));
                ComposerDatesTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(composer, "Dates"));
                ComposerBirthLocationAutoCompleteBox.Text = composer.BirthLocation?.Name;
                ComposerDeathLocationAutoCompleteBox.Text = composer.DeathLocation?.Name;
                ComposerInfluenceListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(composer.Influences, null, "Name"));
                ComposerImageListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(composer.ComposerImages, null));
                ComposerLinkListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(composer.ComposerLinks, null));
                ComposerBiographyTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(composer, "Biography"));
                CompositionCollectionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(composer.CompositionCollections, null, "Name"));
                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(composer.Compositions, null, "Name"));

                ListUtility.AddMany(ComposerNationalityListBox.SelectedItems, composer.Nationalities.ToList());
                ListUtility.AddMany(ComposerEraListBox.SelectedItems, composer.Eras.ToList());

                if (ComposerImageListBox.Items.Count > 0)
                {
                    ComposerImageListBox.SelectedIndex = 0;
                }
            }
            else
            {
                ClearComposerSection();

                ComposerNameTextBox.Text = "<< Multiple Selection >>";
                ComposerDatesTextBox.Text = "<< Multiple Selection >>";
                ComposerBirthLocationAutoCompleteBox.Text = "<< Multiple Selection >>";
                ComposerDeathLocationAutoCompleteBox.Text = "<< Multiple Selection >>";
                ComposerBiographyTextBox.Text = "<< Multiple Selection >>";
                CompositionCollectionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_selectedComposers.Common(c => c.CompositionCollections), null, "Name"));
                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_selectedComposers.Common(c => c.Compositions), null, "Name"));
            }
        }

        public void RefreshComposerSection()
        {
            if (_selectedComposers.Count == 0)
            {
                DisableComposerSection();
                ClearComposerSection();

                _selectedCompositionCollection = null;

                RefreshCompositionCollectionSection();

                return;
            }

            LoadComposerSection();
            EnableComposerSection();
        }

        #endregion Composer Section Methods

        #region Composition Collection Section Methods

        public void ClearCompositionCollectionSection()
        {
            CompositionCollectionNameTextBox.Text = null;
            CompositionCollectionCatalogPrefixListBox.ItemsSource = null;
            CompositionCollectionCatalogNumberTextBox.Text = null;
        }

        public void DisableCompositionCollectionSection()
        {
            CompositionCollectionNameTextBox.IsEnabled = false;
            CompositionCollectionCatalogPrefixListBox.IsEnabled = false;
            CompositionCollectionCatalogNumberTextBox.IsEnabled = false;

            CompositionListBox.ItemsSource = new List<Composition>(_selectedComposers.SelectMany(c => c.Compositions));
        }

        public void EnableCompositionCollectionSection()
        {
            CompositionCollectionNameTextBox.IsEnabled = true;
            CompositionCollectionCatalogPrefixListBox.IsEnabled = true;
            CompositionCollectionCatalogNumberTextBox.IsEnabled = true;
            CompositionListBox.IsEnabled = true;
        }

        public void LoadCompositionCollectionSection()
        {
            CompositionCollectionNameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_selectedCompositionCollection, "Name"));
            CompositionCollectionCatalogPrefixListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_selectedCompositionCollection.Composers.SelectMany(c => c.CompositionCatalogs), null, "Prefix"));
            CompositionCollectionCatalogNumberTextBox.Text = _selectedCompositionCollection.CatalogNumbers.FirstOrDefault(cn => cn.CompositionCatalog.Prefix == (string)CompositionCollectionCatalogPrefixListBox.SelectedItem)?.Number;
            CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_selectedCompositionCollection.Compositions, null, "Name"));

            if (CompositionCollectionCatalogPrefixListBox.Items.Count > 0)
            {
                CompositionCollectionCatalogPrefixListBox.SelectedIndex = 0;
            }
        }

        public void RefreshCompositionCollectionSection()
        {
            if (_selectedCompositionCollection == null)
            {
                DisableCompositionCollectionSection();
                ClearCompositionCollectionSection();

                _selectedComposition = null;

                RefreshCompositionSection();

                return;
            }

            LoadCompositionCollectionSection();
            EnableCompositionCollectionSection();
        }

        #endregion Composition Collection Section Methods

        #region Composition Section Methods

        public void ClearCompositionSection()
        {
            CompositionNameTextBox.Text = null;
            CompositionNicknameTextBox.Text = null;
            CompositionDatesTextBox.Text = null;
            CompositionNameTextBox.Text = null;
            CompositionCatalogPrefixListBox.ItemsSource = null;
            CompositionCatalogNumberTextBox.Text = null;
            MovementListBox.ItemsSource = null;
        }

        public void DisableCompositionSection()
        {
            CompositionNameTextBox.IsEnabled = false;
            CompositionNicknameTextBox.IsEnabled = false;
            CompositionDatesTextBox.IsEnabled = false;
            CompositionNameTextBox.IsEnabled = false;
            CompositionCatalogPrefixListBox.IsEnabled = false;
            CompositionCatalogNumberTextBox.IsEnabled = false;
            MovementListBox.IsEnabled = false;
        }

        public void EnableCompositionSection()
        {
            CompositionNameTextBox.IsEnabled = true;
            CompositionNicknameTextBox.IsEnabled = true;
            CompositionDatesTextBox.IsEnabled = true;
            CompositionNameTextBox.IsEnabled = true;
            CompositionCatalogPrefixListBox.IsEnabled = true;
            CompositionCatalogNumberTextBox.IsEnabled = true;
            MovementListBox.IsEnabled = true;
        }

        public void LoadCompositionSection()
        {
            CompositionNameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_selectedComposition, "Name"));
            CompositionNicknameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_selectedComposition, "Nickname"));
            CompositionDatesTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_selectedComposition, "Dates"));
            CompositionCatalogNumberTextBox.Text = _selectedComposition.CatalogNumbers.FirstOrDefault(cn => cn.CompositionCatalog.Prefix == (string)CompositionCatalogPrefixListBox.SelectedItem)?.Number;
            MovementListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_selectedComposition.Movements, null, "Name"));

            if (_selectedCompositionCollection != null)
            {
                CompositionCatalogPrefixListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_selectedCompositionCollection.Composers.SelectMany(c => c.CompositionCatalogs), null, "Prefix"));
            }
            else
            {
                CompositionCatalogPrefixListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_selectedComposition.Composers.SelectMany(c => c.CompositionCatalogs), null, "Prefix"));
            }

            if (CompositionCatalogPrefixListBox.Items.Count > 0)
            {
                CompositionCatalogPrefixListBox.SelectedIndex = 0;
            }
        }

        public void RefreshCompositionSection()
        {
            if (_selectedComposition == null)
            {
                DisableCompositionSection();
                ClearCompositionSection();

                _selectedMovement = null;

                RefreshMovementSection();

                return;
            }

            LoadCompositionSection();
            EnableCompositionSection();
        }

        #endregion Composition Section Methods

        #region Movement Section Methods

        public void ClearMovementSection()
        {
            MovementNumberBox.Text = null;
            MovementNameTextBox.Text = null;
            RecordingListBox.ItemsSource = null;
        }

        public void DisableMovementSection()
        {
            MovementNumberBox.IsEnabled = false;
            MovementNameTextBox.IsEnabled = false;
            RecordingListBox.IsEnabled = false;
        }

        public void EnableMovementSection()
        {
            MovementNumberBox.IsEnabled = true;
            MovementNameTextBox.IsEnabled = true;
            RecordingListBox.IsEnabled = true;
        }

        public void LoadMovementSection()
        {
            MovementNumberBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_selectedMovement, "Number"));
            MovementNameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_selectedMovement, "Name"));
            RecordingListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_selectedMovement.Recordings, null, "ID"));
        }

        public void RefreshMovementSection()
        {
            if (_selectedMovement == null)
            {
                DisableMovementSection();
                ClearMovementSection();

                _selectedRecording = null;

                RefreshRecordingSection();

                return;
            }

            LoadMovementSection();
            EnableMovementSection();
        }

        #endregion Movement Section Methods

        #region Recording Section Methods

        public void ClearRecordingSection()
        {
            RecordingPerformerListBox.ItemsSource = null;
            RecordingAlbumAutoCompleteBox.Text = null;
            RecordingTrackNumberBox.Text = null;
            RecordingDatesTextBox.Text = null;
            RecordingLocationListBox.ItemsSource = null;
        }

        public void DisableRecordingSection()
        {
            RecordingPerformerListBox.IsEnabled = false;
            RecordingAlbumAutoCompleteBox.IsEnabled = false;
            RecordingTrackNumberBox.IsEnabled = false;
            RecordingDatesTextBox.IsEnabled = false;
            RecordingLocationListBox.IsEnabled = false;
        }

        public void EnableRecordingSection()
        {
            RecordingPerformerListBox.IsEnabled = true;
            RecordingAlbumAutoCompleteBox.IsEnabled = true;
            RecordingTrackNumberBox.IsEnabled = true;
            RecordingDatesTextBox.IsEnabled = true;
            RecordingLocationListBox.IsEnabled = true;
        }

        public void LoadRecordingSection()
        {
            RecordingPathTextBox.Text = LibraryDB.DataProvider.Get(_selectedRecording.ID).FirstOrDefault();
            RecordingPerformerListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_dataProvider.Performers.Local, null, "Name"));
            RecordingAlbumAutoCompleteBox.Text = _selectedRecording.Album?.Name;
            RecordingTrackNumberBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_selectedRecording, "TrackNumber"));
            RecordingDatesTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_selectedRecording, "Dates"));
            RecordingLocationListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_selectedRecording.Locations, null, "Name"));

            ListUtility.AddMany(RecordingPerformerListBox.SelectedItems, _selectedRecording.Performers);
        }

        public void RefreshRecordingSection()
        {
            if (_selectedRecording == null)
            {
                DisableRecordingSection();
                ClearRecordingSection();

                return;
            }

            LoadRecordingSection();
            EnableRecordingSection();
        }

        #endregion Recording Section Methods

        #region Composer Section Events

        private void ComposerBirthLocationAutoCompleteBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var composer = _selectedComposers[0];

            var birthLocationQuery = _dataProvider.Locations.Local.FirstOrDefault(l => l.Name == ComposerBirthLocationAutoCompleteBox.Text);

            if (birthLocationQuery == null)                                                                                                                                                                                // New location does not exist in database.
            {
                if (composer.BirthLocation != null && composer.BirthLocation.BirthLocationComposers.Count + composer.BirthLocation.DeathLocationComposers.Count + composer.BirthLocation.Recordings.Count == 1)            // Delete old location if only reference is gone.
                {
                    _dataProvider.Locations.Remove(composer.BirthLocation);
                }

                if (string.IsNullOrEmpty(ComposerBirthLocationAutoCompleteBox.Text))
                {
                    composer.BirthLocation = null;
                }
                else
                {
                    var location = new Location();
                    location.ID = _dataProvider.Locations.Local.Count + 1;
                    location.Name = ComposerBirthLocationAutoCompleteBox.Text;
                    _dataProvider.Locations.Add(location);
                    composer.BirthLocation = location;
                }
            }
            else                                                                                                                                                                                                           // New location does exist.
            {
                if (composer.BirthLocation != null && birthLocationQuery.Name != composer.BirthLocation.Name && composer.BirthLocation.BirthLocationComposers.Count + composer.BirthLocation.DeathLocationComposers.Count + composer.BirthLocation.Recordings.Count == 1) // Delete old location if only reference is gone.
                {
                    _dataProvider.Locations.Remove(composer.BirthLocation);
                }

                composer.BirthLocation = birthLocationQuery;
            }
        }

        private void ComposerDeathLocationAutoCompleteBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var composer = _selectedComposers[0];

            var deathLocationQuery = _dataProvider.Locations.Local.FirstOrDefault(l => l.Name == ComposerDeathLocationAutoCompleteBox.Text);

            if (deathLocationQuery == null)                                                                                                                                                                                // New location does not exist in database.
            {
                if (composer.DeathLocation != null && composer.DeathLocation.BirthLocationComposers.Count + composer.DeathLocation.DeathLocationComposers.Count + composer.DeathLocation.Recordings.Count == 1)            // Delete old location if only reference is gone.
                {
                    _dataProvider.Locations.Remove(composer.DeathLocation);
                }

                if (string.IsNullOrEmpty(ComposerDeathLocationAutoCompleteBox.Text))
                {
                    composer.DeathLocation = null;
                }
                else
                {
                    var location = new Location();
                    location.ID = _dataProvider.Locations.Local.Count + 1;
                    location.Name = ComposerDeathLocationAutoCompleteBox.Text;
                    _dataProvider.Locations.Add(location);
                    composer.DeathLocation = location;
                }
            }
            else                                                                                                                                                                                                           // New location does exist.
            {
                if (composer.DeathLocation != null && deathLocationQuery.Name != composer.DeathLocation.Name && composer.DeathLocation.BirthLocationComposers.Count + composer.DeathLocation.DeathLocationComposers.Count + composer.DeathLocation.Recordings.Count == 1) // Delete old location if only reference is gone.
                {
                    _dataProvider.Locations.Remove(composer.DeathLocation);
                }

                composer.DeathLocation = deathLocationQuery;
            }
        }

        private void ComposerDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ComposerListBox.IsEnabled)
            {
                _dataProvider.Composers.Local.Remove((Composer)ComposerListBox.SelectedItem);
            }
        }

        private void ComposerEraListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComposerEraListBox.IsEnabled)
            {
                var composer = (Composer)ComposerListBox.SelectedItem;

                foreach (Era addedItem in e.AddedItems)
                {
                    if (!composer.Eras.Contains(addedItem))
                    {
                        composer.Eras.Add(addedItem);
                    }
                }

                foreach (Era removedItem in e.RemovedItems)
                {
                    if (composer.Eras.Contains(removedItem))
                    {
                        composer.Eras.Remove(removedItem);
                    }
                }
            }
        }

        private void ComposerImageDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ComposerImageListBox.IsEnabled)
            {
                _dataProvider.ComposerImages.Remove((ComposerImage)ComposerImageListBox.SelectedItem);
            }
        }

        private void ComposerImageListBox_Drop(object sender, DragEventArgs e)
        {
            if (ComposerImageListBox.IsEnabled)
            {
                var composerImage = ComposerImageProvider.GetComposerImage(_selectedComposers[0], (string)e.Data.GetData(DataFormats.UnicodeText), _dataProvider);

                if (composerImage != null)
                {
                    _selectedComposers[0].ComposerImages.Add(composerImage);

                    ComposerImageListBox.SelectedIndex = _selectedComposers[0].ComposerImages.Count - 1;
                }
            }
        }

        private void ComposerInfluenceDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ComposerInfluenceListBox.IsEnabled)
            {
                ((Composer)ComposerListBox.SelectedItem).Influences.Remove((Composer)ComposerInfluenceListBox.SelectedItem);
            }
        }

        private void ComposerInfluenceListBox_Drop(object sender, DragEventArgs e)
        {
            if (ComposerInfluenceListBox.IsEnabled && _selectedComposers.Count == 1)
            {
                _selectedComposers[0].Influences.Add((Composer)e.Data.GetData(typeof(Composer)));

                ComposerInfluenceListBox.ItemsSource = new List<Composer>(_selectedComposers[0].Influences);
            }
        }

        private void ComposerLinkDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ComposerLinkListBox.IsEnabled)
            {
                _dataProvider.ComposerLinks.Remove((ComposerLink)ComposerLinkListBox.SelectedItem);
            }
        }

        private void ComposerLinkListBox_Drop(object sender, DragEventArgs e)
        {
            if (ComposerLinkListBox.IsEnabled)
            {
                var url = (string)e.Data.GetData(DataFormats.UnicodeText);

                try
                {
                    if (!FileUtility.WebsiteExists(url))
                    {
                        return;
                    }

                    var composerLink = new ComposerLink();
                    composerLink.ID = (short)(_dataProvider.ComposerLinks.Count() + _dataProvider.ComposerLinks.Local.Count + 1);
                    composerLink.Composer = _selectedComposers[0];
                    composerLink.URL = url;

                    _selectedComposers[0].ComposerLinks.Add(composerLink);

                    ComposerLinkListBox.SelectedItem = composerLink;
                }
                catch
                {
                }
            }
        }

        private void ComposerListBox_Drop(object sender, DragEventArgs e)
        {
            if (ComposerListBox.IsEnabled)
            {
                var composerName = (string)e.Data.GetData(DataFormats.UnicodeText);

                if (composerName != null)
                {
                    var composer = new Composer();
                    composer.ID = (short)(_dataProvider.Composers.Local.Count + 1);
                    composer.Name = composerName;

                    _dataProvider.Composers.Add(composer);

                    _selectedComposers = new ObservableCollection<Composer> { composer };

                    RefreshComposerSection();

                    ComposerListBox.SelectedItem = composer;
                }
            }
        }

        private void ComposerListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ComposerListBox.IsEnabled)
            {
                var frameworkElement = e.OriginalSource as FrameworkElement;

                if (frameworkElement == null || frameworkElement.TemplatedParent == null || frameworkElement.TemplatedParent.GetType() != typeof(Thumb))
                {
                    e.Handled = true;
                }
            }
        }

        private void ComposerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ComposerListBox.IsEnabled)
            {
                return;
            }

            _selectedComposers = new ObservableCollection<Composer>(ComposerListBox.SelectedItems.Cast<Composer>());

            RefreshComposerSection();
        }

        private void ComposerListBoxItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (ComposerListBox.IsEnabled)
            {
                var textBlock = sender as TextBlock;

                if (textBlock != null && e.LeftButton == MouseButtonState.Pressed)
                {
                    DragDrop.DoDragDrop(textBlock, new DataObject(typeof(Composer), textBlock.DataContext), DragDropEffects.Link);

                    e.Handled = true;
                }
            }
        }

        private void ComposerListBoxItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ComposerListBox.IsEnabled)
            {
                var clickedElement = e.OriginalSource as FrameworkElement;

                if (clickedElement != null && !(clickedElement is Thumb))
                {
                    ComposerListBox.SelectedItem = clickedElement.DataContext;
                }
            }
        }

        private void ComposerNationalityListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComposerNationalityListBox.IsEnabled)
            {
                var composer = (Composer)ComposerListBox.SelectedItem;

                foreach (Nationality nationality in e.AddedItems)
                {
                    composer.Nationalities.Add(nationality);
                }

                foreach (Nationality nationality in e.RemovedItems)
                {
                    composer.Nationalities.Remove(nationality);
                }
            }
        }

        #endregion Composer Section Events

        #region Composition Collection Section Events

        private void CompositionCollectionCatalogNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!CompositionCollectionCatalogNumberTextBox.IsEnabled)
            {
                return;
            }

            var selectedCatalogs = _selectedComposers
                .SelectMany(c => c.CompositionCatalogs)
                .Where(cc => cc == CompositionCollectionCatalogPrefixListBox.SelectedItem);

            var selectedCatalogNumbers = selectedCatalogs
                .SelectMany(cc => cc.CatalogNumbers)
                .Where(cn => cn.CompositionCollection == _selectedCompositionCollection);

            if (selectedCatalogNumbers.Count() == 0)
            {
                var catalogNumbers = new List<CatalogNumber>();

                foreach (var catalog in selectedCatalogs)
                {
                    var catalogNumber = new CatalogNumber();
                    catalogNumber.ID = (short)(_dataProvider.CatalogNumbers.Count() + _dataProvider.CatalogNumbers.Local.Count + 1);
                    catalogNumber.CompositionCatalog = catalog;
                    catalogNumber.CompositionCollection = _selectedCompositionCollection;

                    _selectedCompositionCollection.CatalogNumbers.Add(catalogNumber);
                    catalog.CatalogNumbers.Add(catalogNumber);
                    catalogNumbers.Add(catalogNumber);
                }

                selectedCatalogNumbers = catalogNumbers;
            }

            foreach (var catalogNumber in selectedCatalogNumbers)
            {
                catalogNumber.Number = CompositionCollectionCatalogNumberTextBox.Text;
            }
        }

        private void CompositionCollectionCatalogPrefixDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!CompositionCollectionCatalogPrefixListBox.IsEnabled)
            {
                return;
            }

            if (!(e.OriginalSource is ListBoxItem))
            {
                return;
            }

            foreach (var composer in _selectedComposers)
            {
                var selectedCatalog = composer.CompositionCatalogs.FirstOrDefault(cc => cc.Prefix == (string)CompositionCollectionCatalogPrefixListBox.SelectedItem);

                if (selectedCatalog != null)
                {
                    composer.CompositionCatalogs.Remove(selectedCatalog);
                }
            }
        }

        private void CompositionCollectionCatalogPrefixListBox_Drop(object sender, DragEventArgs e)
        {
            if (CompositionCollectionCatalogPrefixListBox.IsEnabled)
            {
                var droppedString = (string)e.Data.GetData(DataFormats.UnicodeText);

                if (droppedString == null)
                {
                    return;
                }

                var newCompositionCatalogs = new CompositionCatalog[_selectedComposers.Count];

                for (int i = 0; i < _selectedComposers.Count; i++)
                {
                    var newCompositionCatalog = new CompositionCatalog();
                    newCompositionCatalog.ID = (short)(_dataProvider.CompositionCatalogs.Count() + _dataProvider.CompositionCatalogs.Local.Count + 1);
                    newCompositionCatalog.Prefix = droppedString;
                    newCompositionCatalog.Composer = _selectedComposers[i];

                    _selectedComposers[i].CompositionCatalogs.Add(newCompositionCatalog);
                    newCompositionCatalogs[i] = newCompositionCatalog;
                }

                var availableCompositionCatalogs = _selectedComposers
                    .SelectMany(c => c.CompositionCatalogs)
                    .Select(cc => cc.Prefix)
                    .Distinct();

                CompositionCollectionCatalogPrefixListBox.ItemsSource = availableCompositionCatalogs.ToList();
                CompositionCollectionCatalogPrefixListBox.SelectedItem = droppedString;

                CompositionCollectionCatalogNumberTextBox.Text = null;
            }
        }

        private void CompositionCollectionCatalogPrefixListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
            {
                return;
            }

            CompositionCollectionCatalogNumberTextBox.Text = _selectedCompositionCollection.CatalogNumbers.FirstOrDefault(cn => cn.CompositionCatalog == CompositionCollectionCatalogPrefixListBox.SelectedItem)?.Number;
        }

        private void CompositionCollectionDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void CompositionCollectionListBox_Drop(object sender, DragEventArgs e)
        {
            if (CompositionCollectionListBox.IsEnabled)
            {
                var droppedString = (string)e.Data.GetData(DataFormats.UnicodeText);

                if (droppedString == null)
                {
                    return;
                }

                _selectedCompositionCollection = new CompositionCollection();
                _selectedCompositionCollection.ID = (short)(_dataProvider.CompositionCollections.Count() + _dataProvider.CompositionCollections.Local.Count + 1);
                _selectedCompositionCollection.Name = droppedString;
                _selectedCompositionCollection.Composers = _selectedComposers;

                foreach (var composer in _selectedComposers)
                {
                    composer.CompositionCollections.Add(_selectedCompositionCollection);
                }

                CompositionCollectionListBox.ItemsSource = new List<CompositionCollection>(_selectedComposers.Common(c => c.CompositionCollections));
                CompositionCollectionListBox.SelectedItem = _selectedCompositionCollection;
            }
        }

        private void CompositionCollectionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CompositionCollectionListBox.IsEnabled)
            {
                _selectedCompositionCollection = (CompositionCollection)CompositionCollectionListBox.SelectedItem;

                RefreshCompositionCollectionSection();
            }
        }

        #endregion Composition Collection Section Events

        #region Composition Section Events

        private void CompositionCatalogNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!CompositionCatalogNumberTextBox.IsEnabled)
            {
                return;
            }

            var selectedCatalogs = _selectedComposers
                .SelectMany(c => c.CompositionCatalogs)
                .Where(cc => cc == CompositionCatalogPrefixListBox.SelectedItem);

            var selectedCatalogNumbers = selectedCatalogs
                .SelectMany(cc => cc.CatalogNumbers)
                .Where(cn => cn.Composition == _selectedComposition);

            if (selectedCatalogNumbers.Count() == 0)
            {
                var catalogNumbers = new List<CatalogNumber>();

                foreach (var catalog in selectedCatalogs)
                {
                    var catalogNumber = new CatalogNumber();
                    catalogNumber.ID = (short)(_dataProvider.CatalogNumbers.Count() + _dataProvider.CatalogNumbers.Local.Count + 1);
                    catalogNumber.CompositionCatalog = catalog;
                    catalogNumber.Composition = _selectedComposition;

                    _selectedComposition.CatalogNumbers.Add(catalogNumber);
                    catalog.CatalogNumbers.Add(catalogNumber);
                    catalogNumbers.Add(catalogNumber);
                }

                selectedCatalogNumbers = catalogNumbers;
            }

            foreach (var catalogNumber in selectedCatalogNumbers)
            {
                catalogNumber.Number = CompositionCatalogNumberTextBox.Text;
            }
        }

        private void CompositionCatalogPrefixDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!CompositionCatalogPrefixListBox.IsEnabled)
            {
                return;
            }

            if (!(e.OriginalSource is ListBoxItem))
            {
                return;
            }

            foreach (var composer in _selectedComposers)
            {
                var selectedCatalog = composer.CompositionCatalogs.FirstOrDefault(cc => cc.Prefix == (string)CompositionCatalogPrefixListBox.SelectedItem);

                if (selectedCatalog != null)
                {
                    composer.CompositionCatalogs.Remove(selectedCatalog);
                }
            }
        }

        private void CompositionCatalogPrefixListBox_Drop(object sender, DragEventArgs e)
        {
            if (!CompositionCatalogPrefixListBox.IsEnabled)
            {
                return;
            }

            var droppedString = (string)e.Data.GetData(DataFormats.UnicodeText);

            if (droppedString == null)
            {
                return;
            }

            var newCompositionCatalogs = new CompositionCatalog[_selectedComposers.Count];

            for (int i = 0; i < _selectedComposers.Count; i++)
            {
                var newCompositionCatalog = new CompositionCatalog();
                newCompositionCatalog.ID = (short)(_dataProvider.CompositionCatalogs.Count() + _dataProvider.CompositionCatalogs.Local.Count + 1);
                newCompositionCatalog.Prefix = droppedString;
                newCompositionCatalog.Composer = _selectedComposers[i];

                _dataProvider.CompositionCatalogs.Add(newCompositionCatalog);
                _selectedComposers[i].CompositionCatalogs.Add(newCompositionCatalog);
                newCompositionCatalogs[i] = newCompositionCatalog;
            }

            var availableCompositionCatalogs = _selectedComposers
                .SelectMany(c => c.CompositionCatalogs)
                .Select(cc => cc.Prefix)
                .Distinct();

            CompositionCatalogPrefixListBox.ItemsSource = availableCompositionCatalogs.ToList();
            CompositionCatalogPrefixListBox.SelectedItem = droppedString;

            CompositionCatalogNumberTextBox.Text = null;
        }

        private void CompositionCatalogPrefixListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
            {
                return;
            }

            CompositionCatalogNumberTextBox.Text = _selectedComposition.CatalogNumbers.FirstOrDefault(cn => cn.CompositionCatalog == CompositionCatalogPrefixListBox.SelectedItem)?.Number;
        }

        private void CompositionDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (CompositionListBox.IsEnabled)
            {
                _dataProvider.Compositions.Remove(_selectedComposition);
            }
        }

        private void CompositionListBox_Drop(object sender, DragEventArgs e)
        {
            if (!CompositionListBox.IsEnabled)
            {
                return;
            }

            var compositionName = (string)e.Data.GetData(DataFormats.UnicodeText);

            if (compositionName == null)
            {
                return;
            }

            if (_selectedCompositionCollection != null)
            {
                _selectedComposition = _selectedCompositionCollection.Compositions.FirstOrDefault(c => c.Name == compositionName);

                if (_selectedComposition == null)
                {
                    _selectedComposition = new Composition();
                    _selectedComposition.ID = _dataProvider.Compositions.Count() + _dataProvider.Compositions.Local.Count + 1;
                    _selectedComposition.Name = compositionName;
                    _dataProvider.Compositions.Add(_selectedComposition);
                }

                _selectedComposition.CompositionCollection = _selectedCompositionCollection;
                _selectedCompositionCollection.Compositions.Add(_selectedComposition);

                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_selectedCompositionCollection.Compositions, null, "Name"));
            }
            else
            {
                _selectedComposition = _selectedComposers.Common(c => c.Compositions).FirstOrDefault(c => c.Name == compositionName);

                if (_selectedComposition == null)
                {
                    _selectedComposition = new Composition();
                    _selectedComposition.ID = _dataProvider.Compositions.Local.Count + 1;
                    _selectedComposition.Name = compositionName;
                    _dataProvider.Compositions.Add(_selectedComposition);
                }

                foreach (var composer in _selectedComposers)
                {
                    composer.Compositions.Add(_selectedComposition);
                }

                _selectedComposition.Composers = _selectedComposers;

                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_selectedComposers.Common(c => c.Compositions), null, "Name"));
            }

            CompositionListBox.SelectedItem = _selectedComposition;
        }

        private void CompositionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!CompositionListBox.IsEnabled)
            {
                return;
            }

            _selectedComposition = (Composition)CompositionListBox.SelectedItem;

            RefreshCompositionSection();
        }

        #endregion Composition Section Events

        #region Movement Section Events

        private void MovementListBox_Drop(object sender, DragEventArgs e)
        {
            if (MovementListBox.IsEnabled)
            {
                var movementName = (string)e.Data.GetData(DataFormats.UnicodeText);

                if (movementName != null)
                {
                    _selectedMovement = new Movement();
                    _selectedMovement.ID = _dataProvider.Movements.Count() + _dataProvider.Movements.Local.Count + 1;
                    _selectedMovement.Name = movementName;
                    _selectedMovement.Composition = _selectedComposition;

                    _selectedComposition.Movements.Add(_selectedMovement);

                    MovementListBox.ItemsSource = new List<Movement>(_selectedComposition.Movements);
                    MovementListBox.SelectedItem = _selectedMovement;
                }
            }
        }

        private void MovementListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MovementListBox.IsEnabled)
            {
                _selectedMovement = (Movement)MovementListBox.SelectedItem;

                RefreshMovementSection();
            }
        }

        private void MovementDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MovementListBox.IsEnabled)
            {
                _dataProvider.Movements.Remove((Movement)MovementListBox.SelectedItem);
            }
        }

        #endregion Movement Section Events

        #region Recording Section Events

        private void RecordingAlbumAutoCompleteBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var recordingAlbum = _dataProvider.Albums.Local.FirstOrDefault(l => l.Name == RecordingAlbumAutoCompleteBox.Text);

            if (recordingAlbum == null)
            {
                if (_selectedRecording.Album != null && _selectedRecording.Album.Recordings.Count == 1)
                {
                    _dataProvider.Albums.Remove(_selectedRecording.Album);
                }

                if (string.IsNullOrEmpty(RecordingAlbumAutoCompleteBox.Text))
                {
                    _selectedRecording.Album = null;
                }
                else
                {
                    var album = new Album();
                    album.ID = (short)(_dataProvider.Albums.Count() + _dataProvider.Albums.Local.Count + 1);
                    album.Name = RecordingAlbumAutoCompleteBox.Text;
                    album.Recordings.Add(_selectedRecording);

                    _selectedRecording.Album = album;
                }
            }
            else
            {
                if (_selectedRecording.Album != null && recordingAlbum.Name != _selectedRecording.Album.Name && _selectedRecording.Album.Recordings.Count == 1)
                {
                    _dataProvider.Albums.Remove(_selectedRecording.Album);
                }

                _selectedRecording.Album = recordingAlbum;
            }
        }

        private void RecordingListBox_Drop(object sender, DragEventArgs e)
        {
            if (!RecordingListBox.IsEnabled)
            {
                return;
            }

            var recordingPath = (string)e.Data.GetData(DataFormats.UnicodeText);

            if (recordingPath == null)
            {
                return;
            }

            var recordingId = _dataProvider.Recordings.Count() + _dataProvider.Recordings.Local.Count + 1;

            var importPathBuilder = new StringBuilder(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic))
                .Append("\\")
                .Append(_selectedComposers.Count > 1 ? "Various" : _selectedComposers[0].Name)
                .Append("\\");

            if (_selectedCompositionCollection != null)
            {
                importPathBuilder.Append(_selectedCompositionCollection.Name).Append("\\");
            }

            if (_selectedComposition != null)
            {
                importPathBuilder.Append(_selectedComposition.Name).Append("\\");
            }

            if (_selectedMovement != null)
            {
                importPathBuilder.Append($"{_selectedMovement.Number}. {_selectedMovement.Name}").Append("\\");
            }

            importPathBuilder.Append(recordingId).Append(Path.GetExtension(recordingPath));

            var importPath = importPathBuilder.ToString();

            Directory.CreateDirectory(Path.GetDirectoryName(importPath));

            if (!File.Exists(importPath))
            {
                File.Copy(recordingPath, importPath);
            }

            LibraryDB.DataProvider.Add(recordingId, importPath);

            _selectedRecording = new Recording();
            _selectedRecording.ID = recordingId;

            if (_selectedMovement != null)
            {
                _selectedRecording.Movement = _selectedMovement;
                _selectedMovement.Recordings.Add(_selectedRecording);

                RecordingListBox.ItemsSource = new List<Recording>(_selectedMovement.Recordings);
            }
            else if (_selectedComposition != null)
            {
                _selectedRecording.Composition = _selectedComposition;
                _selectedComposition.Recordings.Add(_selectedRecording);

                RecordingListBox.ItemsSource = new List<Recording>(_selectedComposition.Recordings);
            }
            else if (_selectedCompositionCollection != null)
            {
                _selectedRecording.CompositionCollection = _selectedCompositionCollection;
                _selectedCompositionCollection.Recordings.Add(_selectedRecording);

                RecordingListBox.ItemsSource = new List<Recording>(_selectedCompositionCollection.Recordings);
            }

            RecordingListBox.SelectedItem = _selectedRecording;
        }

        private void RecordingListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecordingListBox.IsEnabled)
            {
                _selectedRecording = (Recording)RecordingListBox.SelectedItem;

                RefreshRecordingSection();
            }
        }

        private void RecordingDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (RecordingListBox.IsEnabled)
            {
                _dataProvider.Recordings.Local.Remove((Recording)RecordingListBox.SelectedItem);
            }
        }

        private void RecordingPerformerDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (RecordingPerformerListBox.IsEnabled)
            {
                var performer = (Performer)RecordingPerformerListBox.SelectedItem;

                performer.Recordings.Remove(_selectedRecording);

                if (performer.Recordings.Count == 0)
                {
                    _dataProvider.Performers.Remove(performer);
                }
            }
        }

        private void RecordingPerformerListBox_Drop(object sender, DragEventArgs e)
        {
            if (!RecordingPerformerListBox.IsEnabled)
            {
                return;
            }

            var performerName = (string)e.Data.GetData(DataFormats.UnicodeText);

            if (performerName == null)
            {
                return;
            }

            var performer = _dataProvider.Performers.FirstOrDefault(p => p.Name == performerName);

            if (performer == null)
            {
                performer = new Performer();
                performer.ID = _dataProvider.Performers.Count() + _dataProvider.Performers.Local.Count + 1;
                performer.Name = performerName;
                _dataProvider.Performers.Local.Add(performer);
            }

            performer.Recordings.Add(_selectedRecording);
            _selectedRecording.Performers.Add(performer);

            RecordingPerformerListBox.SelectedItem = performer;
        }

        private void RecordingLocationListBox_Drop(object sender, DragEventArgs e)
        {
            if (!RecordingLocationListBox.IsEnabled)
            {
                return;
            }

            var locationName = (string)e.Data.GetData(DataFormats.UnicodeText);

            if (locationName == null)
            {
                return;
            }

            var location = _dataProvider.Locations.FirstOrDefault(l => l.Name == locationName);

            if (location == null)
            {
                location = new Location();
                location.ID = _dataProvider.Compositions.Local.Count + 1;
                location.Name = locationName;
                _dataProvider.Locations.Local.Add(location);
            }

            location.Recordings.Add(_selectedRecording);
            _selectedRecording.Locations.Add(location);
        }

        private void RecordingLocationDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (RecordingLocationListBox.IsEnabled)
            {
                var location = (Location)RecordingLocationListBox.SelectedItem;

                location.Recordings.Remove(_selectedRecording);

                if (location.Recordings.Count == 0 && location.BirthLocationComposers.Count == 0 && location.DeathLocationComposers.Count == 0)
                {
                    _dataProvider.Locations.Local.Remove(location);
                }
            }
        }

        #endregion Recording Section Events

        #region Status and Button Section Events

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _dataProvider.Dispose();

            NavigationService.GoBack();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            using (_dataProvider)
            {
                try
                {
                    _dataProvider.SaveChanges();
                }
                catch (OptimisticConcurrencyException ex)
                {
                    Logger.Log(ex.ToString(), "MusicTimeline.log");
                }
            }

            NavigationService.Navigate(new Uri(@"pack://application:,,,/Views/TimelinePage.xaml"));
        }

        #endregion Status and Button Section Events

        private Recording GetRecordingFromFilePath(string selectedFilePath)
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
                return _dataProvider.Recordings.Local.First(r => r.ID == recordingID);
            }

            return null;
        }
    }
}