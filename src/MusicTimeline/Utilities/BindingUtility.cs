using System.ComponentModel;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    internal static class BindingUtility
    {
        internal static Binding Create(object source)
        {
            return new Binding()
            {
                Source = source
            };
        }

        internal static Binding Create(object source, string path, IValueConverter converter = null)
        {
            return new Binding(path)
            {
                Source = source,
                Converter = converter
            };
        }

        internal static Binding Create(object source, string path, string sortPropertyName, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            var collectionViewSource = new CollectionViewSource();
            collectionViewSource.Source = source;
            collectionViewSource.SortDescriptions.Add(new SortDescription(sortPropertyName, sortDirection));

            return new Binding(path)
            {
                Source = collectionViewSource
            };
        }
    }
}