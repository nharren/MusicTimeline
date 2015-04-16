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

            var medievalEra = new TimelineEra("Medieval", ExtendedDateTimeInterval.Parse("1000/1400"), Brushes.Gray, Brushes.White);
            var renaissanceEra = new TimelineEra("Renaissance", ExtendedDateTimeInterval.Parse("1400/1600"), Brushes.Purple, Brushes.White);

            var medievalEvent1 = new TimelineEvent("Medieval Event 1", ExtendedDateTimeInterval.Parse("1000/1089"), medievalEra);
            var medievalEvent2 = new TimelineEvent("Medieval Event 2", ExtendedDateTimeInterval.Parse("1056/1113"), medievalEra);
            var medievalEvent3 = new TimelineEvent("Medieval Event 3", ExtendedDateTimeInterval.Parse("1077/1163"), medievalEra);
            var renaissanceEvent1 = new TimelineEvent("Renaissance Event 1", ExtendedDateTimeInterval.Parse("1402/1470"), renaissanceEra);
            var renaissanceEvent2 = new TimelineEvent("Renaissance Event 2", ExtendedDateTimeInterval.Parse("1432/1500"), renaissanceEra);
            var renaissanceEvent3 = new TimelineEvent("Renaissance Event 3", ExtendedDateTimeInterval.Parse("1440/1511"), renaissanceEra);


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