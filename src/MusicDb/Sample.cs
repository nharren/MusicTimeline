namespace NathanHarrenstein.MusicDB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Sample")]
    public partial class Sample
    {
        public short ID { get; set; }

        [Required]
        public byte[] Audio { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string Artists { get; set; }

        public short ComposerID { get; set; }

        public virtual Composer Composer { get; set; }
    }
}