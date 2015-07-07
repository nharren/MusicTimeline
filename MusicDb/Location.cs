using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NathanHarrenstein.MusicDb
{
    [Table("Location")]
    public partial class Location
    {
        public Location()
        {
            BirthLocationComposers = new ObservableCollection<Composer>();
            DeathLocationComposers = new ObservableCollection<Composer>();
            Recordings = new ObservableCollection<Recording>();
        }

        public virtual ObservableCollection<Composer> BirthLocationComposers { get; set; }

        public virtual ObservableCollection<Composer> DeathLocationComposers { get; set; }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ObservableCollection<Recording> Recordings { get; set; }
    }
}