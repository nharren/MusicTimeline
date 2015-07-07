using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NathanHarrenstein.MusicDb
{
    [Table("Nationality")]
    public partial class Nationality
    {
        public Nationality()
        {
            Composers = new ObservableCollection<Composer>();
        }

        public virtual ObservableCollection<Composer> Composers { get; set; }

        public short ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}