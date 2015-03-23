namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompositionCatalog")]
    public partial class CompositionCatalog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompositionCatalog()
        {
            CompositionCatalogNumbers = new HashSet<CompositionCatalogNumber>();
            CompositionCollectionCatalogNumbers = new HashSet<CompositionCollectionCatalogNumber>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Name { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Prefix { get; set; }

        public long ComposerID { get; set; }

        public virtual Composer Composer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCatalogNumber> CompositionCatalogNumbers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCollectionCatalogNumber> CompositionCollectionCatalogNumbers { get; set; }
    }
}