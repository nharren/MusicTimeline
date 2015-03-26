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
        public virtual DbSet<CatalogNumber> CatalogNumbers { get; set; }
        public virtual DbSet<Composer> Composers { get; set; }
        public virtual DbSet<ComposerImage> ComposerImages { get; set; }
        public virtual DbSet<ComposerLink> ComposerLinks { get; set; }
        public virtual DbSet<Composition> Compositions { get; set; }
        public virtual DbSet<CompositionCatalog> CompositionCatalogs { get; set; }
        public virtual DbSet<CompositionCollection> CompositionCollections { get; set; }
        public virtual DbSet<Era> Eras { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Movement> Movements { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<Performer> Performers { get; set; }
        public virtual DbSet<Recording> Recordings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Album>()
                .HasMany<Recording>(e => e.Recordings)
                .WithOptional(e => e.Album)
                .HasForeignKey(e => e.AlbumID);

            modelBuilder.Entity<CatalogNumber>()
                .Property(e => e.Number)
                .IsUnicode(false);

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
                .WithRequired(e => e.Composer)
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
                .HasMany(e => e.Influences)
                .WithMany(e => e.Influenced)
                .Map(m => m.ToTable("composer_influence", "music").MapRightKey("influence_id"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Nationalities)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("composer_nationality", "music"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.CompositionCollections)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("composition_collection_composer", "music"));

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Compositions)
                .WithMany(e => e.Composers)
                .Map(m => m.ToTable("composition_composer", "music"));

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
                .HasMany(e => e.CatalogNumbers)
                .WithRequired(e => e.Composition)
                .HasForeignKey(e => e.CompositionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.Movements)
                .WithRequired(e => e.Composition)
                .HasForeignKey(e => e.CompositionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composition>()
                .HasMany(e => e.Recordings)
                .WithRequired(e => e.Composition)
                .HasForeignKey(e => e.CompositionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCatalog>()
                .Property(e => e.Prefix)
                .IsUnicode(false);

            modelBuilder.Entity<CompositionCatalog>()
                .HasMany(e => e.CatalogNumber)
                .WithRequired(e => e.CompositionCatalog)
                .HasForeignKey(e => e.CompositionCatalogID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollection>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.CatalogNumber)
                .WithRequired(e => e.CompositionCollection)
                .HasForeignKey(e => e.CompositionCollectionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.Compositions)
                .WithRequired(e => e.CompositionCollections)
                .HasForeignKey(e => e.CompositionCollectionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.Recordings)
                .WithRequired(e => e.CompositionCollection)
                .HasForeignKey(e => e.CompositionCollectionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Era>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Era>()
                .Property(e => e.Dates)
                .IsUnicode(false);

            modelBuilder.Entity<Era>()
                .HasMany(e => e.Composers)
                .WithMany(e => e.Eras)
                .Map(m => m.ToTable("composer_era", "music"));

            modelBuilder.Entity<Location>()
                .Property(e => e.Name)
                .IsUnicode(false);

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
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.LocationID);

            modelBuilder.Entity<Movement>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Movement>()
                .HasMany(e => e.Recordings)
                .WithRequired(e => e.Movement)
                .HasForeignKey(e => e.MovementID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Nationality>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Performer>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Performer>()
                .HasMany(e => e.Recordings)
                .WithMany(e => e.Performers)
                .Map(m => m.ToTable("recording_performer", "music"));

            modelBuilder.Entity<Recording>()
                .Property(e => e.Dates)
                .IsUnicode(false);
        }
    }
}