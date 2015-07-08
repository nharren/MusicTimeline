namespace NathanHarrenstein.MusicDB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ComposerImage")]
    public partial class ComposerImage
    {
        public virtual Composer Composer { get; set; }

        public short ComposerID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ID { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }
}