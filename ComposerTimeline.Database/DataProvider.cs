namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.Data.Entity;

    public partial class DataProvider : DbContext
    {
        public DataProvider()
            : base("name=DataProvider")
        {
        }

        public virtual DbSet<Album> Albums { get; set; }

        public virtual DbSet<Composer> Composers { get; set; }

        public virtual DbSet<ComposerImage> ComposerImages { get; set; }

        public virtual DbSet<ComposerInfluence> ComposerInfluences { get; set; }

        public virtual DbSet<ComposerLink> ComposerLinks { get; set; }

        public virtual DbSet<Composition> Compositions { get; set; }

        public virtual DbSet<CompositionAudioFile> CompositionAudioFiles { get; set; }

        public virtual DbSet<CompositionAudioFilePerformer> CompositionAudioFilePerformers { get; set; }

        public virtual DbSet<CompositionCatalog> CompositionCatalogs { get; set; }

        public virtual DbSet<CompositionCatalogNumber> CompositionCatalogNumbers { get; set; }

        public virtual DbSet<CompositionCollection> CompositionCollections { get; set; }

        public virtual DbSet<CompositionCollectionAudioFile> CompositionCollectionAudioFiles { get; set; }

        public virtual DbSet<CompositionCollectionAudioFilePerformer> CompositionCollectionAudioFilePerformers { get; set; }

        public virtual DbSet<CompositionCollectionCatalogNumber> CompositionCollectionCatalogNumbers { get; set; }

        public virtual DbSet<Era> Eras { get; set; }

        public virtual DbSet<Key> Keys { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<Movement> Movements { get; set; }

        public virtual DbSet<MovementAudioFile> MovementAudioFiles { get; set; }

        public virtual DbSet<MovementAudioFilePerformer> MovementAudioFilePerformers { get; set; }

        public virtual DbSet<Nationality> Nationalities { get; set; }

        public virtual DbSet<Performer> Performers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Composer>()
                .HasMany(e => e.ComposerImages)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.ComposersInfluenced)
                .WithRequired(e => e.Influenced)
                .HasForeignKey(e => e.InfluencedID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.ComposerInfluences)
                .WithRequired(e => e.Influence)
                .HasForeignKey(e => e.InfluenceID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.ComposerLinks)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Compositions)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.CompositionCatalogs)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.CompositionCollections)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.CompositionAudioFiles)
                .WithRequired(e => e.Composition)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.CompositionCatalogNumbers)
                .WithRequired(e => e.Composition)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.Movements)
                .WithRequired(e => e.Composition)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionAudioFile>()
                .HasMany(e => e.CompositionAudioFilePerformers)
                .WithRequired(e => e.CompositionAudioFile)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCatalog>()
                .HasMany(e => e.CompositionCatalogNumbers)
                .WithRequired(e => e.CompositionCatalog)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCatalog>()
                .HasMany(e => e.CompositionCollectionCatalogNumbers)
                .WithRequired(e => e.CompositionCatalog)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.Compositions)
                .WithRequired(e => e.CompositionCollection)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.CompositionCollectionAudioFiles)
                .WithRequired(e => e.CompositionCollection)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.CompositionCollectionCatalogNumbers)
                .WithRequired(e => e.CompositionCollection)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollectionAudioFile>()
                .HasMany(e => e.CompositionCollectionAudioFilePerformers)
                .WithRequired(e => e.CompositionCollectionAudioFile)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Era>()
                .HasMany(e => e.Composers)
                .WithRequired(e => e.Era)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Composers)
                .WithOptional(e => e.DeathPlace)
                .HasForeignKey(e => e.DeathPlaceID);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Composers1)
                .WithOptional(e => e.BirthPlace)
                .HasForeignKey(e => e.BirthPlaceID);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.CompositionAudioFiles)
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.RecordingLocationID);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.CompositionCollectionAudioFiles)
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.RecordingLocationID);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.MovementAudioFiles)
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.RecordingLocationID);

            modelBuilder.Entity<Movement>()
                .HasMany(e => e.MovementAudioFiles)
                .WithRequired(e => e.Movement)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MovementAudioFile>()
                .HasMany(e => e.MovementAudioFilePerformers)
                .WithRequired(e => e.MovementAudioFile)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Performer>()
                .HasMany(e => e.CompositionAudioFilePerformers)
                .WithRequired(e => e.Performer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Performer>()
                .HasMany(e => e.CompositionCollectionAudioFilePerformers)
                .WithRequired(e => e.Performer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Performer>()
                .HasMany(e => e.MovementAudioFilePerformers)
                .WithRequired(e => e.Performer)
                .WillCascadeOnDelete(false);
        }
    }
}