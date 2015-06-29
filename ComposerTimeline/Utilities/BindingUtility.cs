using System.Windows.Data;

namespace NathanHarrenstein.ComposerTimeline
{
    internal static class BindingUtility
    {
        internal static Binding Create(object source, string path, IValueConverter converter = null)
        {
            return new Binding(path)
            {
                Source = source,
                Converter = converter
            };
        }
    }
}