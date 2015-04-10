using NathanHarrenstein.Timeline;
using System.ExtendedDateTimeFormat;
using System.Windows;
using System.Windows.Media;

namespace TimelineTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var medievalEra = new EraControl
            {
                Dates = ExtendedDateTimeInterval.Parse("1000/1400"),
                Background = Brushes.Gray,
                Foreground = Brushes.White,
                Label = "Medieval"
            };

            var renaissanceEra = new EraControl
            {
                Dates = ExtendedDateTimeInterval.Parse("1400/1600"),
                Background = Brushes.Purple,
                Foreground = Brushes.White,
                Label = "Renaissance"
            };

            var medievalEvent1 = new EventControl
            {
                Label = "Medieval Event 1",
                Dates = ExtendedDateTimeInterval.Parse("1000/1089"),
                Background = medievalEra.Background,
                Foreground = medievalEra.Foreground,
                Eras = new EraControl[] { medievalEra }
            };

            var medievalEvent2 = new EventControl
            {
                Label = "Medieval Event 2",
                Dates = ExtendedDateTimeInterval.Parse("1056/1113"),
                Background = medievalEra.Background,
                Foreground = medievalEra.Foreground,
                Eras = new EraControl[] { medievalEra }
            };

            var medievalEvent3 = new EventControl
            {
                Label = "Medieval Event 3",
                Dates = ExtendedDateTimeInterval.Parse("1077/1163"),
                Background = medievalEra.Background,
                Foreground = medievalEra.Foreground,
                Eras = new EraControl[] { medievalEra }
            };

            var renaissanceEvent1 = new EventControl
            {
                Label = "Renaissance Event 1",
                Dates = ExtendedDateTimeInterval.Parse("1402/1470"),
                Background = renaissanceEra.Background,
                Foreground = renaissanceEra.Foreground,
                Eras = new EraControl[] { renaissanceEra }
            };

            var renaissanceEvent2 = new EventControl
            {
                Label = "Renaissance Event 2",
                Dates = ExtendedDateTimeInterval.Parse("1432/1500"),
                Background = renaissanceEra.Background,
                Foreground = renaissanceEra.Foreground,
                Eras = new EraControl[] { renaissanceEra }
            };

            var renaissanceEvent3 = new EventControl
            {
                Label = "Renaissance Event 3",
                Dates = ExtendedDateTimeInterval.Parse("1440/1511"),
                Background = renaissanceEra.Background,
                Foreground = renaissanceEra.Foreground,
                Eras = new EraControl[] { renaissanceEra }
            };

            timeline.Eras.Add(medievalEra);
            timeline.Eras.Add(renaissanceEra);

            timeline.Events.Add(medievalEvent1);
            timeline.Events.Add(medievalEvent2);
            timeline.Events.Add(medievalEvent3);
            timeline.Events.Add(renaissanceEvent1);
            timeline.Events.Add(renaissanceEvent2);
            timeline.Events.Add(renaissanceEvent3);
        }
    }
}