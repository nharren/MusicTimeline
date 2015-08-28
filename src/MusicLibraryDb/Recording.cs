namespace NathanHarrenstein.MusicLibraryDb
{
    public partial class Recording
    {
        public int RecordingId { get; set; }

        public int ClassicalMusicDatabaseId { get; set; }

        public string FilePath { get; set; }
    }
}