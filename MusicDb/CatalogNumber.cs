using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NathanHarrenstein.MusicDb
{
    [Table("CatalogNumber")]
    public partial class CatalogNumber
    {
        public virtual Composition Composition { get; set; }

        public virtual CompositionCatalog CompositionCatalog { get; set; }

        public short CompositionCatalogID { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        public short? CompositionCollectionID { get; set; }

        public int? CompositionID { get; set; }

        public short ID { get; set; }

        [Required]
        [StringLength(15)]
        public string Number { get; set; }
    }
}