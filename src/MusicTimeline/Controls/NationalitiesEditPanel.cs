using NathanHarrenstein.MusicTimeline.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class NationalitiesEditPanel : Control
    {
        public static readonly DependencyProperty ComposerProperty = DependencyProperty.Register("Composer", typeof(Composer), typeof(NationalitiesEditPanel), new PropertyMetadata());

        private Dictionary<Nationality, int> changeTracker;
        private ListBox listBox;
        private SaveToolbar toolbar;

        static NationalitiesEditPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NationalitiesEditPanel), new FrameworkPropertyMetadata(typeof(NationalitiesEditPanel)));
        }

        public NationalitiesEditPanel()
        {
            IsVisibleChanged += nationalitiesEditPanel_IsVisibleChanged;
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

            UpdateData();

            toolbar = GetTemplateChild("toolbar") as SaveToolbar;

            if (toolbar != null)
            {
                toolbar.Saving += toolbar_Saving;
                toolbar.Cancelling += toolbar_Cancelling;
            }
        }

        public void UpdateData()
        {
            if (listBox != null && Composer != null)
            {
                // The change tracker works as follows:
                // If the composer had the nationality at the start of the session, that nationality will get +1.
                // If the composer has the nationality at the end of the sesson (on save), that nationality will get +2.
                // If a nationality is 1 by the end of the session, it means the nationality was removed from the Composer.
                // If a nationality is 2 by the end of the session, it means the nationality was added to the Composer.
                // The other values, 0 and 3, represent unchanged states and are not acted upon.

                changeTracker = App.ClassicalMusicContext.Nationalities.ToDictionary(n => n, n => 0);

                foreach (var nationality in Composer.Nationalities)
                {
                    changeTracker[nationality]++;
                }

                var nationalityViewModels = CreateNationalityViewModels().ToList();
                listBox.ItemsSource = nationalityViewModels;

                foreach (var nationalityViewModel in nationalityViewModels)
                {
                    if (Composer.Nationalities.Contains(nationalityViewModel.Nationality))
                    {
                        listBox.SelectedItems.Add(nationalityViewModel);
                    }
                }
            }
        }

        protected virtual void OnCancelling(EventArgs e)
        {
            Cancelling?.Invoke(this, e);
        }

        protected virtual void OnSaving(EventArgs e)
        {
            Saving?.Invoke(this, e);
        }

        private IEnumerable<NationalityViewModel> CreateNationalityViewModels()
        {
            var nationalities = App.ClassicalMusicContext.Nationalities.OrderBy(n => n.Name);

            foreach (var nationality in nationalities)
            {
                var uri = new Uri($@"pack://application:,,,/Resources/Flags/16/{nationality.Name}.png", UriKind.Absolute);
                var bitmapImage = new BitmapImage(uri);

                yield return new NationalityViewModel(bitmapImage, nationality);
            }
        }

        private void nationalitiesEditPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                UpdateData();
            }
        }

        private void toolbar_Cancelling(object sender, EventArgs e)
        {
            Visibility = Visibility.Collapsed;

            OnSaving(e);
        }

        private void toolbar_Saving(object sender, EventArgs e)
        {
            foreach (NationalityViewModel nationalityViewModel in listBox.SelectedItems)
            {
                changeTracker[nationalityViewModel.Nationality] += 2;
            }

            foreach (var item in changeTracker)
            {
                if (item.Value == 1)
                {
                    App.ClassicalMusicContext.DeleteLink(Composer, "Nationalities", item.Key);
                    Composer.Nationalities.Remove(item.Key);
                }
                else if (item.Value == 2)
                {
                    Composer.Nationalities.Add(item.Key);
                }
            }

            try
            {
                App.ClassicalMusicContext.SaveChanges();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                MessageBox.Show($"Could not save changes: {ex.Message}");

                return;
            }

            Visibility = Visibility.Collapsed;

            OnSaving(e);
        }

        private class NationalityViewModel
        {
            public NationalityViewModel(BitmapImage flag, Nationality nationality)
            {
                Flag = flag;
                Nationality = nationality;
            }

            public BitmapImage Flag { get; private set; }
            public Nationality Nationality { get; private set; }
        }
    }
}