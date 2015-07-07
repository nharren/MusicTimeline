using NathanHarrenstein.MusicDb;
using NathanHarrenstein.MusicTimeline.Converters;
using NathanHarrenstein.MusicTimeline.Extensions;
using NathanHarrenstein.MusicTimeline.Initializers;
using NathanHarrenstein.MusicTimeline.Models;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class InputPage : Page
    {
        private ObservableCollection<Composer> _currentComposers;
        private Composition _currentComposition;
        private CompositionCollection _currentCompositionCollection;
        private Movement _currentMovement;
        private Recording _currentRecording;

        public InputPage()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                InputPageInitializer.Initialize(this);
            }
        }

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
            if (_currentComposers.Count == 1)
            {
                ComposerNationalityListBox.SelectedItems.Clear();
                ComposerEraListBox.SelectedItems.Clear();

                var composer = _currentComposers.First();

                ComposerNameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(composer, "Name"));
                ComposerDatesTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(composer, "Dates"));
                ComposerBirthLocationAutoCompleteBox.Text = composer.BirthLocation?.Name;
                ComposerDeathLocationAutoCompleteBox.Text = composer.DeathLocation?.Name;
                ComposerInfluenceListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(composer.Influences, null, "Name"));
                ComposerImageListBox.ItemsSource = new List<BitmapImage>(composer.ComposerImages.Select(ci => ComposerImageUtility.ToBitmapImage(ci)));
                ComposerLinkListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(composer, "ComposerLinks", new ComposerLinkConverter()));
                ComposerBiographyTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(composer, "Biography"));
                CompositionCollectionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(composer.CompositionCollections, null, "Name"));
                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(composer.Compositions, null, "Name"));

                ListUtility.AddMany(ComposerNationalityListBox.SelectedItems, composer.Nationalities);
                ListUtility.AddMany(ComposerEraListBox.SelectedItems, composer.Eras);

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
                CompositionCollectionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_currentComposers.Common(c => c.CompositionCollections), null, "Name"));
                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_currentComposers.Common(c => c.Compositions), null, "Name"));
            }
        }

        public void RefreshComposerSection()
        {
            if (_currentComposers.Count == 0)
            {
                DisableComposerSection();
                ClearComposerSection();

                _currentCompositionCollection = null;

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

            CompositionListBox.ItemsSource = new List<Composition>(_currentComposers.SelectMany(c => c.Compositions));
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
            CompositionCollectionNameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_currentCompositionCollection, "Name"));
            CompositionCollectionCatalogPrefixListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_currentCompositionCollection.Composers.SelectMany(c => c.CompositionCatalogs), null, "Prefix"));
            CompositionCollectionCatalogNumberTextBox.Text = _currentCompositionCollection.CatalogNumbers.FirstOrDefault(cn => cn.CompositionCatalog.Prefix == (string)CompositionCollectionCatalogPrefixListBox.SelectedItem)?.Number;
            CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_currentCompositionCollection.Compositions, null, "Name"));

            if (CompositionCollectionCatalogPrefixListBox.Items.Count > 0)
            {
                CompositionCollectionCatalogPrefixListBox.SelectedIndex = 0;
            }
        }

        public void RefreshCompositionCollectionSection()
        {
            if (_currentCompositionCollection == null)
            {
                DisableCompositionCollectionSection();
                ClearCompositionCollectionSection();

                _currentComposition = null;

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
            CompositionNameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_currentComposition, "Name"));
            CompositionNicknameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_currentComposition, "Nickname"));
            CompositionDatesTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_currentComposition, "Dates"));
            CompositionCatalogNumberTextBox.Text = _currentComposition.CatalogNumbers.FirstOrDefault(cn => cn.CompositionCatalog.Prefix == (string)CompositionCatalogPrefixListBox.SelectedItem)?.Number;
            MovementListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_currentComposition.Movements, null, "Name"));

            if (_currentCompositionCollection != null)
            {
                CompositionCatalogPrefixListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_currentCompositionCollection.Composers.SelectMany(c => c.CompositionCatalogs), null, "Prefix"));
            }
            else
            {
                CompositionCatalogPrefixListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_currentComposition.Composers.SelectMany(c => c.CompositionCatalogs), null, "Prefix"));
            }

            if (CompositionCatalogPrefixListBox.Items.Count > 0)
            {
                CompositionCatalogPrefixListBox.SelectedIndex = 0;
            }
        }

        public void RefreshCompositionSection()
        {
            if (_currentComposition == null)
            {
                DisableCompositionSection();
                ClearCompositionSection();

                _currentMovement = null;

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
            MovementNumberBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_currentMovement, "Number"));
            MovementNameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_currentMovement, "Name"));
            RecordingListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_currentMovement.Recordings, null, "ID"));
        }

        public void RefreshMovementSection()
        {
            if (_currentMovement == null)
            {
                DisableMovementSection();
                ClearMovementSection();

                _currentRecording = null;

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
            RecordingPathTextBox.Text = LibraryDb.DataProvider.Get(_currentRecording.ID).First();
            RecordingPerformerListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(App.DataProvider.Performers.Local, null, "Name"));
            RecordingAlbumAutoCompleteBox.Text = _currentRecording.Album?.Name;
            RecordingTrackNumberBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_currentRecording, "TrackNumber"));
            RecordingDatesTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_currentRecording, "Dates"));
            RecordingLocationListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_currentRecording.Locations, null, "Name"));

            ListUtility.AddMany(RecordingPerformerListBox.SelectedItems, _currentRecording.Performers);
        }

        public void RefreshRecordingSection()
        {
            if (_currentRecording == null)
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

        private void ComposerBirthLocationAutoCompleteBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var composer = _currentComposers[0];

            var birthLocationQuery = App.DataProvider.Locations.Local.FirstOrDefault(l => l.Name == ComposerBirthLocationAutoCompleteBox.Text);

            if (birthLocationQuery == null)                                                                                                                                                                                // New location does not exist in database.
            {
                if (composer.BirthLocation != null && composer.BirthLocation.BirthLocationComposers.Count + composer.BirthLocation.DeathLocationComposers.Count + composer.BirthLocation.Recordings.Count == 1)            // Delete old location if only reference is gone.
                {
                    App.DataProvider.Locations.Remove(composer.BirthLocation);
                }

                if (string.IsNullOrEmpty(ComposerBirthLocationAutoCompleteBox.Text))
                {
                    composer.BirthLocation = null;
                }
                else
                {
                    var location = new Location();
                    location.Name = ComposerBirthLocationAutoCompleteBox.Text;
                    App.DataProvider.Locations.Add(location);
                    composer.BirthLocation = location;
                }
            }
            else                                                                                                                                                                                                           // New location does exist.
            {
                if (composer.BirthLocation != null && birthLocationQuery.Name != composer.BirthLocation.Name && composer.BirthLocation.BirthLocationComposers.Count + composer.BirthLocation.DeathLocationComposers.Count + composer.BirthLocation.Recordings.Count == 1) // Delete old location if only reference is gone.
                {
                    App.DataProvider.Locations.Remove(composer.BirthLocation);
                }

                composer.BirthLocation = birthLocationQuery;
            }
        }

        private void ComposerDeathLocationAutoCompleteBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var composer = _currentComposers[0];

            var deathLocationQuery = App.DataProvider.Locations.Local.FirstOrDefault(l => l.Name == ComposerDeathLocationAutoCompleteBox.Text);

            if (deathLocationQuery == null)                                                                                                                                                                                // New location does not exist in database.
            {
                if (composer.DeathLocation != null && composer.DeathLocation.BirthLocationComposers.Count + composer.DeathLocation.DeathLocationComposers.Count + composer.DeathLocation.Recordings.Count == 1)            // Delete old location if only reference is gone.
                {
                    App.DataProvider.Locations.Remove(composer.DeathLocation);
                }

                if (string.IsNullOrEmpty(ComposerDeathLocationAutoCompleteBox.Text))
                {
                    composer.DeathLocation = null;
                }
                else
                {
                    composer.DeathLocation = new Location { Name = ComposerDeathLocationAutoCompleteBox.Text };
                }
            }
            else                                                                                                                                                                                                           // New location does exist.
            {
                if (composer.DeathLocation != null && deathLocationQuery.Name != composer.DeathLocation.Name && composer.DeathLocation.BirthLocationComposers.Count + composer.DeathLocation.DeathLocationComposers.Count + composer.DeathLocation.Recordings.Count == 1) // Delete old location if only reference is gone.
                {
                    App.DataProvider.Locations.Remove(composer.DeathLocation);
                }

                composer.DeathLocation = deathLocationQuery;
            }
        }

        private void ComposerDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ComposerListBox.IsEnabled)
            {
                App.DataProvider.Composers.Local.Remove((Composer)ComposerListBox.SelectedItem);
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
                ((Composer)ComposerListBox.SelectedItem).ComposerImages.Remove((ComposerImage)ComposerImageListBox.SelectedItem);
            }
        }

        private void ComposerImageListBox_Drop(object sender, DragEventArgs e)
        {
            if (!ComposerImageListBox.IsEnabled)
            {
                return;
            }

            var imagePath = (string)e.Data.GetData(typeof(string));

            if (imagePath == null)
            {
                return;
            }

            var imageExtension = Path.GetExtension(imagePath);

            if (imageExtension == ".jpg" || imageExtension == ".png" || imageExtension == ".gif" || imageExtension == ".jpeg")
            {
                var composer = (Composer)ComposerListBox.SelectedItem;

                var composerImage = new ComposerImage();
                composerImage.Composer = composer;
                composerImage.Image = FileUtility.GetFile(imagePath);

                composer.ComposerImages.Add(composerImage);

                ComposerImageListBox.ItemsSource = composer.ComposerImages.Select(ci => ComposerImageUtility.ToBitmapImage(ci)).ToList();
                ComposerImageListBox.SelectedIndex = composer.ComposerImages.Count - 1;
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
            if (ComposerInfluenceListBox.IsEnabled && _currentComposers.Count == 1)
            {
                _currentComposers[0].Influences.Add((Composer)e.Data.GetData(typeof(Composer)));

                ComposerInfluenceListBox.ItemsSource = new List<Composer>(_currentComposers[0].Influences);
            }
        }

        private void ComposerLinkDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ComposerLinkListBox.IsEnabled)
            {
                var listBoxItem = e.OriginalSource as ListBoxItem;

                if (listBoxItem != null)
                {
                    var link = (Link)ComposerLinkListBox.DataContext;
                    var composer = (Composer)ComposerListBox.SelectedItem;

                    var targetLink = composer.ComposerLinks.FirstOrDefault(cl => cl.URL == link.Url);

                    if (targetLink != null)
                    {
                        composer.ComposerLinks.Remove(targetLink);
                    }
                }
            }
        }

        private void ComposerLinkListBox_Drop(object sender, DragEventArgs e)
        {
            if (ComposerLinkListBox.IsEnabled)
            {
                var data = (string)e.Data.GetData(typeof(string));

                try
                {
                    if (!data.StartsWith("http"))
                    {
                        data = "http://" + data;
                    }

                    var webResponse = WebRequest.Create(data).GetResponse();

                    var composerLink = new ComposerLink();
                    composerLink.Composer = (Composer)ComposerListBox.SelectedItem;
                    composerLink.URL = data;

                    ((Composer)ComposerListBox.SelectedItem).ComposerLinks.Add(composerLink);
                    ComposerLinkListBox.GetBindingExpression(ItemsControl.ItemsSourceProperty).UpdateTarget();

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
                var composerName = (string)e.Data.GetData(typeof(string));

                if (composerName != null)
                {
                    var composer = new Composer();
                    composer.Name = composerName;

                    App.DataProvider.Composers.Add(composer);

                    _currentComposers = new ObservableCollection<Composer> { composer };

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

            _currentComposers = new ObservableCollection<Composer>(ComposerListBox.SelectedItems.Cast<Composer>());

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
                    nationality.Composers.Add(composer);
                }

                foreach (Nationality nationality in e.RemovedItems)
                {
                    composer.Nationalities.Remove(nationality);
                    nationality.Composers.Remove(composer);
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

            var selectedCatalogs = _currentComposers
                .SelectMany(c => c.CompositionCatalogs)
                .Where(cc => cc == CompositionCollectionCatalogPrefixListBox.SelectedItem);

            var selectedCatalogNumbers = selectedCatalogs
                .SelectMany(cc => cc.CatalogNumbers)
                .Where(cn => cn.CompositionCollection == _currentCompositionCollection);

            if (selectedCatalogNumbers.Count() == 0)
            {
                var catalogNumbers = new List<CatalogNumber>();

                foreach (var catalog in selectedCatalogs)
                {
                    var catalogNumber = new CatalogNumber();
                    catalogNumber.CompositionCatalog = catalog;
                    catalogNumber.CompositionCollection = _currentCompositionCollection;

                    _currentCompositionCollection.CatalogNumbers.Add(catalogNumber);
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

            foreach (var composer in _currentComposers)
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
                var droppedString = (string)e.Data.GetData(typeof(string));

                if (droppedString == null)
                {
                    return;
                }

                var newCompositionCatalogs = new CompositionCatalog[_currentComposers.Count];

                for (int i = 0; i < _currentComposers.Count; i++)
                {
                    var newCompositionCatalog = new CompositionCatalog();
                    newCompositionCatalog.Prefix = droppedString;
                    newCompositionCatalog.Composer = _currentComposers[i];

                    _currentComposers[i].CompositionCatalogs.Add(newCompositionCatalog);
                    newCompositionCatalogs[i] = newCompositionCatalog;
                }

                var availableCompositionCatalogs = _currentComposers
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

            CompositionCollectionCatalogNumberTextBox.Text = _currentCompositionCollection.CatalogNumbers.FirstOrDefault(cn => cn.CompositionCatalog == CompositionCollectionCatalogPrefixListBox.SelectedItem)?.Number;
        }

        private void CompositionCollectionDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void CompositionCollectionListBox_Drop(object sender, DragEventArgs e)
        {
            if (CompositionCollectionListBox.IsEnabled)
            {
                var droppedString = (string)e.Data.GetData(typeof(string));

                if (droppedString == null)
                {
                    return;
                }

                _currentCompositionCollection = new CompositionCollection();
                _currentCompositionCollection.Name = droppedString;
                _currentCompositionCollection.Composers = _currentComposers;

                foreach (var composer in _currentComposers)
                {
                    composer.CompositionCollections.Add(_currentCompositionCollection);
                }

                CompositionCollectionListBox.ItemsSource = new List<CompositionCollection>(_currentComposers.Common(c => c.CompositionCollections));
                CompositionCollectionListBox.SelectedItem = _currentCompositionCollection;
            }
        }

        private void CompositionCollectionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CompositionCollectionListBox.IsEnabled)
            {
                _currentCompositionCollection = (CompositionCollection)CompositionCollectionListBox.SelectedItem;

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

            var selectedCatalogs = _currentComposers
                .SelectMany(c => c.CompositionCatalogs)
                .Where(cc => cc == CompositionCatalogPrefixListBox.SelectedItem);

            var selectedCatalogNumbers = selectedCatalogs
                .SelectMany(cc => cc.CatalogNumbers)
                .Where(cn => cn.Composition == _currentComposition);

            if (selectedCatalogNumbers.Count() == 0)
            {
                var catalogNumbers = new List<CatalogNumber>();

                foreach (var catalog in selectedCatalogs)
                {
                    var catalogNumber = new CatalogNumber();
                    catalogNumber.CompositionCatalog = catalog;
                    catalogNumber.Composition = _currentComposition;

                    _currentComposition.CatalogNumbers.Add(catalogNumber);
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

            foreach (var composer in _currentComposers)
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

            var droppedString = (string)e.Data.GetData(typeof(string));

            if (droppedString == null)
            {
                return;
            }

            var newCompositionCatalogs = new CompositionCatalog[_currentComposers.Count];

            for (int i = 0; i < _currentComposers.Count; i++)
            {
                var newCompositionCatalog = new CompositionCatalog();
                newCompositionCatalog.Prefix = droppedString;
                newCompositionCatalog.Composer = _currentComposers[i];

                App.DataProvider.CompositionCatalogs.Add(newCompositionCatalog);
                _currentComposers[i].CompositionCatalogs.Add(newCompositionCatalog);
                newCompositionCatalogs[i] = newCompositionCatalog;
            }

            var availableCompositionCatalogs = _currentComposers
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

            CompositionCatalogNumberTextBox.Text = _currentComposition.CatalogNumbers.FirstOrDefault(cn => cn.CompositionCatalog == CompositionCatalogPrefixListBox.SelectedItem)?.Number;
        }

        private void CompositionDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void CompositionListBox_Drop(object sender, DragEventArgs e)
        {
            if (!CompositionListBox.IsEnabled)
            {
                return;
            }

            var compositionName = (string)e.Data.GetData(typeof(string));

            if (compositionName == null)
            {
                return;
            }

            if (_currentCompositionCollection != null)
            {
                _currentComposition = _currentCompositionCollection.Compositions.FirstOrDefault(c => c.Name == compositionName);

                if (_currentComposition == null)
                {
                    _currentComposition = new Composition();
                    _currentComposition.Name = compositionName;
                    App.DataProvider.Compositions.Add(_currentComposition);
                }

                _currentComposition.CompositionCollection = _currentCompositionCollection;
                _currentCompositionCollection.Compositions.Add(_currentComposition);

                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_currentCompositionCollection.Compositions, null, "Name"));
            }
            else
            {
                _currentComposition = _currentComposers.Common(c => c.Compositions).FirstOrDefault(c => c.Name == compositionName);

                if (_currentComposition == null)
                {
                    _currentComposition = new Composition();
                    _currentComposition.Name = compositionName;
                    App.DataProvider.Compositions.Add(_currentComposition);
                }

                foreach (var composer in _currentComposers)
                {
                    composer.Compositions.Add(_currentComposition);
                }

                _currentComposition.Composers = _currentComposers;

                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(_currentComposers.Common(c => c.Compositions), null, "Name"));
            }

            CompositionListBox.SelectedItem = _currentComposition;
        }

        private void CompositionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!CompositionListBox.IsEnabled)
            {
                return;
            }

            _currentComposition = (Composition)CompositionListBox.SelectedItem;

            RefreshCompositionSection();
        }

        #endregion Composition Section Events

        #region Movement Section Events

        private void MovementListBox_Drop(object sender, DragEventArgs e)
        {
            if (MovementListBox.IsEnabled)
            {
                var movementName = (string)e.Data.GetData(typeof(string));

                if (movementName != null)
                {
                    _currentMovement = new Movement();
                    _currentMovement.Name = movementName;
                    _currentMovement.Composition = _currentComposition;

                    _currentComposition.Movements.Add(_currentMovement);

                    MovementListBox.ItemsSource = new List<Movement>(_currentComposition.Movements);
                    MovementListBox.SelectedItem = _currentMovement;
                }
            }
        }

        private void MovementListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MovementListBox.IsEnabled)
            {
                _currentMovement = (Movement)MovementListBox.SelectedItem;

                RefreshMovementSection();
            }
        }

        private void MovementDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MovementListBox.IsEnabled)
            {
                App.DataProvider.Movements.Local.Remove((Movement)MovementListBox.SelectedItem);
            }
        }

        #endregion Movement Section Events

        #region Recording Section Events

        private void RecordingAlbumAutoCompleteBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var recordingAlbum = App.DataProvider.Albums.Local.FirstOrDefault(l => l.Name == RecordingAlbumAutoCompleteBox.Text);

            if (recordingAlbum == null)
            {
                if (_currentRecording.Album != null && _currentRecording.Album.Recordings.Count == 1)
                {
                    App.DataProvider.Albums.Remove(_currentRecording.Album);
                }

                if (string.IsNullOrEmpty(RecordingAlbumAutoCompleteBox.Text))
                {
                    _currentRecording.Album = null;
                }
                else
                {
                    var album = new Album();
                    album.Name = RecordingAlbumAutoCompleteBox.Text;
                    album.Recordings.Add(_currentRecording);

                    _currentRecording.Album = album;
                }
            }
            else
            {
                if (_currentRecording.Album != null && recordingAlbum.Name != _currentRecording.Album.Name && _currentRecording.Album.Recordings.Count == 1)
                {
                    App.DataProvider.Albums.Remove(_currentRecording.Album);
                }

                _currentRecording.Album = recordingAlbum;
            }
        }

        private void RecordingListBox_Drop(object sender, DragEventArgs e)
        {
            if (!RecordingListBox.IsEnabled)
            {
                return;
            }

            var recordingPath = (string)e.Data.GetData(typeof(string));

            if (recordingPath == null)
            {
                return;
            }

            var recordingId = App.DataProvider.Recordings.Count();

            var importPathBuilder = new StringBuilder(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic))
                .Append("\\")
                .Append(_currentComposers.Count > 1 ? "Various" : _currentComposers[0].Name)
                .Append("\\");

            if (_currentCompositionCollection != null)
            {
                importPathBuilder.Append(_currentCompositionCollection.Name).Append("\\");
            }

            if (_currentComposition != null)
            {
                importPathBuilder.Append(_currentComposition.Name).Append("\\");
            }

            if (_currentMovement != null)
            {
                importPathBuilder.Append($"{_currentMovement.Number}. {_currentMovement.Name}").Append("\\");
            }

            importPathBuilder.Append(recordingId).Append(Path.GetExtension(recordingPath));

            var importPath = importPathBuilder.ToString();

            Directory.CreateDirectory(Path.GetDirectoryName(importPath));

            if (!File.Exists(importPath))
            {
                File.Copy(recordingPath, importPath);
            }

            LibraryDb.DataProvider.Add(recordingId, importPath);

            _currentRecording = new Recording();
            _currentRecording.ID = recordingId;

            if (_currentMovement != null)
            {
                _currentRecording.Movement = _currentMovement;
                _currentMovement.Recordings.Add(_currentRecording);

                RecordingListBox.ItemsSource = new List<Recording>(_currentMovement.Recordings);
            }
            else if (_currentComposition != null)
            {
                _currentRecording.Composition = _currentComposition;
                _currentComposition.Recordings.Add(_currentRecording);

                RecordingListBox.ItemsSource = new List<Recording>(_currentComposition.Recordings);
            }
            else if (_currentCompositionCollection != null)
            {
                _currentRecording.CompositionCollection = _currentCompositionCollection;
                _currentCompositionCollection.Recordings.Add(_currentRecording);

                RecordingListBox.ItemsSource = new List<Recording>(_currentCompositionCollection.Recordings);
            }

            RecordingListBox.SelectedItem = _currentRecording;
        }

        private void RecordingListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecordingListBox.IsEnabled)
            {
                _currentRecording = (Recording)RecordingListBox.SelectedItem;

                RefreshRecordingSection();
            }
        }

        private void RecordingDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (RecordingListBox.IsEnabled)
            {
                App.DataProvider.Recordings.Local.Remove((Recording)RecordingListBox.SelectedItem);
            }
        }

        private void RecordingPerformerDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (RecordingPerformerListBox.IsEnabled)
            {
                var performer = (Performer)RecordingPerformerListBox.SelectedItem;

                _currentRecording.Performers.Remove(performer);
                performer.Recordings.Remove(_currentRecording);

                if (performer.Recordings.Count == 0)
                {
                    App.DataProvider.Performers.Local.Remove(performer);
                }
            }
        }

        private void RecordingPerformerListBox_Drop(object sender, DragEventArgs e)
        {
            if (!RecordingPerformerListBox.IsEnabled)
            {
                return;
            }

            var performerName = (string)e.Data.GetData(typeof(string));

            if (performerName == null)
            {
                return;
            }

            var performer = App.DataProvider.Performers.FirstOrDefault(p => p.Name == performerName);

            if (performer == null)
            {
                performer = new Performer();
                performer.ID = App.DataProvider.Performers.Local.Count + 1;
                performer.Name = performerName;
                App.DataProvider.Performers.Local.Add(performer);
            }

            performer.Recordings.Add(_currentRecording);
            _currentRecording.Performers.Add(performer);

            RecordingPerformerListBox.SelectedItem = performer;
        }

        private void RecordingLocationListBox_Drop(object sender, DragEventArgs e)
        {
            if (!RecordingLocationListBox.IsEnabled)
            {
                return;
            }

            var locationName = (string)e.Data.GetData(typeof(string));

            if (locationName == null)
            {
                return;
            }

            var location = App.DataProvider.Locations.FirstOrDefault(l => l.Name == locationName);

            if (location == null)
            {
                location = new Location();
                location.Name = locationName;
                App.DataProvider.Locations.Local.Add(location);
            }

            location.Recordings.Add(_currentRecording);
            _currentRecording.Locations.Add(location);
        }

        private void RecordingLocationDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (RecordingLocationListBox.IsEnabled)
            {
                var location = (Location)RecordingLocationListBox.SelectedItem;

                _currentRecording.Locations.Remove(location);
                location.Recordings.Remove(_currentRecording);

                if (location.Recordings.Count == 0 && location.BirthLocationComposers.Count == 0 && location.DeathLocationComposers.Count == 0)
                {
                    App.DataProvider.Locations.Local.Remove(location);
                }
            }
        }

        #endregion Recording Section Events

        #region Status and Button Section Events

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Application.Current.MainWindow.FindName("Frame")).GoBack();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            using (App.DataProvider)
            {
                App.DataProvider.SaveChanges();
            }

            App.DataProvider = new DataProvider();

            ((Frame)Application.Current.MainWindow.FindName("Frame")).Navigate(new Uri(@"pack://application:,,,/Views/TimelinePage.xaml"));
        }

        #endregion Status and Button Section Events
    }
}