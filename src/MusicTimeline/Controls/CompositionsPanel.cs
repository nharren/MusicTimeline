using NathanHarrenstein.MusicTimeline.Data;
using System;
using System.Collections.Generic;
using System.EDTF;
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
    public class CompositionsPanel : Control
    {
        static CompositionsPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CompositionsPanel), new FrameworkPropertyMetadata(typeof(CompositionsPanel)));
        }

        public IEnumerable<Composition> Compositions
        {
            get
            {
                return (IEnumerable<Composition>)GetValue(CompositionsProperty);
            }

            set
            {
                SetValue(CompositionsProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            compositionStackPanel = (StackPanel)Template.FindName("PART_CompositionStackPanel", this);
        }

        private void Build()
        {
            if (compositionStackPanel == null || Compositions == null)
            {
                return;
            }

            Clear();

            var undatedCompositions = new List<Composition>();
            var datedCompositions = new List<Composition>();

            foreach (var composition in Compositions)
            {
                if (string.IsNullOrEmpty(composition.Dates))
                {
                    undatedCompositions.Add(composition);
                }
                else
                {
                    datedCompositions.Add(composition);
                }
            }

            var compositionYearGroups = datedCompositions
                .OrderBy(c => c.Name)
                .GroupBy(c => ExtendedDateTimeFormatParser.Parse(c.Dates).Earliest().Year)
                .OrderBy(c => c.Key);

            foreach (var compositionYearGroup in compositionYearGroups)
            {
                var compositionYear = compositionYearGroup.Key;

                BuildYearHeader(compositionYear.ToString());
                BuildCompositionItems(compositionYearGroup.AsEnumerable());
            }

            BuildYearHeader("Undated");
            BuildCompositionItems(undatedCompositions);
        }

        private void BuildCompositionItems(IEnumerable<Composition> compositions)
        {
            var isInitial = true;

            foreach (var composition in compositions)
            {
                var compositionItemTextBlock = new TextBlock();
                compositionItemTextBlock.FontFamily = new FontFamily("Cambria");
                compositionItemTextBlock.FontSize = 17.333;
                compositionItemTextBlock.Foreground = (SolidColorBrush)App.Current.Resources["ForegroundBrush"];
                compositionItemTextBlock.Text = BuildCompositionItemName(composition);

                if (!isInitial)
                {
                    compositionItemTextBlock.Margin = new Thickness(0, 5, 0, 0);
                }
                else
                {
                    compositionItemTextBlock.Margin = new Thickness(0, 2, 0, 0);
                }

                compositionStackPanel.Children.Add(compositionItemTextBlock);

                isInitial = false;
            }
        }

        private string BuildCompositionItemName(Composition composition)
        {
            var stringBuilder = new StringBuilder(composition.Name);

            if (composition.Key != null)
            {
                stringBuilder.Append(" in ");
                stringBuilder.Append(composition.Key.Name);
            }

            if (composition.CatalogNumbers.Count > 0)
            {
                var firstCatalogNumber = composition.CatalogNumbers.First();

                stringBuilder.Append(", ");
                stringBuilder.Append(firstCatalogNumber.Catalog.Prefix);
                stringBuilder.Append(" ");
                stringBuilder.Append(firstCatalogNumber.Value);
            }

            return stringBuilder.ToString();
        }

        private void BuildYearHeader(string header)
        {
            var yearHeaderTextBlock = new TextBlock();
            yearHeaderTextBlock.FontFamily = new FontFamily("Cambria");
            yearHeaderTextBlock.FontSize = 24;
            yearHeaderTextBlock.Foreground = (SolidColorBrush)App.Current.Resources["HeaderBrush"];
            yearHeaderTextBlock.Text = header;
            yearHeaderTextBlock.Margin = new Thickness(0, 10, 0, 0);

            compositionStackPanel.Children.Add(yearHeaderTextBlock);
        }

        private void Clear()
        {
            if (compositionStackPanel == null)
            {
                return;
            }

            compositionStackPanel.Children.Clear();
        }

        public static readonly DependencyProperty CompositionsProperty = DependencyProperty.Register("Compositions", typeof(IEnumerable<Composition>), typeof(CompositionsPanel), new PropertyMetadata(null, CompositionsChanged));
        private StackPanel compositionStackPanel;

        private static void CompositionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var compositionPanel = (CompositionsPanel)d;

            compositionPanel.Build();
        }
    }
}
