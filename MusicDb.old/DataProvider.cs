using System.Collections.ObjectModel;
using System.Data.Entity;

namespace NathanHarrenstein.MusicDb
{
    public partial class DataProvider : DbContext
    {
        public DataProvider()
            : base("name=DataProvider")
        {
            Database.SetInitializer<DataProvider>(null);
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
                .HasMany(e => e.Recordings)
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
                .Map(m =>
                {
                    m.MapLeftKey("composer_id");
                    m.MapRightKey("influence_id");
                    m.ToTable("composer_influence", "music_test");
                });

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Nationalities)
                .WithMany(e => e.Composers)
                .Map(m =>
                {
                    m.MapLeftKey("composer_id");
                    m.MapRightKey("nationality_id");
                    m.ToTable("composer_nationality", "music_test");
                });

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.CompositionCollections)
                .WithMany(e => e.Composers)
                .Map(m =>
                {
                    m.MapLeftKey("composer_id");
                    m.MapRightKey("composition_collection_id");
                    m.ToTable("composition_collection_composer", "music_test");
                });

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Compositions)
                .WithMany(e => e.Composers)
                .Map(m =>
                {
                    m.MapLeftKey("composer_id");
                    m.MapRightKey("composition_id");
                    m.ToTable("composition_composer", "music_test");
                });

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
                .HasMany(e => e.CatalogNumbers)
                .WithRequired(e => e.CompositionCatalog)
                .HasForeignKey(e => e.CompositionCatalogID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollection>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.CatalogNumbers)
                .WithRequired(e => e.CompositionCollection)
                .HasForeignKey(e => e.CompositionCollectionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompositionCollection>()
                .HasMany(e => e.Compositions)
                .WithRequired(e => e.CompositionCollection)
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
                .Map(m =>
                {
                    m.MapLeftKey("era_id");
                    m.MapRightKey("composer_id");
                    m.ToTable("composer_era", "music_test");
                });

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
                .Map(m =>
                {
                    m.MapLeftKey("performer_id");
                    m.MapRightKey("recording_id");
                    m.ToTable("recording_performer", "music_test");
                });

            modelBuilder.Entity<Recording>()
                .Property(e => e.Dates)
                .IsUnicode(false);

            modelBuilder.Entity<Recording>()
                .HasMany(e => e.Locations)
                .WithMany(e => e.Recordings)
                .Map(m =>
                {
                    m.MapLeftKey("recording_id");
                    m.MapRightKey("location_id");
                    m.ToTable("recording_location", "music_test");
                });
        }
    }
}