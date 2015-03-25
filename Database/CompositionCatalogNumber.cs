namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("classical_music.composition_catalog_number")]
    public partial class CompositionCatalogNumber
    {
        [Column("id", TypeName = "umediumint")]
        public int ID { get; set; }

        [Required]
        [StringLength(15)]
        [Column("catalog_number")]
        public string CatalogNumber { get; set; }

        [Column("composition_catalog_id", TypeName = "usmallint")]
        public int CompositionCatalogID { get; set; }

        [Column("composition_id", TypeName = "umediumint")]
        public int CompositionID { get; set; }

        public virtual Composition Composition { get; set; }

        public virtual CompositionCatalog CompositionCatalog { get; set; }
    }
}
