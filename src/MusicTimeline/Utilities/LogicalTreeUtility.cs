using System.Windows;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class LogicalTreeUtility
    {
        public static T FindLogicalParent<T>(DependencyObject obj) where T : class
        {
            while (obj != null)
            {
                T element = obj as T;

                if (element != null)
                {
                    return element;
                }

                obj = LogicalTreeHelper.GetParent(obj);
            }

            return default(T);
        }
    }
}