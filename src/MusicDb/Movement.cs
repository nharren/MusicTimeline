namespace NathanHarrenstein.MusicDB
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Movement")]
    public partial class Movement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Movement()
        {
            Recordings = new ObservableCollection<Recording>();
        }

        
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public short Number { get; set; }

        public int CompositionID { get; set; }

        public bool IsPopular { get; set; }

        public virtual Composition Composition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Recording> Recordings { get; set; }
    }
}
