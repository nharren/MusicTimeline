using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.EDTF;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Timeline
{
    public class EraPanel : Panel, IScroll
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(EraPanel));
        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(IReadOnlyList<ITimelineEra>), typeof(EraPanel), new PropertyMetadata(new PropertyChangedCallback(EraPanel_ErasChanged)));
        public static readonly DependencyProperty EraTemplatesProperty = DependencyProperty.Register("EraTemplates", typeof(List<DataTemplate>), typeof(EraPanel));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeResolution), typeof(EraPanel));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(EraPanel));

        private FrameworkElement[] _cache;
        private bool _hasViewChanged = true;
        private double _horizontalOffset;
        private List<int> _previouslyVisibleCacheIndexes = new List<int>();
        private List<int> _visibleCacheIndexes = new List<int>();

        public EraPanel()
        {
            ClipToBounds = true;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                _hasViewChanged = false;
            }
        }

        public ExtendedDateTimeInterval Dates
        {
            get
            {
                return (ExtendedDateTimeInterval)GetValue(DatesProperty);
            }
            set
            {
                SetValue(DatesProperty, value);
            }
        }

        public IReadOnlyList<ITimelineEra> Eras
        {
            get
            {
                return (IReadOnlyList<ITimelineEra>)GetValue(ErasProperty);
            }
            set
            {
                SetValue(ErasProperty, value);
            }
        }

        public List<DataTemplate> EraTemplates
        {
            get
            {
                return (List<DataTemplate>)GetValue(EraTemplatesProperty);
            }
            set
            {
                SetValue(EraTemplatesProperty, value);
            }
        }

        public double HorizontalOffset
        {
            get
            {
                return _horizontalOffset;
            }
            set
            {
                _horizontalOffset = value;
            }
        }

        public TimeResolution Resolution
        {
            get
            {
                return (TimeResolution)GetValue(ResolutionProperty);
            }
            set
            {
                SetValue(ResolutionProperty, value);
            }
        }

        public TimeRuler Ruler
        {
            get
            {
                return (TimeRuler)GetValue(RulerProperty);
            }
            set
            {
                SetValue(RulerProperty, value);
            }
        }

        public Vector ReviseScrollingDisplacement(Vector delta)
        {
            if (Ruler == null)
            {
                return delta;
            }

            var extentWidth = Ruler.ToPixels(Dates);

            if (HorizontalOffset + delta.X < 0)                                                   // Panning too far left. Set horizontal pan distance to the amount of space remaining before the left edge.
            {
                delta.X = -HorizontalOffset;
            }

            if (HorizontalOffset + delta.X > extentWidth - ActualWidth)                           // Panning too far right. Set horizontal pan distance to the amount of space remaining before the right edge.
            {
                delta.X = extentWidth - ActualWidth - HorizontalOffset;
            }

            return delta;
        }

        public void Scroll(Vector delta)
        {
            HorizontalOffset += delta.X;

            _hasViewChanged = true;
            _previouslyVisibleCacheIndexes = new List<int>(_visibleCacheIndexes);

            _visibleCacheIndexes.Clear();
            Children.Clear();
            InvalidateMeasure();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var previouslyVisibleCacheIndex in _previouslyVisibleCacheIndexes)
            {
                _cache[previouslyVisibleCacheIndex].Arrange(new Rect());
            }

            foreach (var visibleCacheIndex in _visibleCacheIndexes)
            {
                var dates = Eras[visibleCacheIndex].Dates;

                _cache[visibleCacheIndex].Arrange(new Rect(
                    Ruler.ToPixels(Dates.Earliest() + Ruler.ToTimeSpan(HorizontalOffset), dates.Earliest()),
                    0,
                    Ruler.ToPixels(dates),
                    finalSize.Height));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_hasViewChanged && Dates != null && Eras != null && Ruler != null && availableSize != null)
            {
                if (_cache == null)
                {
                    _cache = new FrameworkElement[Eras.Count];
                }

                var viewportLeftTime = Dates.Earliest() + Ruler.ToTimeSpan(HorizontalOffset);
                var viewportRightTime = viewportLeftTime + Ruler.ToTimeSpan(availableSize.Width);

                for (var i = 0; i < Eras.Count; i++)
                {
                    var timelineEra = Eras[i];

                    if (timelineEra.Dates.End.Latest() > viewportLeftTime && timelineEra.Dates.Start.Earliest() < viewportRightTime)
                    {
                        if (_cache[i] == null)
                        {
                            var eraType = timelineEra.GetType();

                            var template = EraTemplates.FirstOrDefault(et => ((Type)et.DataType) == eraType);

                            if (template != null)
                            {
                                var eraControl = template.LoadContent() as FrameworkElement;

                                if (eraControl != null)
                                {
                                    eraControl.DataContext = timelineEra;

                                    _cache[i] = eraControl;

                                    Children.Add(eraControl);
                                }
                                else
                                {
                                    Children.Add(new ContentControl { Content = timelineEra.ToString() });
                                }
                            }
                            else
                            {
                                Children.Add(new ContentControl { Content = timelineEra.ToString() });
                            }
                        }
                        else
                        {
                            Children.Add(_cache[i]);
                        }

                        _visibleCacheIndexes.Add(i);

                        if (_previouslyVisibleCacheIndexes.Contains(i))
                        {
                            _previouslyVisibleCacheIndexes.Remove(i);
                        }
                    }
                }

                _hasViewChanged = false;

                InvalidateMeasure();
            }

            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            return base.MeasureOverride(availableSize);
        }

        private static void EraPanel_ErasChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EraPanel)d).ClearCache();
        }

        private void ClearCache()
        {
            _cache = null;
            _hasViewChanged = true;
            InvalidateMeasure();
        }
    }
}