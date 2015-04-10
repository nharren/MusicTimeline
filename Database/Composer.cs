namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.CodeAnalysis;

    [Table("music.composer")]
    public partial class Composer
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Composer()
        {
            ComposerImages = new HashSet<ComposerImage>();
            ComposerLinks = new HashSet<ComposerLink>();
            CompositionCatalogs = new HashSet<CompositionCatalog>();
            Eras = new HashSet<Era>();
            Influences = new HashSet<Composer>();
            Influenced = new HashSet<Composer>();
            Nationalities = new HashSet<Nationality>();
            CompositionCollections = new HashSet<CompositionCollection>();
            Compositions = new HashSet<Composition>();
        }

        [Column("id", TypeName = "usmallint")]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Column("dates")]
        public string DatesString { get; set; }

        [Column("birth_location_id", TypeName = "umediumint")]
        public int? BirthLocationID { get; set; }

        [Column("death_location_id", TypeName = "umediumint")]
        public int? DeathLocationID { get; set; }

        [Column("biography", TypeName = "text")]
        [StringLength(65535)]
        public string Biography { get; set; }

        [Column("is_popular")]
        public bool IsPopular { get; set; }

        public virtual Location BirthLocation { get; set; }

        public virtual Location DeathLocation { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComposerImage> ComposerImages { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComposerLink> ComposerLinks { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCatalog> CompositionCatalogs { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Era> Eras { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composer> Influences { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composer> Influenced { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Nationality> Nationalities { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCollection> CompositionCollections { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composition> Compositions { get; set; }
    }
}
