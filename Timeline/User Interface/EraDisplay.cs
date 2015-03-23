using NathanHarrenstein.Controls;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class EraDisplay : FrameworkElement, IPan
    {
        private string font;
        private double fontSize;
        private Brush foreground;
        private double horizontalOffset;
        private double labelOffset;

        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(double), typeof(EraDisplay));
        public static readonly DependencyProperty EraSettingsProperty = DependencyProperty.Register("EraSettings", typeof(List<EraSettings>), typeof(EraDisplay));
        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(List<Era>), typeof(EraDisplay));
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(double), typeof(EraDisplay));
        private static IOrderedEnumerable<Era> sortedEras;
        private static Dictionary<string, NathanHarrenstein.Timeline.EraSettings> eraSettingsDictionary;
        private static bool initialized;

        public EraDisplay()
        {
            SetValue(ErasProperty, new List<Era>());
            SetValue(EraSettingsProperty, new List<EraSettings>());

            if (!initialized)
            {
                Initialize();
            }
        }

        public double End
        {
            get
            {
                return (double)GetValue(EndProperty);
            }
            set
            {
                SetValue(EndProperty, value);
            }
        }

        public List<EraSettings> EraSettings
        {
            get
            {
                return (List<EraSettings>)GetValue(EraSettingsProperty);
            }
            set
            {
                SetValue(EraSettingsProperty, value);
            }
        }

        public List<Era> Eras
        {
            get
            {
                return (List<Era>)GetValue(ErasProperty);
            }
            set
            {
                SetValue(ErasProperty, value);
                Initialize();
            }
        }

        public double Start
        {
            get
            {
                return (double)GetValue(StartProperty);
            }
            set
            {
                SetValue(StartProperty, value);
            }
        }

        public string Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
            }
        }

        public double FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;
            }
        }

        public Brush Foreground
        {
            get
            {
                return foreground;
            }
            set
            {
                foreground = value;
            }
        }

        public double HorizontalOffset
        {
            get
            {
                return horizontalOffset;
            }
            set
            {
                horizontalOffset = value;
                InvalidateVisual();
            }
        }

        public double LabelOffset
        {
            get
            {
                return labelOffset;
            }
            set
            {
                labelOffset = value;
            }
        }

        public double TimeUnitWidth { get; set; }

        public Vector CanPan(Vector delta)
        {
            if (HorizontalOffset + delta.X >= 0)
            {
                return delta;
            }
            else
            {
                return new Vector(-HorizontalOffset, delta.Y);
            }
        }

        public void Pan(Vector delta)
        {
            HorizontalOffset += delta.X;
        }

        private void Initialize()
        {
            initialized = true;
            sortedEras = Eras.OrderByDescending(era => era.Start);
            eraSettingsDictionary = EraSettings.ToDictionary(es => es.Era);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var timePosition = Start + HorizontalOffset / TimeUnitWidth;
            var hasRendered = false;

            foreach (var era in sortedEras)
            {
                if (timePosition + ActualWidth / TimeUnitWidth >= era.Start && timePosition <= era.End)
                {
                    hasRendered = true;

                    DrawEra(timePosition, era, drawingContext);
                    DrawLabel(timePosition, era, drawingContext);
                }
                else if (hasRendered)
                {
                    break;
                }
            }
        }

        private void DrawEra(double timePosition, Era era, DrawingContext drawingContext)
        {
            var point = new Point((era.Start - timePosition) * TimeUnitWidth, 0d);
            var size = new Size((era.End - era.Start) * TimeUnitWidth, ActualHeight);
            var eraSetting = (EraSettings)null;

            if (eraSettingsDictionary.TryGetValue(era.Name, out eraSetting))
            {
                drawingContext.DrawRectangle(eraSetting.Brush, null, new Rect(point, size));
            }
            else
            {
                drawingContext.DrawRectangle(Brushes.Black, null, new Rect(point, size));
            }
        }

        private void DrawLabel(double timePosition, Era era, DrawingContext drawingContext)
        {
            var point = new Point((era.Start - timePosition) * TimeUnitWidth + LabelOffset, ActualHeight * 0.5 - FontSize * 0.6);
            var eraSetting = (EraSettings)null;

            if (eraSettingsDictionary.TryGetValue(era.Name, out eraSetting))
            {
                drawingContext.DrawText(new FormattedText(era.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Font), FontSize, eraSetting.TextBrush), point);
            }
            else
            {
                drawingContext.DrawText(new FormattedText(era.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Font), FontSize, Foreground), point);
            }
        }
    }
}