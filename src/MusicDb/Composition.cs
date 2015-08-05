namespace NathanHarrenstein.MusicDB
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Composition")]
    public partial class Composition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Composition()
        {
            CatalogNumbers = new ObservableCollection<CatalogNumber>();
            CompositionLinks = new ObservableCollection<CompositionLink>();
            Movements = new ObservableCollection<Movement>();
            Recordings = new ObservableCollection<Recording>();
            Composers = new ObservableCollection<Composer>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Dates { get; set; }

        [StringLength(50)]
        public string Nickname { get; set; }

        public bool IsPopular { get; set; }

        public short? CompositionCollectionID { get; set; }

        public string Comment { get; set; }

        [StringLength(255)]
        public string Premiere { get; set; }

        [StringLength(255)]
        public string Dedication { get; set; }

        [StringLength(255)]
        public string Occasion { get; set; }

        public short? CompositionTypeID { get; set; }

        public short? KeyID { get; set; }

        public short? InstrumentationID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<CatalogNumber> CatalogNumbers { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        public virtual CompositionType CompositionType { get; set; }

        public virtual Instrumentation Instrumentation { get; set; }

        public virtual Key Key { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<CompositionLink> CompositionLinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Movement> Movements { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Recording> Recordings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Composer> Composers { get; set; }
    }
}