namespace NathanHarrenstein.MusicDb
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;

    [Table("music_test.recording")]
    public partial class Recording
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recording()
        {
            Performers = new HashSet<Performer>();
            Locations = new ObservableCollection<Location>();
        }

        [Column("id", TypeName = "umediumint")]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("dates")]
        public string Dates { get; set; }

        [Column("album_id", TypeName = "usmallint")]
        public int? AlbumID { get; set; }

        [Column("track_number")]
        public byte? TrackNumber { get; set; }

        [Column("composition_collection_id", TypeName = "usmallint")]
        public int? CompositionCollectionID { get; set; }

        [Column("composition_id", TypeName = "umediumint")]
        public int? CompositionID { get; set; }

        [Column("movement_id", TypeName = "umediumint")]
        public int? MovementID { get; set; }

        public virtual Album Album { get; set; }

        public virtual Composition Composition { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        public virtual Movement Movement { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Performer> Performers { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Location> Locations { get; set; }
    }
}