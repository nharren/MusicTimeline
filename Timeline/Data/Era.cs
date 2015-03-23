using System.Collections.Generic;

namespace NathanHarrenstein.Timeline
{
    public class Era : Event
    {
        private List<Event> events;

        public Era()
        {
            events = new List<Event>();
        }

        public virtual List<Event> Events
        {
            get
            {
                return events;
            }
            set
            {
                events = value;
            }
        }
    }
}