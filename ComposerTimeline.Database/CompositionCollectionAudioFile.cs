namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompositionCollectionAudioFile")]
    public partial class CompositionCollectionAudioFile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompositionCollectionAudioFile()
        {
            CompositionCollectionAudioFilePerformers = new HashSet<CompositionCollectionAudioFilePerformer>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Path { get; set; }

        [StringLength(2147483647)]
        public string RecordingCompletionDate { get; set; }

        public long? RecordingLocationID { get; set; }

        public long? AlbumID { get; set; }

        public long? TrackNumber { get; set; }

        public long CompositionCollectionID { get; set; }

        public virtual Album Album { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        public virtual Location Location { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCollectionAudioFilePerformer> CompositionCollectionAudioFilePerformers { get; set; }
    }
}