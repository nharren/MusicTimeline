using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class VisualTreeUtility
    {
        public static T FindAncestor<T>(DependencyObject obj) where T : DependencyObject
        {
            var parentDependencyObject = VisualTreeHelper.GetParent(obj);

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
                return FindAncestor<T>(parentDependencyObject);
            }
        }

        public static T FindDescendant<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindDescendant<T>(child);

                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }

            return null;
        }

        public static IEnumerable<T> FindDescendants<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is T)
                {
                    yield return (T)child;
                }

                foreach (var descendant in FindDescendants<T>(child))
                {
                    yield return descendant;
                }
            }
        }
    }

}