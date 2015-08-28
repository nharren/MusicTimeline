namespace NathanHarrenstein.MusicLibraryDb
{
    using System.Data.Entity;
    using System.Linq;

    public partial class MusicLibraryContext : DbContext
    {
        public MusicLibraryContext()
            : base("name=MusicLibraryContext")
        {
        }

        public virtual DbSet<Recording> Recordings { get; set; }

        public static void Add(int mdbid, string path)
        {
            using (var dataProvider = new MusicLibraryContext())
            {
                var recording = new Recording();
                recording.RecordingId = dataProvider.Recordings.Count();
                recording.ClassicalMusicDatabaseId = mdbid;
                recording.FilePath = path;

                dataProvider.Recordings.Add(recording);
                dataProvider.SaveChanges();
            }
        }

        public static string[] Get(int mdbid)
        {
            using (var dataProvider = new MusicLibraryContext())
            {
                return dataProvider.Recordings
                    .Where(r => r.ClassicalMusicDatabaseId == mdbid)
                    .Select(r => r.FilePath)
                    .ToArray();
            }
        }

        public static string[] GetAll()
        {
            using (var dataProvider = new MusicLibraryContext())
            {
                return dataProvider.Recordings.Local.Select(r => r.FilePath).ToArray();
            }
        }

        public static bool Has(string path)
        {
            using (var dataProvider = new MusicLibraryContext())
            {
                return dataProvider.Recordings.Any(r => r.FilePath == path);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recording>()
                .Property(e => e.FilePath)
                .IsUnicode(false);
        }
    }
}