using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NathanHarrenstein.MusicDb
{
    [Table("ComposerImage")]
    public partial class ComposerImage
    {
        public virtual Composer Composer { get; set; }

        public short ComposerID { get; set; }

        public short ID { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }
}