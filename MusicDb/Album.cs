using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NathanHarrenstein.MusicDb
{
    [Table("Album")]
    public partial class Album
    {
        public Album()
        {
            Recordings = new ObservableCollection<Recording>();
        }

        public short ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual ObservableCollection<Recording> Recordings { get; set; }
    }
}