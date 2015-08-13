﻿using HTMLConverter;
using Luminescence.Xiph;
using NathanHarrenstein.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Builders;
using NathanHarrenstein.MusicTimeline.Controls;
using NathanHarrenstein.MusicTimeline.Converters;
using NathanHarrenstein.MusicTimeline.Extensions;
using NathanHarrenstein.MusicTimeline.Logging;
using NathanHarrenstein.MusicTimeline.Scrapers;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class InputPage : Page, IDisposable
    {
        private ClassicalMusicDbContext _classicalMusicDbContext;
        private Thread _dataLoadingThread;
        private ObservableCollection<Composer> _selectedComposers;
        private Composition _selectedComposition;
        private CompositionCollection _selectedCompositionCollection;
        private Movement _selectedMovement;
        private Recording _selectedRecording;

        public InputPage()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Initialize();
            }
        }

        #region Initialization

        private Queue<Action> _dataLoadingQueue = new Queue<Action>();

        private event EventHandler InitialDataLoaded;

        protected virtual void OnInitialDataLoaded()
        {
            if (InitialDataLoaded != null)
            {
                InitialDataLoaded(this, null);
            }
        }

        private void Initialize()
        {
            InitializeAutoCompleteBoxStringSelectors();

            InitialDataLoaded += InputPage_InitialDataLoaded;

            InitializeDataThread();
        }

        private void InitializeAutoCompleteBoxStringSelectors()
        {
            var stringSelector = new Func<object, string>(o => ((Location)o).Name);

            ComposerBirthLocationAutoCompleteBox.StringSelector = stringSelector;
            ComposerDeathLocationAutoCompleteBox.StringSelector = stringSelector;
        }

        private void InitializeDataThread()
        {
            _dataLoadingThread = new Thread(new ThreadStart(StartDataLoadingLoop));
            _dataLoadingThread.Name = "Data Loading Thread";
            _dataLoadingThread.IsBackground = true;
            _dataLoadingThread.Start();

            _dataLoadingQueue.Enqueue(LoadInitialData);
        }

        private void InputPage_InitialDataLoaded(object sender, EventArgs e)
        {
            SetInitialBindings();
        }

        private void LoadInitialData()
        {
            _classicalMusicDbContext = new ClassicalMusicDbContext();
            _classicalMusicDbContext.Composers.Load();
            _classicalMusicDbContext.Eras.Load();
            _classicalMusicDbContext.Locations.Load();
            _classicalMusicDbContext.Nationalities.Load();

            Dispatcher.Invoke(OnInitialDataLoaded);
        }

        private void SetInitialBindings()
        {
            ComposerListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_classicalMusicDbContext.Composers.Local.OrderBy(c => c.Name)));
            ComposerNationalityListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_classicalMusicDbContext.Nationalities.Local.OrderBy(c => c.Name)));
            ComposerEraListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_classicalMusicDbContext.Eras.Local));
            ComposerBirthLocationAutoCompleteBox.SetBinding(AutoCompleteBox.SuggestionsProperty, BindingBuilder.Build(_classicalMusicDbContext.Locations.Local));
            ComposerDeathLocationAutoCompleteBox.SetBinding(AutoCompleteBox.SuggestionsProperty, BindingBuilder.Build(_classicalMusicDbContext.Locations.Local));
        }

        private void StartDataLoadingLoop()
        {
            while (!_isDisposed)
            {
                if (_dataLoadingQueue.Count > 0)
                {
                    _dataLoadingQueue.Dequeue()();
                }
            }
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

                ComposerNameTextBox.SetBinding(TextBox.TextProperty, BindingBuilder.Build(composer, "Name"));
                ComposerDatesTextBox.SetBinding(TextBox.TextProperty, BindingBuilder.Build(composer, "Dates"));
                ComposerBirthLocationAutoCompleteBox.Text = composer.BirthLocation?.Name;
                ComposerDeathLocationAutoCompleteBox.Text = composer.DeathLocation?.Name;
                ComposerInfluenceListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(composer.Influences, null, "Name"));
                ComposerImageListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(composer.ComposerImages, null));
                ComposerLinkListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(composer.Links, null));
                ComposerBiographyTextBox.SetBinding(TextBox.TextProperty, BindingBuilder.Build(composer, "Biography"));
                CompositionCollectionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(composer.CompositionCollections, null, "Name"));
                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(composer.Compositions, null, "Name"));

                var nationalities = composer.Nationalities.ToList();
                var eras = composer.Eras.ToList();

                foreach (var nationality in nationalities)
                {
                    ComposerNationalityListBox.SelectedItems.Add(nationality);
                }

                foreach (var era in eras)
                {
                    ComposerEraListBox.SelectedItems.Add(era);
                }

                if (nationalities.Count > 0)
                {
                    ComposerNationalityListBox.ScrollIntoView(nationalities.First());
                }

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
                CompositionCollectionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_selectedComposers.Common(c => c.CompositionCollections), null, "Name"));
                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_selectedComposers.Common(c => c.Compositions), null, "Name"));
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
            CompositionCollectionNameTextBox.SetBinding(TextBox.TextProperty, BindingBuilder.Build(_selectedCompositionCollection, "Name"));
            CompositionCollectionCatalogPrefixListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_selectedCompositionCollection.Composers.SelectMany(c => c.Catalogs), null, "Prefix"));
            CompositionCollectionCatalogNumberTextBox.Text = _selectedCompositionCollection.CatalogNumbers.FirstOrDefault(cn => cn.Catalog.Prefix == (string)CompositionCollectionCatalogPrefixListBox.SelectedItem)?.Value;
            CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_selectedCompositionCollection.Compositions, null, "Name"));

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
            CompositionNameTextBox.SetBinding(TextBox.TextProperty, BindingBuilder.Build(_selectedComposition, "Name"));
            CompositionNicknameTextBox.SetBinding(TextBox.TextProperty, BindingBuilder.Build(_selectedComposition, "Nickname"));
            CompositionDatesTextBox.SetBinding(TextBox.TextProperty, BindingBuilder.Build(_selectedComposition, "Dates"));
            CompositionCatalogNumberTextBox.Text = _selectedComposition.CatalogNumbers.FirstOrDefault(cn => cn.Catalog.Prefix == (string)CompositionCatalogPrefixListBox.SelectedItem)?.Value;
            MovementListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_selectedComposition.Movements, null, "Number"));

            if (_selectedCompositionCollection != null)
            {
                CompositionCatalogPrefixListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_selectedCompositionCollection.Composers.SelectMany(c => c.Catalogs), null, "Prefix"));
            }
            else
            {
                CompositionCatalogPrefixListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_selectedComposition.Composers.SelectMany(c => c.Catalogs), null, "Prefix"));
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
            MovementNumberBox.SetBinding(TextBox.TextProperty, BindingBuilder.Build(_selectedMovement, "Number"));
            MovementNameTextBox.SetBinding(TextBox.TextProperty, BindingBuilder.Build(_selectedMovement, "Name"));
            RecordingListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_selectedMovement.Recordings, null, "ID"));
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
            RecordingPathTextBox.Text = LibraryDB.DataProvider.Get(_selectedRecording.Id).FirstOrDefault();
            RecordingPerformerListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_classicalMusicDbContext.Performers.Local, null, "Name"));
            RecordingAlbumAutoCompleteBox.Text = _selectedRecording.Album?.Name;
            RecordingTrackNumberBox.SetBinding(TextBox.TextProperty, BindingBuilder.Build(_selectedRecording, "TrackNumber"));
            RecordingDatesTextBox.SetBinding(TextBox.TextProperty, BindingBuilder.Build(_selectedRecording, "Dates"));
            RecordingLocationListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_selectedRecording.Locations, null, "Name"));

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

            var birthLocationQuery = _classicalMusicDbContext.Locations.Local.FirstOrDefault(l => l.Name == ComposerBirthLocationAutoCompleteBox.Text);

            if (birthLocationQuery == null)                                                                                                                                                                                // New location does not exist in database.
            {
                if (composer.BirthLocation != null && composer.BirthLocation.BirthLocationComposers.Count + composer.BirthLocation.DeathLocationComposers.Count + composer.BirthLocation.Recordings.Count == 1)            // Delete old location if only reference is gone.
                {
                    _classicalMusicDbContext.Locations.Remove(composer.BirthLocation);
                }

                if (string.IsNullOrEmpty(ComposerBirthLocationAutoCompleteBox.Text))
                {
                    composer.BirthLocation = null;
                }
                else
                {
                    var location = new Location();
                    location.Name = ComposerBirthLocationAutoCompleteBox.Text;

                    _classicalMusicDbContext.Locations.Add(location);
                    composer.BirthLocation = location;
                }
            }
            else                                                                                                                                                                                                           // New location does exist.
            {
                if (composer.BirthLocation != null && birthLocationQuery.Name != composer.BirthLocation.Name && composer.BirthLocation.BirthLocationComposers.Count + composer.BirthLocation.DeathLocationComposers.Count + composer.BirthLocation.Recordings.Count == 1) // Delete old location if only reference is gone.
                {
                    _classicalMusicDbContext.Locations.Remove(composer.BirthLocation);
                }

                composer.BirthLocation = birthLocationQuery;
            }
        }

        private void ComposerDeathLocationAutoCompleteBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var composer = _selectedComposers[0];

            var deathLocationQuery = _classicalMusicDbContext.Locations.Local.FirstOrDefault(l => l.Name == ComposerDeathLocationAutoCompleteBox.Text);

            if (deathLocationQuery == null)                                                                                                                                                                                // New location does not exist in database.
            {
                if (composer.DeathLocation != null && composer.DeathLocation.BirthLocationComposers.Count + composer.DeathLocation.DeathLocationComposers.Count + composer.DeathLocation.Recordings.Count == 1)            // Delete old location if only reference is gone.
                {
                    _classicalMusicDbContext.Locations.Remove(composer.DeathLocation);
                }

                if (string.IsNullOrEmpty(ComposerDeathLocationAutoCompleteBox.Text))
                {
                    composer.DeathLocation = null;
                }
                else
                {
                    var location = new Location();
                    location.Name = ComposerDeathLocationAutoCompleteBox.Text;

                    _classicalMusicDbContext.Locations.Add(location);
                    composer.DeathLocation = location;
                }
            }
            else                                                                                                                                                                                                           // New location does exist.
            {
                if (composer.DeathLocation != null && deathLocationQuery.Name != composer.DeathLocation.Name && composer.DeathLocation.BirthLocationComposers.Count + composer.DeathLocation.DeathLocationComposers.Count + composer.DeathLocation.Recordings.Count == 1) // Delete old location if only reference is gone.
                {
                    _classicalMusicDbContext.Locations.Remove(composer.DeathLocation);
                }

                composer.DeathLocation = deathLocationQuery;
            }
        }

        private void ComposerDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ComposerListBox.IsEnabled)
            {
                _classicalMusicDbContext.Composers.Local.Remove((Composer)ComposerListBox.SelectedItem);
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
                _classicalMusicDbContext.ComposerImages.Remove((ComposerImage)ComposerImageListBox.SelectedItem);
            }
        }

        private async void ComposerImageListBox_Drop(object sender, DragEventArgs e)
        {
            if (ComposerImageListBox.IsEnabled && e.Data.GetDataPresent(DataFormats.UnicodeText))
            {
                var imageBytes = await FileUtility.GetImageAsync((string)e.Data.GetData(DataFormats.UnicodeText));

                if (imageBytes == null)
                {
                    return;
                }

                var composerImage = new ComposerImage();
                composerImage.Composer = _selectedComposers[0];
                composerImage.Bytes = imageBytes;

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
                _classicalMusicDbContext.Links.Remove((Link)ComposerLinkListBox.SelectedItem);
            }
        }

        private void ComposerLinkListBox_Drop(object sender, DragEventArgs e)
        {
            if (ComposerLinkListBox.IsEnabled && e.Data.GetDataPresent(DataFormats.UnicodeText))
            {
                Dispatcher.BeginInvoke(new Action<string>(DropComposerLink), e.Data.GetData(DataFormats.UnicodeText));
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
                    composer.Name = composerName;

                    _classicalMusicDbContext.Composers.Add(composer);

                    _selectedComposers = new ObservableCollection<Composer> { composer };

                    RefreshComposerSection();

                    ComposerListBox.SelectedItem = composer;
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

        private void ComposerListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ComposerListBox.IsEnabled)
            {
                var frameworkElement = e.OriginalSource as FrameworkElement;

                if (frameworkElement == null || frameworkElement.TemplatedParent == null || !(frameworkElement.TemplatedParent is Thumb || frameworkElement.TemplatedParent is RepeatButton))
                {
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

        private void DropComposerLink(string url)
        {
            try
            {
                if (!url.StartsWith("http"))
                {
                    url = "http://" + url;
                }

                if (!FileUtility.WebsiteExists(url))
                {
                    return;
                }

                var link = new Link();
                link.Composers.Add(_selectedComposers[0]);
                link.Url = url;
                link.Name = UrlToTitleConverter.UrlToTitle(url);

                _selectedComposers[0].Links.Add(link);
                ComposerLinkListBox.SelectedItem = link;

                if (url.Contains("wikipedia"))
                {
                    _selectedComposers[0].Biography = BiographyUtility.CleanXaml(HtmlToXamlConverter.ConvertHtmlToXaml(WikipediaScraper.ScrapeArticle(url), false));
                    ComposerBiographyTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                }

                if (url.Contains("charles.smith"))
                {
                    ClassicalMusicNavigatorScraper.ScrapeComposer(url, _selectedComposers[0], _classicalMusicDbContext);
                }

                if (url.Contains("klassika"))
                {
                    var progressBar = new ProgressBar();
                    progressBar.Width = 500;
                    progressBar.Height = 20;
                    progressBar.Maximum = 1;
                    progressBar.Minimum = 0;
                    progressBar.Margin = new Thickness(15);

                    var progressDialog = new Window();
                    progressDialog.Content = progressBar;
                    progressDialog.ResizeMode = ResizeMode.NoResize;
                    progressDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    progressDialog.Owner = Application.Current.MainWindow;
                    progressDialog.ShowInTaskbar = false;
                    progressDialog.SizeToContent = SizeToContent.WidthAndHeight;
                    progressDialog.Title = "Downloading Compositions";

                    var progress = new Progress<double>();
                    progress.ProgressChanged += (o, p) =>
                    {
                        progressBar.Value = p;

                        if (p == 1d)
                        {
                            progressDialog.Close();
                        }
                    };

                    var cancellationTokenSource = new CancellationTokenSource();

                    _dataLoadingQueue.Enqueue(new Action(() => KlassikaScraper.ScrapeComposerDetailPage(url, _selectedComposers[0], _classicalMusicDbContext, progress, cancellationTokenSource.Token)));

                    progressDialog.ShowDialog();

                    cancellationTokenSource.Cancel();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString(), "MusicTimeline.log");
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
                .SelectMany(c => c.Catalogs)
                .Where(cc => cc.Prefix == (string)CompositionCollectionCatalogPrefixListBox.SelectedItem);

            var selectedCatalogNumbers = selectedCatalogs
                .SelectMany(cc => cc.CatalogNumbers)
                .Where(cn => cn.CompositionCollections.Contains(_selectedCompositionCollection));

            if (selectedCatalogNumbers.Count() == 0)
            {
                var catalogNumbers = new List<CatalogNumber>();

                foreach (var catalog in selectedCatalogs)
                {
                    var catalogNumber = new CatalogNumber();
                    catalogNumber.Catalog = catalog;
                    catalogNumber.CompositionCollections.Add(_selectedCompositionCollection);

                    _selectedCompositionCollection.CatalogNumbers.Add(catalogNumber);
                    catalog.CatalogNumbers.Add(catalogNumber);
                    catalogNumbers.Add(catalogNumber);
                }

                selectedCatalogNumbers = catalogNumbers;
            }

            foreach (var catalogNumber in selectedCatalogNumbers)
            {
                catalogNumber.Value = CompositionCollectionCatalogNumberTextBox.Text;
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
                var selectedCatalog = composer.Catalogs.FirstOrDefault(cc => cc.Prefix == (string)CompositionCollectionCatalogPrefixListBox.SelectedItem);

                if (selectedCatalog != null)
                {
                    composer.Catalogs.Remove(selectedCatalog);
                }
            }
        }

        private void CompositionCollectionCatalogPrefixListBox_Drop(object sender, DragEventArgs e)
        {
            if (!CompositionCollectionCatalogPrefixListBox.IsEnabled)
            {
                return;
            }

            var compositionCatalogPrefix = (string)e.Data.GetData(DataFormats.UnicodeText);

            if (compositionCatalogPrefix == null)
            {
                return;
            }

            var newCatalogs = new Catalog[_selectedComposers.Count];

            for (int i = 0; i < _selectedComposers.Count; i++)
            {
                var compositionCatalog = new Catalog();
                compositionCatalog.Prefix = compositionCatalogPrefix;
                compositionCatalog.Composer = _selectedComposers[i];

                _selectedComposers[i].Catalogs.Add(compositionCatalog);
                newCatalogs[i] = compositionCatalog;
            }

            var availableCatalogs = _selectedComposers
                .SelectMany(c => c.Catalogs)
                .Select(cc => cc.Prefix)
                .Distinct();

            CompositionCollectionCatalogPrefixListBox.ItemsSource = availableCatalogs.ToList();
            CompositionCollectionCatalogPrefixListBox.SelectedItem = compositionCatalogPrefix;

            CompositionCollectionCatalogNumberTextBox.Text = null;
        }

        private void CompositionCollectionCatalogPrefixListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
            {
                return;
            }

            CompositionCollectionCatalogNumberTextBox.Text = _selectedCompositionCollection.CatalogNumbers.FirstOrDefault(cn => cn.Catalog == CompositionCollectionCatalogPrefixListBox.SelectedItem)?.Value;
        }

        private void CompositionCollectionDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void CompositionCollectionListBox_Drop(object sender, DragEventArgs e)
        {
            if (CompositionCollectionListBox.IsEnabled)
            {
                var compositionCollectionName = (string)e.Data.GetData(DataFormats.UnicodeText);

                if (compositionCollectionName == null)
                {
                    return;
                }

                _selectedCompositionCollection = new CompositionCollection();
                _selectedCompositionCollection.Name = compositionCollectionName;
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

        private void CatalogPrefixDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
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
                var selectedCatalog = composer.Catalogs.FirstOrDefault(cc => cc.Prefix == (string)CompositionCatalogPrefixListBox.SelectedItem);

                if (selectedCatalog != null)
                {
                    composer.Catalogs.Remove(selectedCatalog);
                }
            }
        }

        private void CompositionCatalogNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!CompositionCatalogNumberTextBox.IsEnabled)
            {
                return;
            }

            var selectedCatalogs = _selectedComposers
                .SelectMany(c => c.Catalogs)
                .Where(cc => cc.Prefix == (string)CompositionCatalogPrefixListBox.SelectedItem);

            var selectedCatalogNumbers = selectedCatalogs
                .SelectMany(cc => cc.CatalogNumbers)
                .Where(cn => cn.Compositions.Contains(_selectedComposition));

            if (selectedCatalogNumbers.Count() == 0)
            {
                var catalogNumbers = new List<CatalogNumber>();

                foreach (var catalog in selectedCatalogs)
                {
                    var catalogNumber = new CatalogNumber();
                    catalogNumber.Catalog = catalog;
                    catalogNumber.Compositions.Add(_selectedComposition);

                    _selectedComposition.CatalogNumbers.Add(catalogNumber);
                    catalog.CatalogNumbers.Add(catalogNumber);
                    catalogNumbers.Add(catalogNumber);
                }

                selectedCatalogNumbers = catalogNumbers;
            }

            foreach (var catalogNumber in selectedCatalogNumbers)
            {
                catalogNumber.Value = CompositionCatalogNumberTextBox.Text;
            }
        }

        private void CompositionCatalogPrefixListBox_Drop(object sender, DragEventArgs e)
        {
            if (!CompositionCatalogPrefixListBox.IsEnabled)
            {
                return;
            }

            var compositionCatalogPrefix = (string)e.Data.GetData(DataFormats.UnicodeText);

            if (compositionCatalogPrefix == null)
            {
                return;
            }

            var catalogs = new Catalog[_selectedComposers.Count];

            for (int i = 0; i < _selectedComposers.Count; i++)
            {
                var catalog = new Catalog();
                catalog.Prefix = compositionCatalogPrefix;
                catalog.Composer = _selectedComposers[i];

                _selectedComposers[i].Catalogs.Add(catalog);
                catalogs[i] = catalog;
            }

            var availableCatalogs = _selectedComposers
                .SelectMany(c => c.Catalogs)
                .Select(cc => cc.Prefix)
                .Distinct();

            CompositionCatalogPrefixListBox.ItemsSource = availableCatalogs.ToList();
            CompositionCatalogPrefixListBox.SelectedItem = compositionCatalogPrefix;

            CompositionCatalogNumberTextBox.Text = null;
        }

        private void CompositionCatalogPrefixListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
            {
                return;
            }

            CompositionCatalogNumberTextBox.Text = _selectedComposition.CatalogNumbers.FirstOrDefault(cn => cn.Catalog == CompositionCatalogPrefixListBox.SelectedItem)?.Value;
        }

        private void CompositionDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (CompositionListBox.IsEnabled)
            {
                _classicalMusicDbContext.Compositions.Remove(_selectedComposition);
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
                    var _selectedComposition = new Composition();
                    _selectedComposition.Name = compositionName;
                    _selectedComposition.Composers = _selectedComposers;

                    _classicalMusicDbContext.Compositions.Add(_selectedComposition);
                }

                _selectedComposition.CompositionCollection = _selectedCompositionCollection;
                _selectedCompositionCollection.Compositions.Add(_selectedComposition);

                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_selectedCompositionCollection.Compositions, null, "Name"));
            }
            else
            {
                _selectedComposition = _selectedComposers.Common(c => c.Compositions).FirstOrDefault(c => c.Name == compositionName);

                if (_selectedComposition == null)
                {
                    var _selectedComposition = new Composition();
                    _selectedComposition.Name = compositionName;
                    _selectedComposition.Composers = _selectedComposers;

                    _classicalMusicDbContext.Compositions.Add(_selectedComposition);
                }

                foreach (var composer in _selectedComposers)
                {
                    composer.Compositions.Add(_selectedComposition);
                }

                CompositionListBox.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(_selectedComposers.Common(c => c.Compositions), null, "Name"));
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

        private void MovementDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MovementListBox.IsEnabled)
            {
                _classicalMusicDbContext.Movements.Remove((Movement)MovementListBox.SelectedItem);
            }
        }

        private void MovementListBox_Drop(object sender, DragEventArgs e)
        {
            if (MovementListBox.IsEnabled)
            {
                var movementName = (string)e.Data.GetData(DataFormats.UnicodeText);

                if (movementName != null)
                {
                    _selectedMovement = new Movement();
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

        #endregion Movement Section Events

        #region Recording Section Events

        private void RecordingAlbumAutoCompleteBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var recordingAlbum = _classicalMusicDbContext.Albums.Local.FirstOrDefault(l => l.Name == RecordingAlbumAutoCompleteBox.Text);

            if (recordingAlbum == null)
            {
                if (_selectedRecording.Album != null && _selectedRecording.Album.Recordings.Count == 1)
                {
                    _classicalMusicDbContext.Albums.Remove(_selectedRecording.Album);
                }

                if (string.IsNullOrEmpty(RecordingAlbumAutoCompleteBox.Text))
                {
                    _selectedRecording.Album = null;
                }
                else
                {
                    var album = new Album();
                    album.Name = RecordingAlbumAutoCompleteBox.Text;
                    album.Recordings.Add(_selectedRecording);

                    _selectedRecording.Album = album;
                }
            }
            else
            {
                if (_selectedRecording.Album != null && recordingAlbum.Name != _selectedRecording.Album.Name && _selectedRecording.Album.Recordings.Count == 1)
                {
                    _classicalMusicDbContext.Albums.Remove(_selectedRecording.Album);
                }

                _selectedRecording.Album = recordingAlbum;
            }
        }

        private void RecordingDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (RecordingListBox.IsEnabled)
            {
                _classicalMusicDbContext.Recordings.Local.Remove((Recording)RecordingListBox.SelectedItem);
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

            _selectedRecording = new Recording();

            if (_selectedMovement != null)
            {
                _selectedRecording.Movements.Add(_selectedMovement);
                _selectedMovement.Recordings.Add(_selectedRecording);
                RecordingListBox.ItemsSource = new List<Recording>(_selectedMovement.Recordings);
            }
            else if (_selectedComposition != null)
            {
                _selectedRecording.Compositions.Add(_selectedComposition);
                _selectedComposition.Recordings.Add(_selectedRecording);
                RecordingListBox.ItemsSource = new List<Recording>(_selectedComposition.Recordings);
            }
            else if (_selectedCompositionCollection != null)
            {
                _selectedRecording.CompositionCollections.Add(_selectedCompositionCollection);
                _selectedCompositionCollection.Recordings.Add(_selectedRecording);
                RecordingListBox.ItemsSource = new List<Recording>(_selectedCompositionCollection.Recordings);
            }

            RecordingListBox.SelectedItem = _selectedRecording;

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

            importPathBuilder.Append(_selectedRecording.Id).Append(Path.GetExtension(recordingPath));

            var importPath = importPathBuilder.ToString();

            Directory.CreateDirectory(Path.GetDirectoryName(importPath));

            if (!File.Exists(importPath))
            {
                File.Copy(recordingPath, importPath);
            }

            LibraryDB.DataProvider.Add(_selectedRecording.Id, importPath);
        }

        private void RecordingListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecordingListBox.IsEnabled)
            {
                _selectedRecording = (Recording)RecordingListBox.SelectedItem;

                RefreshRecordingSection();
            }
        }

        private void RecordingLocationDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (RecordingLocationListBox.IsEnabled)
            {
                var location = (Location)RecordingLocationListBox.SelectedItem;

                location.Recordings.Remove(_selectedRecording);

                if (location.Recordings.Count == 0 && location.BirthLocationComposers.Count == 0 && location.DeathLocationComposers.Count == 0)
                {
                    _classicalMusicDbContext.Locations.Local.Remove(location);
                }
            }
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

            var location = _classicalMusicDbContext.Locations.FirstOrDefault(l => l.Name == locationName);

            if (location == null)
            {
                location = new Location();
                location.Name = locationName;

                _classicalMusicDbContext.Locations.Local.Add(location);
            }

            location.Recordings.Add(_selectedRecording);
            _selectedRecording.Locations.Add(location);
        }

        private void RecordingPerformerDeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (RecordingPerformerListBox.IsEnabled)
            {
                var performer = (Performer)RecordingPerformerListBox.SelectedItem;

                performer.Recordings.Remove(_selectedRecording);

                if (performer.Recordings.Count == 0)
                {
                    _classicalMusicDbContext.Performers.Remove(performer);
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

            var performer = _classicalMusicDbContext.Performers.FirstOrDefault(p => p.Name == performerName);

            if (performer == null)
            {
                performer = new Performer();
                performer.Name = performerName;

                _classicalMusicDbContext.Performers.Local.Add(performer);
            }

            performer.Recordings.Add(_selectedRecording);
            _selectedRecording.Performers.Add(performer);

            RecordingPerformerListBox.SelectedItem = performer;
        }

        #endregion Recording Section Events

        #region Status and Button Section Events

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _classicalMusicDbContext.Dispose();

            NavigationService.GoBack();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            using (_classicalMusicDbContext)
            {
                try
                {
                    _classicalMusicDbContext.SaveChanges();
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
                return _classicalMusicDbContext.Recordings.Local.First(r => r.Id == recordingID);
            }

            return null;
        }

        #region IDisposable Support

        private bool _isDisposed = false;

        ~InputPage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // dispose managed objects
                }

                if (_classicalMusicDbContext != null)
                {
                    _classicalMusicDbContext.Dispose();
                }

                _isDisposed = true;
            }
        }

        #endregion IDisposable Support
    }
}