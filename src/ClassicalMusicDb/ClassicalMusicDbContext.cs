namespace NathanHarrenstein.ClassicalMusicDb
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ClassicalMusicDbContext : DbContext
    {
        public ClassicalMusicDbContext()
            : base("name=ClassicalMusicDbContext")
        {
            Database.SetInitializer<ClassicalMusicDbContext>(null);
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Catalog> Catalogs { get; set; }
        public virtual DbSet<CatalogNumber> CatalogNumbers { get; set; }
        public virtual DbSet<Composer> Composers { get; set; }
        public virtual DbSet<ComposerImage> ComposerImages { get; set; }
        public virtual DbSet<Composition> Compositions { get; set; }
        public virtual DbSet<CompositionCollection> CompositionCollections { get; set; }
        public virtual DbSet<Era> Eras { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Instrumentation> Instrumentations { get; set; }
        public virtual DbSet<Key> Keys { get; set; }
        public virtual DbSet<Link> Links { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Movement> Movements { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<Performer> Performers { get; set; }
        public virtual DbSet<Recording> Recordings { get; set; }
        public virtual DbSet<Sample> Samples { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Catalog>()
                .HasMany(e => e.CatalogNumbers)
                .WithRequired(e => e.Catalog)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CatalogNumber>()
                .HasMany(e => e.Compositions)
                .WithMany(e => e.CatalogNumbers)
                .Map(m => m.ToTable("CompositionCatalogNumber").MapLeftKey("CatalogNumberId").MapRightKey("CompositionId"));

            modelBuilder.Entity<CatalogNumber>()
                .HasMany(e => e.CompositionCollections)
                .WithMany(e => e.CatalogNumbers)
                .Map(m => m.ToTable("CompositionCollectionCatalogNumber").MapLeftKey("CatalogNumberId").MapRightKey("CompositionCollectionId"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Catalogs)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.ComposerImages)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Samples)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Eras)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("ComposerEra").MapLeftKey("ComposerId").MapRightKey("EraId"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Influences)
                .WithMany(e => e.Influenced)
                .Map(m => m.ToTable("ComposerInfluence").MapLeftKey("ComposerId").MapRightKey("InfluenceId"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Links)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("ComposerLink").MapLeftKey("ComposerId").MapRightKey("LinkId"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Nationalities)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("ComposerNationality").MapLeftKey("ComposerId").MapRightKey("NationalityId"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.CompositionCollections)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("CompositionCollectionComposer").MapLeftKey("ComposerId").MapRightKey("CompositionCollectionId"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Compositions)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("CompositionComposer").MapLeftKey("ComposerId").MapRightKey("CompositionId"));

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.Movements)
                .WithRequired(e => e.Composition)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.Links)
                .WithMany(e => e.Compositions)
                .Map(m => m.ToTable("CompositionLink").MapLeftKey("CompositionId").MapRightKey("LinkId"));

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.Recordings)
                .WithMany(e => e.Compositions)
                .Map(m => m.ToTable("CompositionRecording").MapLeftKey("CompositionId").MapRightKey("RecordingId"));

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.Recordings)
                .WithMany(e => e.CompositionCollections)
                .Map(m => m.ToTable("CompositionCollectionRecording").MapLeftKey("CompositionCollectionId").MapRightKey("RecordingId"));

            modelBuilder.Entity<Location>()
                .HasMany(e => e.BirthLocationComposers)
                .WithOptional(e => e.BirthLocation)
                .HasForeignKey(e => e.BirthLocationId);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.DeathLocationComposers)
                .WithOptional(e => e.DeathLocation)
                .HasForeignKey(e => e.DeathLocationId);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Recordings)
                .WithMany(e => e.Locations)
                .Map(m => m.ToTable("RecordingLocation").MapLeftKey("LocationId").MapRightKey("RecordingId"));

            modelBuilder.Entity<Movement>()
                .HasMany(e => e.Recordings)
                .WithMany(e => e.Movements)
                .Map(m => m.ToTable("MovementRecording").MapLeftKey("MovementId").MapRightKey("RecordingId"));

            modelBuilder.Entity<Performer>()
                .HasMany(e => e.Recordings)
                .WithMany(e => e.Performers)
                .Map(m => m.ToTable("RecordingPerformer").MapLeftKey("PerformerId").MapRightKey("RecordingId"));
        }
    }
}
