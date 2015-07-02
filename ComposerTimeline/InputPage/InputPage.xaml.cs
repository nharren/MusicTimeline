using Database;
using NathanHarrenstein.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline
{
    public partial class InputPage : Page
    {
        private IList<Composer> _currentComposers;
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
                var composer = _currentComposers.First();

                ComposerNameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(composer, "Name"));
                ComposerDatesTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(composer, "Dates"));
                ComposerBirthLocationAutoCompleteBox.Text = composer.BirthLocation != null ? composer.BirthLocation.Name : null;
                ComposerDeathLocationAutoCompleteBox.Text = composer.DeathLocation != null ? composer.DeathLocation.Name : null;
                ComposerInfluenceListBox.ItemsSource = new List<Composer>(composer.Influences);
                ComposerImageListBox.ItemsSource = new List<BitmapImage>(composer.ComposerImages.Select(ci => ComposerImageUtility.ToBitmapImage(ci)));
                ComposerLinkListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingUtility.Create(composer, "ComposerLinks", new ComposerLinkConverter()));
                ComposerBiographyTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(composer, "Biography"));
                CompositionCollectionListBox.ItemsSource = new List<CompositionCollection>(composer.CompositionCollections);
                CompositionListBox.ItemsSource = new List<Composition>(composer.Compositions);

                ComposerNationalityListBox.SelectedItems.Clear();
                ListUtility.AddMany(ComposerNationalityListBox.SelectedItems, composer.Nationalities);

                ComposerEraListBox.SelectedItems.Clear();
                ListUtility.AddMany(ComposerEraListBox.SelectedItems, composer.Eras);
            }
            else
            {
                ClearComposerSection();

                ComposerNameTextBox.Text = "<< Multiple Selection >>";
                ComposerDatesTextBox.Text = "<< Multiple Selection >>";
                ComposerBirthLocationAutoCompleteBox.Text = "<< Multiple Selection >>";
                ComposerDeathLocationAutoCompleteBox.Text = "<< Multiple Selection >>";
                ComposerBiographyTextBox.Text = "<< Multiple Selection >>";
                CompositionCollectionListBox.ItemsSource = _currentComposers.Common(c => c.CompositionCollections);
                CompositionListBox.ItemsSource = _currentComposers.Common(c => c.Compositions);
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
            App.EnsureDbConnection();

            CompositionCollectionNameTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_currentCompositionCollection, "Name"));
            CompositionCollectionCatalogPrefixListBox.ItemsSource = new List<string>(_currentCompositionCollection.Composers.SelectMany(c => c.CompositionCatalogs).Select(cc => cc.Prefix));

            var catalogNumber = _currentCompositionCollection.CatalogNumbers.FirstOrDefault(cn => cn.CompositionCatalog.Prefix == (string)CompositionCollectionCatalogPrefixListBox.SelectedItem);
            CompositionCollectionCatalogNumberTextBox.Text = catalogNumber == null ? null : catalogNumber.Number;

            CompositionListBox.ItemsSource = new List<Composition>(_currentCompositionCollection.Compositions);
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

            CompositionCatalogPrefixListBox.ItemsSource = _currentComposition.Composers.SelectMany(c => c.CompositionCatalogs).Select(cc => cc.Prefix).ToList();

            var catalogNumber = _currentComposition.CatalogNumbers.FirstOrDefault(cn => cn.CompositionCatalog.Prefix == (string)CompositionCatalogPrefixListBox.SelectedItem);
            CompositionCatalogNumberTextBox.Text = catalogNumber == null ? null : catalogNumber.Number;

            MovementListBox.ItemsSource = new List<Movement>(_currentComposition.Movements);
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
            RecordingListBox.ItemsSource = new List<Recording>(_currentMovement.Recordings);
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
            RecordingLocationAutoCompleteBox.Text = null;
        }

        public void DisableRecordingSection()
        {
            RecordingPerformerListBox.IsEnabled = false;
            RecordingAlbumAutoCompleteBox.IsEnabled = false;
            RecordingTrackNumberBox.IsEnabled = false;
            RecordingDatesTextBox.IsEnabled = false;
            RecordingLocationAutoCompleteBox.IsEnabled = false;
        }

        public void EnableRecordingSection()
        {
            RecordingPerformerListBox.IsEnabled = true;
            RecordingAlbumAutoCompleteBox.IsEnabled = true;
            RecordingTrackNumberBox.IsEnabled = true;
            RecordingDatesTextBox.IsEnabled = true;
            RecordingLocationAutoCompleteBox.IsEnabled = true;
        }

        public void LoadRecordingSection()
        {
            App.EnsureDbConnection();

            RecordingPerformerListBox.ItemsSource = new List<Performer>(App.DataProvider.Performers);
            ListUtility.AddMany(RecordingPerformerListBox.SelectedItems, _currentRecording.Performers);

            if (_currentRecording.Album != null)
            {
                RecordingAlbumAutoCompleteBox.Text = _currentRecording.Album.Name;
            }

            RecordingTrackNumberBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_currentRecording, "TrackNumber"));
            RecordingDatesTextBox.SetBinding(TextBox.TextProperty, BindingUtility.Create(_currentRecording, "Dates"));

            if (_currentRecording.Location != null)
            {
                RecordingLocationAutoCompleteBox.Text = _currentRecording.Location.Name;
            }
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
                    composer.BirthLocation = new Location { Name = ComposerBirthLocationAutoCompleteBox.Text };
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

                    var composer = (Composer)ComposerListBox.SelectedItem;
                    composer.ComposerLinks.Add(new ComposerLink { Composer = composer, URL = data });

                    ComposerLinkListBox.GetBindingExpression(ItemsControl.ItemsSourceProperty).UpdateTarget();
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

                    _currentComposers = new List<Composer> { composer };

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

            _currentComposers = ComposerListBox.SelectedItems.Cast<Composer>().ToList();

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
                var exists = clickedElement != null;
                var notScrollBar = !(clickedElement is Thumb);

                if (exists && notScrollBar)
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

                foreach (var addedItem in e.AddedItems)
                {
                    composer.Nationalities.Add((Nationality)addedItem);
                }

                foreach (var removedItem in e.RemovedItems)
                {
                    composer.Nationalities.Remove((Nationality)removedItem);
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
                .Where(cc => cc.Prefix == (string)CompositionCollectionCatalogPrefixListBox.SelectedItem);

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

            var catalogNumber = _currentCompositionCollection.CatalogNumbers
                .FirstOrDefault(cn => cn.CompositionCatalog.Prefix == (string)CompositionCollectionCatalogPrefixListBox.SelectedItem);

            CompositionCollectionCatalogNumberTextBox.Text = catalogNumber == null ? null : catalogNumber.Number;
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
                .Where(cc => cc.Prefix == (string)CompositionCatalogPrefixListBox.SelectedItem);

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

            var catalogNumber = _currentComposition.CatalogNumbers
                .FirstOrDefault(cn => cn.CompositionCatalog.Prefix == (string)CompositionCatalogPrefixListBox.SelectedItem);

            CompositionCatalogNumberTextBox.Text = catalogNumber == null ? null : catalogNumber.Number;
        }

        private void CompositionDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void CompositionListBox_Drop(object sender, DragEventArgs e)
        {
            if (CompositionListBox.IsEnabled)
            {
                var droppedString = (string)e.Data.GetData(typeof(string));
                var droppedStringExists = droppedString != null;

                if (!droppedStringExists)
                {
                    return;
                }

                _currentComposition = new Composition();
                _currentComposition.Name = droppedString;
                _currentComposition.Composers = _currentComposers;

                if (_currentCompositionCollection == null)
                {
                    foreach (var composer in _currentComposers)
                    {
                        composer.Compositions.Add(_currentComposition);
                    }

                    CompositionListBox.ItemsSource = _currentComposers
                        .Common(c => c.Compositions)
                        .ToList();
                }
                else
                {
                    _currentCompositionCollection.Compositions.Add(_currentComposition);

                    CompositionListBox.ItemsSource = _currentCompositionCollection.Compositions.ToList();
                }

                CompositionListBox.SelectedItem = _currentComposition;
            }
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

        private void RecordingLocationAutoCompleteBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var recordingLocation = App.DataProvider.Locations.Local.FirstOrDefault(l => l.Name == RecordingLocationAutoCompleteBox.Text);

            if (recordingLocation == null)
            {
                if (_currentRecording.Location != null && _currentRecording.Location.BirthLocationComposers.Count + _currentRecording.Location.DeathLocationComposers.Count + _currentRecording.Location.Recordings.Count == 1)
                {
                    App.DataProvider.Locations.Remove(_currentRecording.Location);
                }

                if (string.IsNullOrEmpty(RecordingLocationAutoCompleteBox.Text))
                {
                    _currentRecording.Location = null;
                }
                else
                {
                    var location = new Location();
                    location.Name = RecordingLocationAutoCompleteBox.Text;
                    location.Recordings.Add(_currentRecording);

                    _currentRecording.Location = location;
                }
            }
            else
            {
                if (_currentRecording.Location != null && recordingLocation.Name != _currentRecording.Location.Name && _currentRecording.Location.BirthLocationComposers.Count + _currentRecording.Location.DeathLocationComposers.Count + _currentRecording.Location.Recordings.Count == 1)
                {
                    App.DataProvider.Locations.Remove(_currentRecording.Location);
                }

                _currentRecording.Location = recordingLocation;
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

            Library.AddFilePath(recordingPath);

            _currentRecording = new Recording();

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
                _currentRecording.Performers.Remove((Performer)RecordingPerformerListBox.SelectedItem);
            }
        }

        private void RecordingPerformerListBox_Drop(object sender, DragEventArgs e)
        {
            if (!RecordingPerformerListBox.IsEnabled)
            {
                return;
            }

            var performerName = (string)e.Data.GetData(typeof(string));

            if (performerName != null)
            {
                var performer = new Performer();
                performer.Name = performerName;
                performer.Recordings.Add(_currentRecording);

                _currentRecording.Performers.Add(performer);
                App.DataProvider.Performers.Add(performer);

                RecordingPerformerListBox.ItemsSource = new List<Performer>(App.DataProvider.Performers);
                RecordingPerformerListBox.SelectedItem = performer;
            }
        }

        #endregion Recording Section Events
    }
}