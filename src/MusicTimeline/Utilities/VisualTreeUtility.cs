using System.Windows;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class VisualTreeUtility
    {
        public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parentDependencyObject = VisualTreeHelper.GetParent(child);

            if (parentDependencyObject == null)
            {
                return null;
            }

            T parentT = parentDependencyObject as T;

            if (parentT != null)
            {
                return parentT;
            }
            else
            {
                return FindVisualParent<T>(parentDependencyObject);
            }
        }
    }
}