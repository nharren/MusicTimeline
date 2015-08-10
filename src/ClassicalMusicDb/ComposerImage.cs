namespace NathanHarrenstein.ClassicalMusicDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ComposerImage")]
    public partial class ComposerImage
    {
        public int Id { get; set; }

        [Required]
        public byte[] Bytes { get; set; }

        public int ComposerId { get; set; }

        public virtual Composer Composer { get; set; }
    }
}
