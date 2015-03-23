namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ComposerLink")]
    public partial class ComposerLink
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string URL { get; set; }

        [StringLength(2147483647)]
        public string FaviconPath { get; set; }

        public long ComposerID { get; set; }

        public virtual Composer Composer { get; set; }
    }
}