namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.CodeAnalysis;

    [Table("music.composition")]
    public partial class Composition
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Composition()
        {
            CatalogNumbers = new HashSet<CatalogNumber>();
            Movements = new HashSet<Movement>();
            Recordings = new HashSet<Recording>();
            Composers = new HashSet<Composer>();
        }

        [Column("id", TypeName = "umediumint")]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Column("dates")]
        public string Dates { get; set; }

        [StringLength(50)]
        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("is_popular")]
        public bool IsPopular { get; set; }

        [Column("composition_collection_id", TypeName = "usmallint")]
        public int CompositionCollectionID { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CatalogNumber> CatalogNumbers { get; set; }

        public virtual CompositionCollection CompositionCollections { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Movement> Movements { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recording> Recordings { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composer> Composers { get; set; }
    }
}
