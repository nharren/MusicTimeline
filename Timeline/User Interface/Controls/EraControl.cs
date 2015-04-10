using System;
using System.Collections;
using System.Collections.Generic;
using System.ExtendedDateTimeFormat;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Timeline
{
    public class EraControl : Control
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(EraControl));

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(EraControl));

        static EraControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EraControl), new FrameworkPropertyMetadata(typeof(EraControl)));
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

        public string Label
        {
            get
            {
                return (string)GetValue(LabelProperty);
            }
            set
            {
                SetValue(LabelProperty, value);
            }
        }
    }
}