namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.CodeAnalysis;

    [Table("music_test.era")]
    public partial class Era
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Era()
        {
            Composers = new HashSet<Composer>();
        }

        [Column("id")]
        public byte ID { get; set; }

        [Required]
        [StringLength(12)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [StringLength(9)]
        [Column("dates")]
        public string Dates { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composer> Composers { get; set; }
    }
}
