using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    [Table("music_test.composition_collection")]
    public partial class CompositionCollection
    {
        public CompositionCollection()
        {
            CatalogNumbers = new HashSet<CatalogNumber>();
            Compositions = new HashSet<Composition>();
            Recordings = new HashSet<Recording>();
            Composers = new HashSet<Composer>();
        }

        public virtual ICollection<CatalogNumber> CatalogNumbers { get; set; }
        public virtual ICollection<Composer> Composers { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }

        [Column("id", TypeName = "usmallint")]
        public int ID { get; set; }

        [Column("is_popular")]
        public bool IsPopular { get; set; }

        [Required]
        [StringLength(50)]
        [Column("name")]
        public string Name { get; set; }

        public virtual ICollection<Recording> Recordings { get; set; }
    }
}
