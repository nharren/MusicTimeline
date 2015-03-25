namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("classical_music.composition_collection_catalog_number")]
    public partial class CompositionCollectionCatalogNumber
    {
        [Column("id", TypeName = "usmallint")]
        public int ID { get; set; }

        [Required]
        [StringLength(15)]
        [Column("catalog_number")]
        public string CatalogNumber { get; set; }

        [Column("composition_catalog_id", TypeName = "usmallint")]
        public int CompositionCatalogID { get; set; }

        [Column("composition_collection_id", TypeName = "usmallint")]
        public int CompositionCollectionID { get; set; }

        public virtual CompositionCatalog CompositionCatalog { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }
    }
}
