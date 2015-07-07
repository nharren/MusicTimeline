using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NathanHarrenstein.MusicDb
{
    [Table("Movement")]
    public partial class Movement
    {
        public Movement()
        {
            Recordings = new ObservableCollection<Recording>();
        }

        public virtual Composition Composition { get; set; }

        public int CompositionID { get; set; }

        public int ID { get; set; }

        public bool IsPopular { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public short Number { get; set; }

        public virtual ObservableCollection<Recording> Recordings { get; set; }
    }
}