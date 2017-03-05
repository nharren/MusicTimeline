using NathanHarrenstein.MusicTimeline.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.EDTF;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class CompositionsEditPanel : Control
    {
        public static readonly DependencyProperty ComposerProperty = DependencyProperty.Register("Composer", typeof(Composer), typeof(CompositionsEditPanel), new PropertyMetadata());

        private List<Composition> addedCompositions;
        private ObservableCollection<Composition> compositionSource;
        private ListBox listBox;
        private List<Composition> removedCompositions;
        private AddSaveToolbar toolBar;

        static CompositionsEditPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CompositionsEditPanel), new FrameworkPropertyMetadata(typeof(CompositionsEditPanel)));
        }

        public CompositionsEditPanel()
        {
            addedCompositions = new List<Composition>();
            removedCompositions = new List<Composition>();

            IsVisibleChanged += compositionsEditPanel_IsVisibleChanged;
        }

        public event EventHandler Cancelling;
        public event EventHandler Saving;

        public Composer Composer
        {
            get
            {
                return (Composer)GetValue(ComposerProperty);
            }

            set
            {
                SetValue(ComposerProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            listBox = GetTemplateChild("listBox") as ListBox;
            toolBar = GetTemplateChild("toolBar") as AddSaveToolbar;

            if (toolBar != null)
            {
                toolBar.Adding += toolBar_Adding;
                toolBar.Removing += toolBar_Removing;
                toolBar.Saving += toolBar_Saving;
                toolBar.Cancelling += toolBar_Cancelling;
            }

            UpdateData();
        }

        protected virtual void OnCancelling(EventArgs e)
        {
            Cancelling?.Invoke(this, e);
        }

        protected virtual void OnSaving(EventArgs e)
        {
            Saving?.Invoke(this, e);
        }

        private void compositionsEditPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                UpdateData();
            }
            else
            {
                addedCompositions.Clear();
                removedCompositions.Clear();
                compositionSource.Clear();
            }
        }

        private void toolBar_Adding(object sender, EventArgs e)
        {
            var composition = new Composition();
            composition.Name = "[Name]";
            composition.Dates = "[Dates]";

            addedCompositions.Add(composition);
            compositionSource.Add(composition);

            listBox.SelectedItem = composition;

            listBox.ScrollIntoView(composition);
        }

        private void toolBar_Cancelling(object sender, EventArgs e)
        {
            Visibility = Visibility.Collapsed;

            OnCancelling(e);
        }

        private void toolBar_Removing(object sender, EventArgs e)
        {
            var composition = (Composition)listBox.SelectedItem;
            compositionSource.Remove(composition);

            foreach (var addedComposition in addedCompositions)
            {
                if (addedComposition == composition)
                {
                    addedCompositions.Remove(composition);

                    break;
                }
            }
        }

        private void toolBar_Saving(object sender, EventArgs e)
        {
            foreach (var composition in addedCompositions)
            {
                if (!ValidateComposition(composition))
                {
                    return;
                }
            }

            foreach (var composition in addedCompositions)
            {
                Composer.Compositions.Add(composition);
            }

            foreach (var composition in removedCompositions)
            {
                App.ClassicalMusicContext.DeleteObject(composition);
                Composer.Compositions.Remove(composition);
            }

            try
            {
                App.ClassicalMusicContext.SaveChanges();
                App.ClassicalMusicContext.LoadProperty(Composer, "Compositions");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Changes were not saved: {ex.Message}");
            }

            Visibility = Visibility.Collapsed;

            OnSaving(e);
        }

        private void UpdateData()
        {
            if (Composer == null || listBox == null)
            {
                return;
            }

            var sortedCompositions = Composer.Compositions
                .AsEnumerable()
                .OrderBy(c => c.Name);

            compositionSource = new ObservableCollection<Composition>(sortedCompositions);
            listBox.ItemsSource = compositionSource;
        }

        private bool ValidateComposition(Composition composition)
        {
            if (string.IsNullOrWhiteSpace(composition.Name))
            {
                MessageBox.Show("Name fields cannot be null or whitespace.");

                return false;
            }

            try
            {
                ExtendedDateTimeInterval.Parse(composition.Dates);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return false;
            }

            return true;
        }
    }
}