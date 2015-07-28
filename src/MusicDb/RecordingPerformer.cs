namespace NathanHarrenstein.MusicDB
{
    public class RecordingPerformer
    {
        public Performer Performer { get; set; }
        public int PerformerID { get; set; }
        public Recording Recording { get; set; }
        public int RecordingID { get; set; }
    }
}