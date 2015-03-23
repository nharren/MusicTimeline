namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Composition")]
    public partial class Composition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Composition()
        {
            CompositionAudioFiles = new HashSet<CompositionAudioFile>();
            CompositionCatalogNumbers = new HashSet<CompositionCatalogNumber>();
            Movements = new HashSet<Movement>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string CommonName { get; set; }

        public long? KeyID { get; set; }

        public long CompletionYear { get; set; }

        [StringLength(2147483647)]
        public string Nickname { get; set; }

        public long TotalMovements { get; set; }

        public long IsPopular { get; set; }

        public long ComposerID { get; set; }

        public long CompositionCollectionID { get; set; }

        public virtual Composer Composer { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        public virtual Key Key { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionAudioFile> CompositionAudioFiles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCatalogNumber> CompositionCatalogNumbers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Movement> Movements { get; set; }
    }
}