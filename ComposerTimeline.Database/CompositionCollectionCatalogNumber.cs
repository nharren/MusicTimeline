namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompositionCollectionCatalogNumber")]
    public partial class CompositionCollectionCatalogNumber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public long Value { get; set; }

        public long CompositionCatalogID { get; set; }

        public long CompositionCollectionID { get; set; }

        public virtual CompositionCatalog CompositionCatalog { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }
    }
}