namespace NathanHarrenstein.Timeline
{
    public class Event
    {
        private string name;
        private double start;
        private double end;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public double Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        public double End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }
    }
}