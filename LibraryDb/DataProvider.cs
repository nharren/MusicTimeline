namespace NathanHarrenstein.LibraryDB
{
    using System.Data.Entity;
    using System.Linq;

    public partial class DataProvider : DbContext
    {
        public DataProvider()
            : base("name=DataProvider1")
        {
        }

        public virtual DbSet<Recording> Recordings { get; set; }

        public static void Add(int mdbid, string path)
        {
            using (var dataProvider = new DataProvider())
            {
                var recording = new Recording();
                recording.ID = dataProvider.Recordings.Count();
                recording.MDBID = mdbid;
                recording.FilePath = path;

                dataProvider.Recordings.Add(recording);
                dataProvider.SaveChanges();
            }
        }

        public static string[] Get(int mdbid)
        {
            using (var dataProvider = new DataProvider())
            {
                return dataProvider.Recordings
                    .Where(r => r.MDBID == mdbid)
                    .Select(r => r.FilePath)
                    .ToArray();
            }
        }

        public static string[] GetAll()
        {
            using (var dataProvider = new DataProvider())
            {
                return dataProvider.Recordings.Local.Select(r => r.FilePath).ToArray();
            }
        }

        public static bool Has(string path)
        {
            using (var dataProvider = new DataProvider())
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