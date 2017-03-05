using NathanHarrenstein.MusicTimeline.Data;
using System.Collections.Generic;
using System.EDTF;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class CompositionsPanel : Control
    {
        public static readonly DependencyProperty CompositionItemStyleProperty = DependencyProperty.Register("CompositionItemStyle", typeof(Style), typeof(CompositionsPanel), new PropertyMetadata(null, new PropertyChangedCallback(CompositionItemStyle_Changed)));
        public static readonly DependencyProperty CompositionsProperty = DependencyProperty.Register("Compositions", typeof(IEnumerable<Composition>), typeof(CompositionsPanel), new PropertyMetadata(null, CompositionsChanged));
        public static readonly DependencyProperty YearHeaderStyleProperty = DependencyProperty.Register("YearHeaderStyle", typeof(Style), typeof(CompositionsPanel), new PropertyMetadata(null, new PropertyChangedCallback(YearHeaderStyle_Changed)));

        private StackPanel stackPanel;

        static CompositionsPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CompositionsPanel), new FrameworkPropertyMetadata(typeof(CompositionsPanel)));
        }

        public Style CompositionItemStyle
        {
            get
            {
                return (Style)GetValue(CompositionItemStyleProperty);
            }

            set
            {
                SetValue(CompositionItemStyleProperty, value);
            }
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

        public Style YearHeaderStyle
        {
            get
            {
                return (Style)GetValue(YearHeaderStyleProperty);
            }

            set
            {
                SetValue(YearHeaderStyleProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            stackPanel = GetTemplateChild("stackPanel") as StackPanel;
        }

        private static void CompositionItemStyle_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var compositionsPanel = (CompositionsPanel)d;
            compositionsPanel.UpdateData();
        }

        private static void CompositionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var compositionPanel = (CompositionsPanel)d;
            compositionPanel.UpdateData();
        }

        private static void YearHeaderStyle_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var compositionsPanel = (CompositionsPanel)d;
            compositionsPanel.UpdateData();
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

        private void BuildCompositionItems(IEnumerable<Composition> compositions)
        {
            foreach (var composition in compositions)
            {
                var compositionItemTextBox = new TextBox();
                compositionItemTextBox.Style = CompositionItemStyle;
                compositionItemTextBox.Text = BuildCompositionItemName(composition);

                stackPanel.Children.Add(compositionItemTextBox);
            }
        }

        private void BuildYearHeader(string header)
        {
            var yearHeaderTextBlock = new TextBlock();
            yearHeaderTextBlock.Style = YearHeaderStyle;
            yearHeaderTextBlock.Text = header;

            stackPanel.Children.Add(yearHeaderTextBlock);
        }

        public void UpdateData()
        {
            if (stackPanel == null || Compositions == null)
            {
                return;
            }

            stackPanel.Children.Clear();

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
    }
}