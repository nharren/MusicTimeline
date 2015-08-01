namespace NathanHarrenstein.MusicDB
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Composition")]
    public partial class Composition
    {
        public Composition()
        {
            CatalogNumbers = new ObservableCollection<CatalogNumber>();
            Movements = new ObservableCollection<Movement>();
            Recordings = new ObservableCollection<Recording>();
            Composers = new ObservableCollection<Composer>();
        }

        public virtual ObservableCollection<CatalogNumber> CatalogNumbers { get; set; }

        public virtual ObservableCollection<Composer> Composers { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        public short? CompositionCollectionID { get; set; }

        [StringLength(50)]
        public string Dates { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public bool IsPopular { get; set; }

        public virtual ObservableCollection<Movement> Movements { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Nickname { get; set; }

        public virtual ObservableCollection<Recording> Recordings { get; set; }
    }
}