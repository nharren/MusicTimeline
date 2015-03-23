namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompositionAudioFilePerformer")]
    public partial class CompositionAudioFilePerformer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public long PerformerID { get; set; }

        public long CompositionAudioFileID { get; set; }

        public virtual CompositionAudioFile CompositionAudioFile { get; set; }

        public virtual Performer Performer { get; set; }
    }
}