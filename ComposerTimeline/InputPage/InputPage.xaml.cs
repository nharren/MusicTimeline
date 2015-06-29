using Database;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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

        public IList<Composer> CurrentComposers
        {
            get
            {
                return _currentComposers;
            }

            set
            {
                _currentComposers = value;
            }
        }

        public Composition CurrentComposition
        {
            get
            {
                return _currentComposition;
            }

            set
            {
                _currentComposition = value;
            }
        }

        public CompositionCollection CurrentCompositionCollection
        {
            get
            {
                return _currentCompositionCollection;
            }

            set
            {
                _currentCompositionCollection = value;
            }
        }

        public Movement CurrentMovement
        {
            get
            {
                return _currentMovement;
            }

            set
            {
                _currentMovement = value;
            }
        }

        public Recording CurrentRecording
        {
            get
            {
                return _currentRecording;
            }

            set
            {
                _currentRecording = value;
            }
        }

        private static void AddPathToLibrary(string path)
        {
            using (var file = new FileStream("library.txt", FileMode.OpenOrCreate))
            {
                if (!FileUtility.HasLine(file, path))
                {
                    FileUtility.WriteLine(file, path);
                }
            }
        }

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
                var composer = (Composer)ComposerListBox.SelectedItem;

                composer.ComposerImages.Remove((ComposerImage)ComposerImageListBox.SelectedItem);
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
                var composer = (Composer)ComposerListBox.SelectedItem;

                composer.Influences.Remove((Composer)ComposerInfluenceListBox.SelectedItem);
            }
        }

        private void ComposerInfluenceListBox_Drop(object sender, DragEventArgs e)
        {
            if (ComposerInfluenceListBox.IsEnabled && _currentComposers.Count == 1)
            {
                var influence = (Composer)e.Data.GetData(typeof(Composer));
                _currentComposers[0].Influences.Add(influence);

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
                var data = (string)e.Data.GetData(typeof(string));

                if (!string.IsNullOrEmpty(data))
                {
                    var composer = new Composer();
                    composer.Name = data;

                    App.DataProvider.Composers.Add(composer);

                    _currentComposers = new List<Composer> { composer };

                    InputPageInitializer.BindComposers(this, _currentComposers);

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
            if (ComposerListBox.IsEnabled)
            {
                _currentComposers = ComposerListBox.SelectedItems.Cast<Composer>().ToList();

                InputPageInitializer.BindComposers(this, _currentComposers);
            }
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

                CompositionCollectionListBox.ItemsSource = _currentComposers
                    .Common(c => c.CompositionCollections)
                    .ToList();
                CompositionCollectionListBox.SelectedItem = _currentCompositionCollection;
            }
        }

        private void CompositionCollectionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CompositionCollectionListBox.IsEnabled)
            {
                _currentCompositionCollection = (CompositionCollection)CompositionCollectionListBox.SelectedItem;

                InputPageInitializer.BindCompositionCollection(this, _currentCompositionCollection);
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

            InputPageInitializer.BindComposition(this, _currentComposition);
        }

        #endregion Composition Section Events

        #region Movement Section Events

        private void MovementListBox_Drop(object sender, DragEventArgs e)
        {
            if (MovementListBox.IsEnabled)
            {
                var data = (string)e.Data.GetData(typeof(string));

                if (!string.IsNullOrEmpty(data))
                {
                    var movement = new Movement();
                    movement.Name = data;

                    _currentComposition.Movements.Add(movement);

                    _currentMovement = movement;

                    InputPageInitializer.BindMovement(this, _currentMovement);

                    MovementListBox.SelectedItem = movement;
                }
            }
        }

        private void MovementListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MovementListBox.IsEnabled)
            {
                _currentMovement = (Movement)MovementListBox.SelectedItem;

                InputPageInitializer.BindMovement(this, _currentMovement);
            }
        }

        #endregion Movement Section Events

        #region Recording Section Events

        private void RecordingListBox_Drop(object sender, DragEventArgs e)
        {
            if (RecordingListBox.IsEnabled)
            {
                var droppedString = (string)e.Data.GetData(typeof(string));
                var droppedStringExists = droppedString != null;

                if (droppedStringExists)
                {
                    AddPathToLibrary(droppedString);

                    _currentRecording = new Recording();

                    var currentMovementExists = _currentMovement != null;
                    var currentCompositionExists = _currentComposition != null;
                    var currentCompositionCollectionExists = _currentCompositionCollection != null;

                    if (currentMovementExists)
                    {
                        _currentRecording.Movement = _currentMovement;
                        _currentMovement.Recordings.Add(_currentRecording);
                    }
                    else if (currentCompositionExists)
                    {
                        _currentRecording.Composition = _currentComposition;
                        _currentComposition.Recordings.Add(_currentRecording);
                    }
                    else if (currentCompositionCollectionExists)
                    {
                        _currentRecording.CompositionCollection = _currentCompositionCollection;
                        _currentCompositionCollection.Recordings.Add(_currentRecording);
                    }

                    RecordingListBox.SelectedItem = _currentRecording;
                }
            }
        }

        private void RecordingListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecordingListBox.IsEnabled)
            {
                _currentRecording = (Recording)RecordingListBox.SelectedItem;

                InputPageInitializer.BindRecording(this, _currentRecording);
            }
        }

        #endregion Recording Section Events
    }
}