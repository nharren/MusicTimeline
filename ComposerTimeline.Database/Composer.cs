namespace NathanHarrenstein.ComposerTimeline.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Composer")]
    public partial class Composer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Composer()
        {
            ComposerImages = new HashSet<ComposerImage>();
            ComposersInfluenced = new HashSet<ComposerInfluence>();
            ComposerInfluences = new HashSet<ComposerInfluence>();
            ComposerLinks = new HashSet<ComposerLink>();
            Compositions = new HashSet<Composition>();
            CompositionCatalogs = new HashSet<CompositionCatalog>();
            CompositionCollections = new HashSet<CompositionCollection>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Name { get; set; }

        public long BirthYear { get; set; }

        public long? DeathYear { get; set; }

        public long? BirthPlaceID { get; set; }

        public long? DeathPlaceID { get; set; }

        public long? NationalityID { get; set; }

        public long EraID { get; set; }

        [StringLength(2147483647)]
        public string Biography { get; set; }

        public long IsPopular { get; set; }

        public virtual Era Era { get; set; }

        public virtual Nationality Nationality { get; set; }

        public virtual Location DeathPlace { get; set; }

        public virtual Location BirthPlace { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComposerImage> ComposerImages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComposerInfluence> ComposersInfluenced { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComposerInfluence> ComposerInfluences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComposerLink> ComposerLinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composition> Compositions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCatalog> CompositionCatalogs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompositionCollection> CompositionCollections { get; set; }
    }
}