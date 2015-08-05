namespace NathanHarrenstein.MusicDB
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Recording")]
    public partial class Recording
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recording()
        {
            Locations = new ObservableCollection<Location>();
            Performers = new ObservableCollection<Performer>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Dates { get; set; }

        public short? AlbumID { get; set; }

        public short? TrackNumber { get; set; }

        public short? CompositionCollectionID { get; set; }

        public int? CompositionID { get; set; }

        public int? MovementID { get; set; }

        public virtual Album Album { get; set; }

        public virtual Composition Composition { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        public virtual Movement Movement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Location> Locations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Performer> Performers { get; set; }
    }
}