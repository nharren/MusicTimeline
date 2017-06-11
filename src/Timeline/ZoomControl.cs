using System;
using System.Collections.Generic;
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

namespace NathanHarrenstein.Timeline
{
    public class ZoomControl : Control
    {
        private bool isTemplateApplied;
        private Slider slider;

        static ZoomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomControl), new FrameworkPropertyMetadata(typeof(ZoomControl)));
        }

        public UIElement Target
        {
            get
            {
                return (UIElement)GetValue(TargetProperty);
            }

            set
            {
                SetValue(TargetProperty, value);
            }
        }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(UIElement), typeof(ZoomControl), new PropertyMetadata(null, Target_Changed));

        private static void Target_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnApplyTemplate()
        {
            if (isTemplateApplied)
            {
                return;
            }

            slider = (Slider)Template.FindName("slider", this);
            slider.ValueChanged += Slider_ValueChanged;

            isTemplateApplied = true;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            throw new NotImplementedException();
        }
    }
}
