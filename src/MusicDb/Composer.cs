namespace NathanHarrenstein.MusicDB
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Composer")]
    public partial class Composer
    {
        public Composer()
        {
            ComposerImages = new ObservableCollection<ComposerImage>();
            ComposerLinks = new ObservableCollection<ComposerLink>();
            CompositionCatalogs = new ObservableCollection<CompositionCatalog>();
            Eras = new ObservableCollection<Era>();
            Influences = new ObservableCollection<Composer>();
            Influenced = new ObservableCollection<Composer>();
            Nationalities = new ObservableCollection<Nationality>();
            CompositionCollections = new ObservableCollection<CompositionCollection>();
            Compositions = new ObservableCollection<Composition>();
            Samples = new ObservableCollection<Sample>();
        }

        public string Biography { get; set; }

        public int? BirthLocationID { get; set; }

        public virtual Location BirthLocation { get; set; }

        public virtual ObservableCollection<ComposerImage> ComposerImages { get; set; }

        public virtual ObservableCollection<ComposerLink> ComposerLinks { get; set; }

        public virtual ObservableCollection<CompositionCatalog> CompositionCatalogs { get; set; }

        public virtual ObservableCollection<CompositionCollection> CompositionCollections { get; set; }

        public virtual ObservableCollection<Composition> Compositions { get; set; }

        public virtual ObservableCollection<Sample> Samples { get; set; }

        [Required]
        [StringLength(50)]
        public string Dates { get; set; }

        public int? DeathLocationID { get; set; }

        public virtual Location DeathLocation { get; set; }

        public virtual ObservableCollection<Era> Eras { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ID { get; set; }

        public virtual ObservableCollection<Composer> Influenced { get; set; }

        public virtual ObservableCollection<Composer> Influences { get; set; }

        public bool IsPopular { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ObservableCollection<Nationality> Nationalities { get; set; }
    }
}