namespace NathanHarrenstein.MusicDB
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Era")]
    public partial class Era
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Era()
        {
            Composers = new ObservableCollection<Composer>();
        }

        
        public short ID { get; set; }

        [Required]
        [StringLength(12)]
        public string Name { get; set; }

        [Required]
        [StringLength(9)]
        public string Dates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Composer> Composers { get; set; }
    }
}
