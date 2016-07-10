using System;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class SaveToolbar : Control
    {
        private Button cancelButton;
        private Button saveButton;

        static SaveToolbar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SaveToolbar), new FrameworkPropertyMetadata(typeof(SaveToolbar)));
        }

        public event EventHandler Cancelling;

        public event EventHandler Saving;

        public override void OnApplyTemplate()
        {
            saveButton = (Button)Template.FindName("PART_SaveButton", this);
            cancelButton = (Button)Template.FindName("PART_CancelButton", this);

            saveButton.Click += SaveButton_Click;
            cancelButton.Click += CancelButton_Click;
        }

        protected virtual void OnCancelling(EventArgs e)
        {
            if (Cancelling != null)
            {
                Cancelling(this, e);
            }
        }

        protected virtual void OnSaving(EventArgs e)
        {
            if (Saving != null)
            {
                Saving(this, e);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnCancelling(EventArgs.Empty);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            OnSaving(EventArgs.Empty);
        }
    }
}