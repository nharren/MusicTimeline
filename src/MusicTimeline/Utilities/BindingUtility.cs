using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    internal static class BindingUtility
    {
        internal static Binding Create(object source)
        {
            var binding = new Binding();
            binding.Source = source;

            return binding;
        }

        internal static Binding Create(object source, string path, IValueConverter converter = null)
        {
            var binding = new Binding();
            binding.Source = source;
            binding.Path = new PropertyPath(path);
            binding.Converter = converter;

            return binding;
        }

        internal static Binding Create(object source, string path, string sortPropertyName, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            var sortDescription = new SortDescription(sortPropertyName, sortDirection);

            var collectionViewSource = new CollectionViewSource();
            collectionViewSource.Source = source;
            collectionViewSource.SortDescriptions.Add(sortDescription);

            var binding = new Binding();
            binding.Source = collectionViewSource;
            binding.Path = new PropertyPath(path);

            return binding;
        }
    }
}