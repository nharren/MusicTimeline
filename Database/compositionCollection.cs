namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.CodeAnalysis;

    [Table("classical_music.composition_collection")]
    public partial class CompositionCollection
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompositionCollection()
        {
            Compositions = new HashSet<Composition>();
            CompositionCollectionCatalogNumber = new HashSet<CompositionCollectionCatalogNumber>();
            CompositionCollectionRecording = new HashSet<CompositionCollectionRecording>();
        }

        [Column("id", TypeName = "usmallint")]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("name")]
        public string Name { get; set; }

        [Column("is_popular")]
        public bool IsPopular { get; set; }

        [Column("composer_id", TypeName = "usmallint")]
        public int ComposerID { get; set; }

        public virtual Composer Composer { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composition> Compositions { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCollectionCatalogNumber> CompositionCollectionCatalogNumber { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCollectionRecording> CompositionCollectionRecording { get; set; }
    }
}
