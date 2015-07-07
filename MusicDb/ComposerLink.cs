using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NathanHarrenstein.MusicDb
{
    [Table("ComposerLink")]
    public partial class ComposerLink
    {
        public virtual Composer Composer { get; set; }

        public short ComposerID { get; set; }

        public short ID { get; set; }

        [Required]
        [StringLength(255)]
        public string URL { get; set; }
    }
}