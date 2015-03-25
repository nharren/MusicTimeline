namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.CodeAnalysis;

    [Table("classical_music.movement")]
    public partial class Movement
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Movement()
        {
            MovementRecordings = new HashSet<MovementRecording>();
        }

        [Column("id", TypeName = "umediumint")]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; }

        [Column("number")]
        public byte Number { get; set; }

        [Column("composition_id", TypeName = "umediumint")]
        public int CompositionID { get; set; }

        [Column("is_popular")]
        public bool IsPopular { get; set; }

        public virtual Composition Composition { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovementRecording> MovementRecordings { get; set; }
    }
}
