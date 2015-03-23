namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ComposerInfluence")]
    public partial class ComposerInfluence
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public long InfluenceID { get; set; }

        public long InfluencedID { get; set; }

        public virtual Composer Influenced { get; set; }

        public virtual Composer Influence { get; set; }
    }
}