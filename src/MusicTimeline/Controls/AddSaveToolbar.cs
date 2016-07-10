using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class AddSaveToolbar : SaveToolbar
    {
        private Button addButton;
        private Button removeButton;

        static AddSaveToolbar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AddSaveToolbar), new FrameworkPropertyMetadata(typeof(AddSaveToolbar)));
        }

        public event EventHandler Adding;
        public event EventHandler Removing;

        public override void OnApplyTemplate()
        {
            addButton = (Button)Template.FindName("addButton", this);
            removeButton = (Button)Template.FindName("removeButton", this);

            addButton.Click += addButton_Click;
            removeButton.Click += removeButton_Click;

            base.OnApplyTemplate();
        }

        protected virtual void OnRemoving(EventArgs e)
        {
            if (Removing != null)
            {
                Removing(this, e);
            }
        }

        protected virtual void OnAdding(EventArgs e)
        {
            if (Adding != null)
            {
                Adding(this, e);
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            OnRemoving(EventArgs.Empty);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            OnAdding(EventArgs.Empty);
        }
    }
}
