using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NathanHarrenstein.MusicDb
{
    [Table("Recording")]
    public partial class Recording
    {
        public Recording()
        {
            Locations = new ObservableCollection<Location>();
            Performers = new ObservableCollection<Performer>();
        }

        public virtual Album Album { get; set; }

        public short? AlbumID { get; set; }

        public virtual Composition Composition { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        public short? CompositionCollectionID { get; set; }

        public int? CompositionID { get; set; }

        [Required]
        [StringLength(255)]
        public string Dates { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public virtual ObservableCollection<Location> Locations { get; set; }

        public virtual Movement Movement { get; set; }

        public int? MovementID { get; set; }

        public virtual ObservableCollection<Performer> Performers { get; set; }

        public short? TrackNumber { get; set; }
    }
}