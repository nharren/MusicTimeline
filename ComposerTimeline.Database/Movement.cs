namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Movement")]
    public partial class Movement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Movement()
        {
            MovementAudioFiles = new HashSet<MovementAudioFile>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string CommonName { get; set; }

        public long Number { get; set; }

        public long? KeyID { get; set; }

        public long CompositionID { get; set; }

        public long IsPopular { get; set; }

        public virtual Composition Composition { get; set; }

        public virtual Key Key { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovementAudioFile> MovementAudioFiles { get; set; }
    }
}