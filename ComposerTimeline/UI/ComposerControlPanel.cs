using NathanHarrenstein.ComposerTimeline.Controls;
using NathanHarrenstein.ComposerTimeline.Data;
using NathanHarrenstein.Controls;
using NathanHarrenstein.Timeline;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.ComposerTimeline
{
    public class ComposerControlPanel : Panel, IPan
    {
        public ComposerControlPanel()
        {
            VisibleComposerControls = new List<VisibleComposerControlData>();

            ClipToBounds = true;
        }

        public double End { get; set; }

        public List<Era> Eras { get; set; }

        public List<EraSettings> EraSettings { get; set; }

        public double EventHeight { get; set; }

        public double EventSpacing { get; set; }

        public double HorizontalOffset { get; set; }

        public double Start { get; set; }

        public double TimeUnitWidth { get; set; }

        public double VerticalOffset { get; set; }

        private List<VisibleComposerControlData> VisibleComposerControls { get; set; }

        public Vector CanPan(Vector delta)
        {
            var coercedX = delta.X;
            var coercedY = delta.Y;

            if (HorizontalOffset + coercedX < 0)
            {
                coercedX = -HorizontalOffset;
            }

            if (HorizontalOffset + coercedX > End * TimeUnitWidth - ActualWidth)
            {
                coercedX = End * TimeUnitWidth - ActualWidth - HorizontalOffset;
            }

            if (VerticalOffset + coercedY < 0)
            {
                coercedY = -VerticalOffset;
            }

            var eventCount = Eras.SelectMany(era => era.Events).Count();
            var extentHeight = eventCount * (EventSpacing + EventHeight);
            var visibleBottom = VerticalOffset + ActualHeight;
            var pansPastExtentHeight = VerticalOffset > 0 && visibleBottom + delta.Y > extentHeight;

            if (pansPastExtentHeight)
            {
                coercedY = 0;
            }

            delta.X = coercedX;
            delta.Y = coercedY;

            return delta;
        }

        public Vector? GetCenteringVector(ComposerControl composerControl)
        {
            // Only call during or after the arrange pass.
            // See: "file:///C:\Users\Nathan\SkyDrive\Programming\ComposerTimeline\Diagrams\Composer Control Layout Diagram.png"

            if (composerControl == null)
            {
                return null;
            }

            var eventIndex = Children.IndexOf(composerControl);
            var composerControlLeftEdge = (composerControl.ComposerEvent.Start - Start) * TimeUnitWidth;
            var composerControlHalfWidth = composerControl.ActualWidth / 2;
            var viewportHalfWidth = ActualWidth / 2;

            var x = composerControlLeftEdge + composerControlHalfWidth - viewportHalfWidth;

            var composerControlTopEdge = eventIndex * (EventHeight + EventSpacing);
            var composerControlHalfHeight = composerControl.ActualHeight / 2;
            var viewportHalfHeight = ActualHeight / 2;

            var y = composerControlTopEdge + composerControlHalfHeight - viewportHalfHeight;

            return new Vector(x - HorizontalOffset, y - VerticalOffset);
        }

        public void Pan(Vector delta)
        {
            HorizontalOffset += delta.X;
            VerticalOffset += delta.Y;

            InvalidateMeasure();
        }

        internal void LoadComposerControls()
        {
            var composerControls = new List<ComposerControl>();

            foreach (var era in Eras)
            {
                foreach (var composerEvent in era.Events.Cast<ComposerEvent>())
                {
                    var composerControlProvider = new ComposerControlProvider();
                    var composerControl = composerControlProvider.GetComposerControl(composerEvent);

                    composerControl.Width = (composerEvent.End - composerEvent.Start) * TimeUnitWidth;
                    composerControls.Add(composerControl);
                }
            }

            foreach (var composerControl in composerControls.OrderBy(x => x.ComposerEvent.Start))
            {
                Children.Add(composerControl);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var composerControlData in VisibleComposerControls)
            {
                var composerControl = composerControlData.ComposerControl;
                var eventTop = composerControlData.Index * (EventHeight + EventSpacing) - VerticalOffset;
                var viewportLeftTime = Start + HorizontalOffset / TimeUnitWidth;
                var eventLeft = (composerControl.ComposerEvent.Start - viewportLeftTime) * TimeUnitWidth;
                var eventBottom = eventTop + EventHeight;
                var viewportBottom = VerticalOffset + ActualHeight;
                var viewportTop = VerticalOffset;

                composerControl.Arrange(new Rect(eventLeft, eventTop, composerControl.DesiredSize.Width, composerControl.DesiredSize.Height));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var composerControls = Children.OfType<ComposerControl>();
            var viewportLeftTime = Start + HorizontalOffset / TimeUnitWidth;
            var viewportRightTime = viewportLeftTime + availableSize.Width / TimeUnitWidth;
            var eventIndex = 0;

            VisibleComposerControls.Clear();

            foreach (var composerControl in composerControls)
            {
                var eventTop = eventIndex * (EventHeight + EventSpacing);
                var eventBottom = eventTop + EventHeight;
                var viewportBottom = VerticalOffset + availableSize.Height;
                var viewportTop = VerticalOffset;

                var horizontallyVisible = composerControl.ComposerEvent.End >= viewportLeftTime - 100 && composerControl.ComposerEvent.Start <= viewportRightTime + 100;
                var verticallyVisible = eventTop < viewportBottom + 100 && eventBottom >= viewportTop - 100;

                if (horizontallyVisible && verticallyVisible)
                {
                    composerControl.Measure(availableSize);

                    var composerControlData = new VisibleComposerControlData();
                    composerControlData.ComposerControl = composerControl;
                    composerControlData.Index = eventIndex;

                    VisibleComposerControls.Add(composerControlData);
                }

                eventIndex++;
            }

            return base.MeasureOverride(availableSize);
        }
    }
}