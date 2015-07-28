namespace NathanHarrenstein.MusicDB
{
    public class RecordingLocation
    {
        public Location Location { get; set; }
        public int LocationID { get; set; }
        public Recording Recording { get; set; }
        public int RecordingID { get; set; }
    }
}