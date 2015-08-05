namespace NathanHarrenstein.MusicDB
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompositionCatalog")]
    public partial class CompositionCatalog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompositionCatalog()
        {
            CatalogNumbers = new ObservableCollection<CatalogNumber>();
        }

        public short ID { get; set; }

        [Required]
        [StringLength(10)]
        public string Prefix { get; set; }

        public short ComposerID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<CatalogNumber> CatalogNumbers { get; set; }

        public virtual Composer Composer { get; set; }
    }
}