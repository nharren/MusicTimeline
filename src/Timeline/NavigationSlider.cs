using System.Collections.Generic;
using System.EDTF;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace NathanHarrenstein.Timeline
{
    public class NavigationSlider : Control, IPan
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(NavigationSlider));
        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(IReadOnlyList<ITimelineEra>), typeof(NavigationSlider));
        public static readonly DependencyProperty EraTemplatesProperty = DependencyProperty.Register("EraTemplates", typeof(List<DataTemplate>), typeof(NavigationSlider));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeResolution), typeof(NavigationSlider));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(NavigationSlider));

        private readonly RoutedEvent _panRequestedEvent;
        private ColumnDefinition _centerColumn;
        private double _horizontalOffset;
        private ColumnDefinition _leftColumn;
        private Thumb _thumb;

        static NavigationSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationSlider), new FrameworkPropertyMetadata(typeof(NavigationSlider)));
        }

        public NavigationSlider()
        {
            EventManager.RegisterClassHandler(typeof(NavigationSlider), Thumb.DragDeltaEvent, new DragDeltaEventHandler(NavigationSlider_DragDelta));

            _panRequestedEvent = EventManager.GetRoutedEvents().FirstOrDefault(re => re.Name == "PanRequested");

            Unloaded += NavigationSlider_Unloaded;
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
            return delta;
        }

        public override void OnApplyTemplate()
        {
            _leftColumn = (ColumnDefinition)Template.FindName("PART_LeftColumn", this);
            _centerColumn = (ColumnDefinition)Template.FindName("PART_CenterColumn", this);
            _thumb = (Thumb)Template.FindName("PART_Thumb", this);
        }

        public void Pan(Vector delta)
        {
            _horizontalOffset += delta.X;

            var ratio = _horizontalOffset / Ruler.ToPixels(Dates);
            var sliderLeftX = ActualWidth * ratio;

            _leftColumn.Width = new GridLength(sliderLeftX);
        }

        public void RequestPan(Vector delta)
        {
            if (_panRequestedEvent != null)
            {
                RaiseEvent(new PanEventArgs(delta, _panRequestedEvent, this));
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _centerColumn.Width = new GridLength(constraint.Width * (constraint.Width / Ruler.ToPixels(Dates)));

            return base.MeasureOverride(constraint);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var positionX = e.GetPosition(this).X;
            var ratio = positionX / ActualWidth;
            var extentCenterX = ratio * Ruler.ToPixels(Dates) - ActualWidth / 2;
            var panVector = new Vector(extentCenterX - HorizontalOffset, 0);

            RequestPan(panVector);

            base.OnMouseLeftButtonDown(e);
        }

        private void NavigationSlider_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var ratio = e.HorizontalChange / ActualWidth;
            var extentHorizontalChangeX = ratio * Ruler.ToPixels(Dates);
            var panVector = new Vector(extentHorizontalChangeX, 0);

            RequestPan(panVector);

            e.Handled = true;
        }

        private void NavigationSlider_Unloaded(object sender, RoutedEventArgs e)
        {
            _thumb.CancelDrag();
        }
    }
}