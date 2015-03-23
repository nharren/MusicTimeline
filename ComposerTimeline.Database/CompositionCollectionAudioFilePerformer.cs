namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompositionCollectionAudioFilePerformer")]
    public partial class CompositionCollectionAudioFilePerformer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public long PerformerID { get; set; }

        public long CompositionCollectionAudioFileID { get; set; }

        public virtual CompositionCollectionAudioFile CompositionCollectionAudioFile { get; set; }

        public virtual Performer Performer { get; set; }
    }
}