namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.CodeAnalysis;

    [Table("music.composition_collection")]
    public partial class CompositionCollection
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompositionCollection()
        {
            CatalogNumber = new HashSet<CatalogNumber>();
            Compositions = new HashSet<Composition>();
            Recordings = new HashSet<Recording>();
            Composers = new HashSet<Composer>();
        }

        [Column("id", TypeName = "usmallint")]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("name")]
        public string Name { get; set; }

        [Column("is_popular")]
        public bool IsPopular { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CatalogNumber> CatalogNumber { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composition> Compositions { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recording> Recordings { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composer> Composers { get; set; }
    }
}
