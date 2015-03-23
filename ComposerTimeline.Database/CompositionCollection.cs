namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompositionCollection")]
    public partial class CompositionCollection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompositionCollection()
        {
            Compositions = new HashSet<Composition>();
            CompositionCollectionAudioFiles = new HashSet<CompositionCollectionAudioFile>();
            CompositionCollectionCatalogNumbers = new HashSet<CompositionCollectionCatalogNumber>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string CommonName { get; set; }

        [StringLength(2147483647)]
        public string Nickname { get; set; }

        public long TotalCompositions { get; set; }

        public long IsPopular { get; set; }

        public long ComposerID { get; set; }

        public virtual Composer Composer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composition> Compositions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCollectionAudioFile> CompositionCollectionAudioFiles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCollectionCatalogNumber> CompositionCollectionCatalogNumbers { get; set; }
    }
}