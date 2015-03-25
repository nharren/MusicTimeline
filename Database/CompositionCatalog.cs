namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.CodeAnalysis;

    [Table("classical_music.composition_catalog")]
    public partial class CompositionCatalog
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompositionCatalog()
        {
            CompositionCatalogNumber = new HashSet<CompositionCatalogNumber>();
            CompositionCollectionCatalogNumber = new HashSet<CompositionCollectionCatalogNumber>();
        }

        [Column("id", TypeName = "usmallint")]
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        [Column("prefix")]
        public string Prefix { get; set; }

        [Column("composer_id", TypeName = "usmallint")]
        public int ComposerID { get; set; }

        public virtual Composer Composer { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCatalogNumber> CompositionCatalogNumber { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCollectionCatalogNumber> CompositionCollectionCatalogNumber { get; set; }
    }
}
