namespace NathanHarrenstein.MusicDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("music_test.composer_link")]
    public partial class ComposerLink
    {
        [Column("id", TypeName = "usmallint")]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        [Column("url")]
        public string URL { get; set; }

        [Column("composer_id", TypeName = "usmallint")]
        public int ComposerID { get; set; }

        public virtual Composer Composer { get; set; }
    }
}
