namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompositionCatalogNumber")]
    public partial class CompositionCatalogNumber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public long Value { get; set; }

        public long CompositionCatalogID { get; set; }

        public long CompositionID { get; set; }

        public virtual Composition Composition { get; set; }

        public virtual CompositionCatalog CompositionCatalog { get; set; }
    }
}