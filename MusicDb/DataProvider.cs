using System.Data.Entity;

namespace NathanHarrenstein.MusicDb
{
    public partial class DataProvider : DbContext
    {
        public DataProvider()
            : base("name=DataProvider")
        {
        }

        public virtual DbSet<Album> Albums { get; set; }

        public virtual DbSet<CatalogNumber> CatalogNumbers { get; set; }

        public virtual DbSet<ComposerImage> ComposerImages { get; set; }

        public virtual DbSet<ComposerLink> ComposerLinks { get; set; }

        public virtual DbSet<Composer> Composers { get; set; }

        public virtual DbSet<CompositionCatalog> CompositionCatalogs { get; set; }

        public virtual DbSet<CompositionCollection> CompositionCollections { get; set; }

        public virtual DbSet<Composition> Compositions { get; set; }

        public virtual DbSet<Era> Eras { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<Movement> Movements { get; set; }

        public virtual DbSet<Nationality> Nationalities { get; set; }

        public virtual DbSet<Performer> Performers { get; set; }

        public virtual DbSet<Recording> Recordings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Composer>()
                .HasMany(e => e.ComposerImages)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.ComposerLinks)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.CompositionCatalogs)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Eras)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("ComposerEra").MapLeftKey("ComposerID").MapRightKey("EraID"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Influences)
                .WithMany(e => e.Influenced)
                .Map(m => m.ToTable("ComposerInfluence").MapLeftKey("ComposerID").MapRightKey("InfluenceID"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Nationalities)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("ComposerNationality").MapLeftKey("ComposerID").MapRightKey("NationalityID"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.CompositionCollections)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("CompositionCollectionComposer").MapLeftKey("ComposerID").MapRightKey("CompositionCollectionID"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Compositions)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("CompositionComposer").MapLeftKey("ComposerID").MapRightKey("CompositionID"));

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.Movements)
                .WithRequired(e => e.Composition)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCatalog>()
                .HasMany(e => e.CatalogNumbers)
                .WithRequired(e => e.CompositionCatalog)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.BirthLocationComposers)
                .WithOptional(e => e.BirthLocation)
                .HasForeignKey(e => e.BirthLocationID);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.DeathLocationComposers)
                .WithOptional(e => e.DeathLocation)
                .HasForeignKey(e => e.DeathLocationID);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Recordings)
                .WithMany(e => e.Locations)
                .Map(m => m.ToTable("RecordingLocation").MapLeftKey("LocationID").MapRightKey("RecordingID"));

            modelBuilder.Entity<Performer>()
                .HasMany(e => e.Recordings)
                .WithMany(e => e.Performers)
                .Map(m => m.ToTable("RecordingPerformer").MapLeftKey("PerformerID").MapRightKey("RecordingID"));
        }
    }
}