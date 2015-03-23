namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompositionAudioFile")]
    public partial class CompositionAudioFile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompositionAudioFile()
        {
            CompositionAudioFilePerformers = new HashSet<CompositionAudioFilePerformer>();
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

        public long CompositionID { get; set; }

        public virtual Album Album { get; set; }

        public virtual Composition Composition { get; set; }

        public virtual Location Location { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionAudioFilePerformer> CompositionAudioFilePerformers { get; set; }
    }
}