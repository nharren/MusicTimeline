namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MovementAudioFilePerformer")]
    public partial class MovementAudioFilePerformer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public long PerformerID { get; set; }

        public long MovementAudioFileID { get; set; }

        public virtual MovementAudioFile MovementAudioFile { get; set; }

        public virtual Performer Performer { get; set; }
    }
}