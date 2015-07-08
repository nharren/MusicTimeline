namespace NathanHarrenstein.MusicDB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ComposerLink")]
    public partial class ComposerLink
    {
        public virtual Composer Composer { get; set; }

        public short ComposerID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ID { get; set; }

        [Required]
        [StringLength(255)]
        public string URL { get; set; }
    }
}