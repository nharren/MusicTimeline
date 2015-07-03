namespace NathanHarrenstein.MusicDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("music_test.catalog_number")]
    public partial class CatalogNumber
    {
        [Column("id", TypeName = "usmallint")]
        public int ID { get; set; }

        [Required]
        [StringLength(15)]
        [Column("number")]
        public string Number { get; set; }

        [Column("composition_catalog_id", TypeName = "usmallint")]
        public int CompositionCatalogID { get; set; }

        [Column("composition_collection_id", TypeName = "usmallint")]
        public int CompositionCollectionID { get; set; }

        [Column("composition_id", TypeName = "umediumint")]
        public int CompositionID { get; set; }

        public virtual CompositionCatalog CompositionCatalog { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        public virtual Composition Composition { get; set; }
    }
}
