namespace Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataProvider : DbContext
    {
        public DataProvider()
            : base("name=DataProvider")
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Composer> Composers { get; set; }
        public virtual DbSet<ComposerImage> ComposerImages { get; set; }
        public virtual DbSet<ComposerLink> ComposerLinks { get; set; }
        public virtual DbSet<Composition> Compositions { get; set; }
        public virtual DbSet<CompositionCatalog> CompositionCatalogs { get; set; }
        public virtual DbSet<CompositionCatalogNumber> CompositionCatalogNumbers { get; set; }
        public virtual DbSet<CompositionCollection> CompositionCollections { get; set; }
        public virtual DbSet<CompositionCollectionCatalogNumber> CompositionCollectionCatalogNumbers { get; set; }
        public virtual DbSet<CompositionCollectionRecording> CompositionCollectionRecordings { get; set; }
        public virtual DbSet<CompositionRecording> CompositionRecordings { get; set; }
        public virtual DbSet<Era> Eras { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Movement> Movements { get; set; }
        public virtual DbSet<MovementRecording> MovementRecordings { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<Performer> Performers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Album>()
                .HasMany(e => e.CompositionCollectionRecordings)
                .WithOptional(e => e.Album)
                .HasForeignKey(e => e.AlbumID);

            modelBuilder.Entity<Album>()
                .HasMany(e => e.CompositionRecordings)
                .WithOptional(e => e.Album)
                .HasForeignKey(e => e.AlbumID);

            modelBuilder.Entity<Album>()
                .HasMany(e => e.MovementRecordings)
                .WithOptional(e => e.Album)
                .HasForeignKey(e => e.AlbumID);

            modelBuilder.Entity<Composer>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Composer>()
                .Property(e => e.Dates)
                .IsUnicode(false);

            modelBuilder.Entity<Composer>()
                .Property(e => e.Biography)
                .IsUnicode(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.ComposerImages)
                .WithRequired(e => e.composer)
                .HasForeignKey(e => e.ComposerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.ComposerLinks)
                .WithRequired(e => e.Composer)
                .HasForeignKey(e => e.ComposerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.CompositionCatalogs)
                .WithRequired(e => e.Composer)
                .HasForeignKey(e => e.ComposerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.CompositionCollections)
                .WithRequired(e => e.Composer)
                .HasForeignKey(e => e.ComposerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Compositions)
                .WithRequired(e => e.Composer)
                .HasForeignKey(e => e.ComposerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Influences)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("composer_influence", "classical_music").MapRightKey("influence_id"));

            modelBuilder.Entity<ComposerImage>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<ComposerLink>()
                .Property(e => e.URL)
                .IsUnicode(false);

            modelBuilder.Entity<Composition>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Composition>()
                .Property(e => e.Dates)
                .IsUnicode(false);

            modelBuilder.Entity<Composition>()
                .Property(e => e.Nickname)
                .IsUnicode(false);

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.CompositionCatalogNumber)
                .WithRequired(e => e.Composition)
                .HasForeignKey(e => e.CompositionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.CompositionRecording)
                .WithRequired(e => e.Composition)
                .HasForeignKey(e => e.CompositionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.Movements)
                .WithRequired(e => e.Composition)
                .HasForeignKey(e => e.CompositionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCatalog>()
                .Property(e => e.Prefix)
                .IsUnicode(false);

            modelBuilder.Entity<CompositionCatalog>()
                .HasMany(e => e.CompositionCatalogNumber)
                .WithRequired(e => e.CompositionCatalog)
                .HasForeignKey(e => e.CompositionCatalogID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCatalog>()
                .HasMany(e => e.CompositionCollectionCatalogNumber)
                .WithRequired(e => e.CompositionCatalog)
                .HasForeignKey(e => e.CompositionCatalogID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCatalogNumber>()
                .Property(e => e.CatalogNumber)
                .IsUnicode(false);

            modelBuilder.Entity<CompositionCollection>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.Compositions)
                .WithRequired(e => e.CompositionCollection)
                .HasForeignKey(e => e.CompositionCollectionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.CompositionCollectionCatalogNumber)
                .WithRequired(e => e.CompositionCollection)
                .HasForeignKey(e => e.CompositionCollectionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.CompositionCollectionRecording)
                .WithRequired(e => e.CompositionCollection)
                .HasForeignKey(e => e.CompositionCollectionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollectionCatalogNumber>()
                .Property(e => e.CatalogNumber)
                .IsUnicode(false);

            modelBuilder.Entity<CompositionCollectionRecording>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<CompositionCollectionRecording>()
                .Property(e => e.Dates)
                .IsUnicode(false);

            modelBuilder.Entity<CompositionRecording>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<CompositionRecording>()
                .Property(e => e.Dates)
                .IsUnicode(false);

            modelBuilder.Entity<Era>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Era>()
                .Property(e => e.Dates)
                .IsUnicode(false);

            modelBuilder.Entity<Era>()
                .HasMany(e => e.Composers)
                .WithMany(e => e.Eras)
                .Map(m => m.ToTable("composer_era", "classical_music"));

            modelBuilder.Entity<Location>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.BirthLocations)
                .WithOptional(e => e.BirthLocation)
                .HasForeignKey(e => e.BirthLocationID);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.DeathLocations)
                .WithOptional(e => e.DeathLocation)
                .HasForeignKey(e => e.DeathLocationID);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.CompositionCollectionRecordings)
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.LocationID);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.CompositionRecordings)
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.LocationID);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.MovementRecordings)
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.LocationID);

            modelBuilder.Entity<Movement>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Movement>()
                .HasMany(e => e.MovementRecordings)
                .WithRequired(e => e.Movement)
                .HasForeignKey(e => e.MovementID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MovementRecording>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<MovementRecording>()
                .Property(e => e.Dates)
                .IsUnicode(false);

            modelBuilder.Entity<Nationality>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Nationality>()
                .HasMany(e => e.Composers)
                .WithOptional(e => e.Nationality)
                .HasForeignKey(e => e.NationalityID);

            modelBuilder.Entity<Performer>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Performer>()
                .HasMany(e => e.CompositionCollectionRecordings)
                .WithMany(e => e.Performers)
                .Map(m => m.ToTable("composition_collection_recording_performer", "classical_music"));

            modelBuilder.Entity<Performer>()
                .HasMany(e => e.CompositionRecordings)
                .WithMany(e => e.Performers)
                .Map(m => m.ToTable("composition_recording_performer", "classical_music"));

            modelBuilder.Entity<Performer>()
                .HasMany(e => e.MovementRecordings)
                .WithMany(e => e.Performers)
                .Map(m => m.ToTable("movement_recording_performer", "classical_music"));
        }
    }
}
