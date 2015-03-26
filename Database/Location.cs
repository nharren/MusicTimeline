namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.CodeAnalysis;

    [Table("music.location")]
    public partial class Location
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            BirthLocationComposers = new HashSet<Composer>();
            DeathLocationComposers = new HashSet<Composer>();
            Recordings = new HashSet<Recording>();
        }

        [Column("id", TypeName = "umediumint")]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("name")]
        public string Name { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composer> BirthLocationComposers { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composer> DeathLocationComposers { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recording> Recordings { get; set; }
    }
}
