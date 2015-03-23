namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ComposerImage")]
    public partial class ComposerImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Path { get; set; }

        public long ComposerID { get; set; }

        public virtual Composer Composer { get; set; }
    }
}