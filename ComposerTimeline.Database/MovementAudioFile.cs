namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MovementAudioFile")]
    public partial class MovementAudioFile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MovementAudioFile()
        {
            MovementAudioFilePerformers = new HashSet<MovementAudioFilePerformer>();
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

        public long MovementID { get; set; }

        public virtual Album Album { get; set; }

        public virtual Location Location { get; set; }

        public virtual Movement Movement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovementAudioFilePerformer> MovementAudioFilePerformers { get; set; }
    }
}