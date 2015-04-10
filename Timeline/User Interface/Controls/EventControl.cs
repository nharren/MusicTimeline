using System.Collections.Generic;
using System.ExtendedDateTimeFormat;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NathanHarrenstein.Timeline
{
    public class EventControl : Control
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventControl));

        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(EventControl));

        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(IList<EraControl>), typeof(EventControl));

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(EventControl));

        static EventControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EventControl), new FrameworkPropertyMetadata(typeof(EventControl)));
        }

        public EventControl()
        {
            Eras = new List<EraControl>();
        }

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
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

        public IList<EraControl> Eras
        {
            get
            {
                return (IList<EraControl>)GetValue(ErasProperty);
            }
            set
            {
                SetValue(ErasProperty, value);
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