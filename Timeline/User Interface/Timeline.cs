using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Timeline
{
    public class Timeline : Control
    {
        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(double), typeof(Timeline));
        public static readonly DependencyProperty EraSettingsProperty = DependencyProperty.Register("EraSettings", typeof(List<EraSettings>), typeof(Timeline));
        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(List<Era>), typeof(Timeline));
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(double), typeof(Timeline));

        static Timeline()
        {
            var thisType = typeof(Timeline);
            var styleKeyMetadata = new FrameworkPropertyMetadata(thisType);
            DefaultStyleKeyProperty.OverrideMetadata(thisType, styleKeyMetadata);
        }

        public Timeline()
        {
            SetValue(ErasProperty, new List<Era>());
            SetValue(EraSettingsProperty, new List<EraSettings>());
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
    }
}