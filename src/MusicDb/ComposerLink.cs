namespace NathanHarrenstein.MusicDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ComposerLink")]
    public partial class ComposerLink
    {
        
        public short ID { get; set; }

        [Required]
        [StringLength(255)]
        public string URL { get; set; }

        public short ComposerID { get; set; }

        public virtual Composer Composer { get; set; }

        [StringLength(300)]
        public string Name { get; set; }
    }
}
