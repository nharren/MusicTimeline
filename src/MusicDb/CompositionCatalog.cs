namespace NathanHarrenstein.MusicDB
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompositionCatalog")]
    public partial class CompositionCatalog
    {
        public CompositionCatalog()
        {
            CatalogNumbers = new ObservableCollection<CatalogNumber>();
        }

        public virtual ObservableCollection<CatalogNumber> CatalogNumbers { get; set; }

        public virtual Composer Composer { get; set; }

        public short ComposerID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ID { get; set; }

        [Required]
        [StringLength(10)]
        public string Prefix { get; set; }
    }
}