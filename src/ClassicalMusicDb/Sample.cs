namespace NathanHarrenstein.ClassicalMusicDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sample")]
    public partial class Sample
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Artists { get; set; }

        [Required]
        public byte[] Bytes { get; set; }

        public int ComposerID { get; set; }

        public virtual Composer Composer { get; set; }
    }
}
