using NathanHarrenstein.Controls;
using System.Collections.Generic;
using System.ExtendedDateTimeFormat;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System;

namespace NathanHarrenstein.Timeline
{
    public class EraPanel : Panel, IPan
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(EraPanel));
        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(IReadOnlyList<ITimelineEra>), typeof(EraPanel));
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

        public Vector CoercePan(Vector delta)
        {
            var extentWidth = Ruler.ToPixels(Dates);

            if (HorizontalOffset + delta.X < 0)                                                   // Panning too far left. Set horizontal pan distance to the amount of space remaining before the left edge.
            {
                delta.X = -HorizontalOffset;
            }

            if (HorizontalOffset + delta.X > extentWidth - ActualWidth)                           // Panning too far right. Set horizontal pan distance to the amount of space remaining before the right edge.
            {
                delta.X = extentWidth - (HorizontalOffset + ActualWidth);
            }

            return delta;
        }

        public void Pan(Vector delta)
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
                var visibleEra = _cache[visibleCacheIndex];

                var dates = Eras[visibleCacheIndex].Dates;

                var viewportLeftTime = Dates.Earliest() + Ruler.ToTimeSpan(HorizontalOffset);

                var eraRightTime = dates.Latest();

                var eraWidth = Ruler.ToPixels(dates);
                var eraHeight = finalSize.Height;

                var eraLeftViewportOffset = Ruler.ToPixels(viewportLeftTime, dates.Earliest());

                visibleEra.Arrange(new Rect(eraLeftViewportOffset, 0, eraWidth, eraHeight));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_cache == null)
            {
                _cache = new FrameworkElement[Eras.Count];
            }

            if (_hasViewChanged)
            {
                var viewportLeftTime = Dates.Earliest() + Ruler.ToTimeSpan(HorizontalOffset);
                var viewportRightTime = viewportLeftTime + Ruler.ToTimeSpan(availableSize.Width);

                for (var i = 0; i < Eras.Count; i++)
                {
                    var timelineEra = Eras[i];

                    var horizontallyVisible = timelineEra.Dates.End.Latest() > viewportLeftTime && timelineEra.Dates.Start.Earliest() < viewportRightTime;

                    if (horizontallyVisible)
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
    }
}