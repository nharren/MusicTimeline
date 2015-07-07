using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NathanHarrenstein.MusicDb
{
    [Table("Era")]
    public partial class Era
    {
        public Era()
        {
            Composers = new ObservableCollection<Composer>();
        }

        public virtual ObservableCollection<Composer> Composers { get; set; }

        [Required]
        [StringLength(9)]
        public string Dates { get; set; }

        public short ID { get; set; }

        [Required]
        [StringLength(12)]
        public string Name { get; set; }
    }
}