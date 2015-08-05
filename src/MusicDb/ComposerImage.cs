namespace NathanHarrenstein.MusicDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ComposerImage")]
    public partial class ComposerImage
    {
        
        public short ID { get; set; }

        public short ComposerID { get; set; }

        [Required]
        public byte[] Image { get; set; }

        public virtual Composer Composer { get; set; }
    }
}
