using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
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
            var file = new FileStream("library.txt", FileMode.OpenOrCreate);
            var reader = new StreamReader(file);

            if (!reader.HasLine(path))
            {
                var writer = new StreamWriter(file);

                writer.WriteLine(path);
            }

            file.Dispose();
        }

        #region Composer Section Events

        private void ComposerImageListBox_Drop(object sender, DragEventArgs e)
        {
            if (ComposerImageListBox.IsEnabled)
            {
                var path = (string)e.Data.GetData(typeof(string));
                var extension = Path.GetExtension(path);

                if (extension == ".jpg" || extension == ".png" || extension == ".gif" || extension == ".jpeg")
                {
                    var composer = (Composer)ComposerListBox.SelectedItem;
                    var composerImage = new ComposerImage { Composer = composer };

                    if (new Uri(path).IsFile)
                    {
                        composerImage.Image = File.ReadAllBytes(path);

                        composer.ComposerImages.Add(composerImage);
                    }
                    else
                    {
                        using (var webClient = new WebClient())
                        {
                            composerImage.Image = webClient.DownloadData(path);

                            composer.ComposerImages.Add(composerImage);
                        }
                    }

                    ComposerImageListBox.GetBindingExpression(ItemsControl.ItemsSourceProperty).UpdateTarget();
                    ComposerImageListBox.SelectedIndex = composer.ComposerImages.Count - 1;
                    ComposerImageListBox.ApplyTemplate();
                }
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

        private void ComposerDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ComposerListBox.IsEnabled)
            {
                App.DataProvider.Composers.Local.Remove((Composer)ComposerListBox.SelectedItem);
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

        private void ComposerInfluenceDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ComposerInfluenceListBox.IsEnabled)
            {
                var composer = (Composer)ComposerListBox.SelectedItem;

                composer.Influences.Remove((Composer)ComposerInfluenceListBox.SelectedItem);
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

        private void ComposerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComposerListBox.IsEnabled)
            {
                _currentComposers = ComposerListBox.SelectedItems.Cast<Composer>().ToList();

                InputPageInitializer.BindComposers(this, _currentComposers);
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

        #endregion Composer Section Events

        #region Composition Collection Section

        private void CompositionCollectionListBox_Drop(object sender, DragEventArgs e)
        {
            if (CompositionCollectionListBox.IsEnabled)
            {
                var droppedString = (string)e.Data.GetData(typeof(string));
                var droppedStringExists = droppedString != null;

                if (droppedStringExists)
                {
                    _currentCompositionCollection = new CompositionCollection();
                    _currentCompositionCollection.Name = droppedString;

                    foreach (var composer in _currentComposers)
                    {
                        composer.CompositionCollections.Add(_currentCompositionCollection);
                    }

                    var commonCompositionCollections = _currentComposers
                        .Common(c => c.CompositionCollections);

                    CompositionCollectionListBox.ItemsSource = new List<CompositionCollection>(commonCompositionCollections);
                    CompositionCollectionListBox.SelectedItem = _currentCompositionCollection;
                }
            }
        }

        private void CompositionCollectionDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void CompositionCollectionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CompositionCollectionListBox.IsEnabled)
            {
                _currentCompositionCollection = (CompositionCollection)CompositionCollectionListBox.SelectedItem;

                InputPageInitializer.BindCompositionCollection(this, _currentCompositionCollection);
            }
        }

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

        private void CompositionCollectionCatalogPrefixListBox_Drop(object sender, DragEventArgs e)
        {
            if (!CompositionCollectionCatalogPrefixListBox.IsEnabled)
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

                _currentComposers[i]
            }

            var availableCompositionCatalogs = _currentComposers
                .SelectMany(c => c.CompositionCatalogs)
                .Select(cc => cc.Prefix)
                .Distinct();

            CompositionCollectionCatalogPrefixListBox.ItemsSource = new List<string>(availableCompositionCatalogs);
            CompositionCollectionCatalogPrefixListBox.SelectedItem = droppedString;

            CompositionCollectionCatalogNumberTextBox.Text = null;
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

        #endregion Composition Collection Section

        #region Composition Section Events

        private void CompositionListBox_Drop(object sender, DragEventArgs e)
        {
            if (CompositionListBox.IsEnabled)
            {
                var droppedString = (string)e.Data.GetData(typeof(string));
                var droppedStringExists = droppedString != null;

                if (droppedStringExists)
                {
                    _currentComposition = new Composition();
                    _currentComposition.Name = droppedString;

                    var compositionNotInCollection = _currentCompositionCollection == null;

                    if (compositionNotInCollection)
                    {
                        foreach (var composer in _currentComposers)
                        {
                            composer.Compositions.Add(_currentComposition);
                        }

                        var commonCompositions = _currentComposers
                            .Common(c => c.Compositions);

                        CompositionListBox.ItemsSource = new List<Composition>(commonCompositions);
                    }
                    else
                    {
                        _currentCompositionCollection.Compositions.Add(_currentComposition);

                        CompositionListBox.ItemsSource = new List<Composition>(_currentCompositionCollection.Compositions);
                    }

                    CompositionListBox.SelectedItem = _currentComposition;
                }
            }
        }

        private void CompositionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CompositionListBox.IsEnabled)
            {
                _currentComposition = (Composition)CompositionListBox.SelectedItem;

                InputPageInitializer.BindComposition(this, _currentComposition);
            }
        }

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

            CompositionCatalogPrefixListBox.ItemsSource = new List<string>(availableCompositionCatalogs);
            CompositionCatalogPrefixListBox.SelectedItem = droppedString;

            CompositionCatalogNumberTextBox.Text = null;
        }

        private void CompositionCatalogPrefixListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
            {
                return;
            }

            var catalogNumber = _currentCompositionCollection.CatalogNumbers
                .FirstOrDefault(cn => cn.CompositionCatalog.Prefix == (string)CompositionCollectionCatalogPrefixListBox.SelectedItem);

            CompositionCollectionCatalogNumberTextBox.Text = catalogNumber == null ? null : catalogNumber.Number;
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