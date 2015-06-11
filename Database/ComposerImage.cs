namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("music_test.composer_image")]
    public partial class ComposerImage
    {
        [Column("id", TypeName = "usmallint")]
        public int ID { get; set; }

        [Column("image", TypeName = "mediumblob")]
        [Required]
        public byte[] Image { get; set; }

        [Column("composer_id", TypeName = "usmallint")]
        public int ComposerID { get; set; }

        public virtual Composer Composer { get; set; }
    }
}
