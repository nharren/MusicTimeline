namespace NathanHarrenstein.MusicDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CatalogNumber")]
    public partial class CatalogNumber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ID { get; set; }

        [Required]
        [StringLength(15)]
        public string Number { get; set; }

        public short CompositionCatalogID { get; set; }

        public short? CompositionCollectionID { get; set; }

        public int? CompositionID { get; set; }

        public virtual CompositionCatalog CompositionCatalog { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        public virtual Composition Composition { get; set; }
    }
}
