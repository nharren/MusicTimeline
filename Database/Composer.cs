using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    [Table("music_test.composer")]
    public partial class Composer
    {
        public Composer()
        {
            ComposerImages = new ObservableCollection<ComposerImage>();
            ComposerLinks = new HashSet<ComposerLink>();
            CompositionCatalogs = new HashSet<CompositionCatalog>();
            Eras = new HashSet<Era>();
            Influences = new HashSet<Composer>();
            Influenced = new HashSet<Composer>();
            Nationalities = new HashSet<Nationality>();
            CompositionCollections = new HashSet<CompositionCollection>();
            Compositions = new HashSet<Composition>();
        }

        [Column("id", TypeName = "usmallint")]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Column("dates")]
        public string Dates { get; set; }

        [Column("birth_location_id", TypeName = "umediumint")]
        public int? BirthLocationID { get; set; }

        [Column("death_location_id", TypeName = "umediumint")]
        public int? DeathLocationID { get; set; }

        [Column("biography", TypeName = "text")]
        [StringLength(65535)]
        public string Biography { get; set; }

        [Column("is_popular")]
        public bool IsPopular { get; set; }

        public virtual Location BirthLocation { get; set; }

        public virtual Location DeathLocation { get; set; }

        public virtual ObservableCollection<ComposerImage> ComposerImages { get; set; }

        public virtual ICollection<ComposerLink> ComposerLinks { get; set; }

        public virtual ICollection<CompositionCatalog> CompositionCatalogs { get; set; }

        public virtual ICollection<Era> Eras { get; set; }

        public virtual ICollection<Composer> Influences { get; set; }

        public virtual ICollection<Composer> Influenced { get; set; }

        public virtual ICollection<Nationality> Nationalities { get; set; }

        public virtual ICollection<CompositionCollection> CompositionCollections { get; set; }

        public virtual ICollection<Composition> Compositions { get; set; }
    }
}