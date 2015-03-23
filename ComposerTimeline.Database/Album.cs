namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Album")]
    public partial class Album
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Album()
        {
            CompositionAudioFiles = new HashSet<CompositionAudioFile>();
            CompositionCollectionAudioFiles = new HashSet<CompositionCollectionAudioFile>();
            MovementAudioFiles = new HashSet<MovementAudioFile>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Name { get; set; }

        public long TotalTracks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionAudioFile> CompositionAudioFiles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCollectionAudioFile> CompositionCollectionAudioFiles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovementAudioFile> MovementAudioFiles { get; set; }
    }
}