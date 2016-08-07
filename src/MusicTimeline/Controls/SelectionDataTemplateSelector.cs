using NathanHarrenstein.MusicTimeline.Utilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class SelectionDataTemplateSelector : DataTemplateSelector
    {
        private Selector selector;

        public DataTemplate SelectedTemplate { get; set; }
        public DataTemplate UnselectedTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (selector == null)
            {
                selector = VisualTreeUtility.FindAncestor<Selector>(container);
            }

            if (item == selector.SelectedItem)
            {
                return SelectedTemplate;
            }
            else
            {
                return UnselectedTemplate;
            }
        }
    }
}