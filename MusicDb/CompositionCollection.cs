using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NathanHarrenstein.MusicDb
{
    [Table("CompositionCollection")]
    public partial class CompositionCollection
    {
        public CompositionCollection()
        {
            CatalogNumbers = new ObservableCollection<CatalogNumber>();
            Compositions = new ObservableCollection<Composition>();
            Recordings = new ObservableCollection<Recording>();
            Composers = new ObservableCollection<Composer>();
        }

        public virtual ObservableCollection<CatalogNumber> CatalogNumbers { get; set; }

        public virtual ObservableCollection<Composer> Composers { get; set; }

        public virtual ObservableCollection<Composition> Compositions { get; set; }

        public short ID { get; set; }

        public bool IsPopular { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ObservableCollection<Recording> Recordings { get; set; }
    }
}