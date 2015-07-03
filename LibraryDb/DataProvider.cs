using System.Data.Entity;

namespace NathanHarrenstein.LibraryDb
{
    public partial class DataProvider : DbContext
    {
        public DataProvider()
            : base("name=DataProvider")
        {
        }

        public virtual DbSet<Recording> Recordings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recording>()
                .Property(e => e.Path)
                .IsUnicode(false);
        }
    }
}