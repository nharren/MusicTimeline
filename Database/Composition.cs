namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.CodeAnalysis;

    [Table("classical_music.composition")]
    public partial class Composition
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Composition()
        {
            CompositionCatalogNumber = new HashSet<CompositionCatalogNumber>();
            CompositionRecording = new HashSet<CompositionRecording>();
            Movements = new HashSet<Movement>();
        }

        [Column("ID", TypeName = "umediumint")]
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

        [Column("composer_id", TypeName = "usmallint")]
        public int ComposerId { get; set; }

        [Column("composition_collection_id", TypeName = "usmallint")]
        public int CompositionCollectionID { get; set; }

        public virtual Composer Composer { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCatalogNumber> CompositionCatalogNumber { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionRecording> CompositionRecording { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Movement> Movements { get; set; }
    }
}
