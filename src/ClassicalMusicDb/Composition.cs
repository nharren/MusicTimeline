namespace NathanHarrenstein.ClassicalMusicDb
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Composition")]
    public partial class Composition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Composition()
        {
            Movements = new ObservableCollection<Movement>();
            CatalogNumbers = new ObservableCollection<CatalogNumber>();
            Composers = new ObservableCollection<Composer>();
            Links = new ObservableCollection<Link>();
            Recordings = new ObservableCollection<Recording>();
        }

        public int CompositionId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Dates { get; set; }

        public string Nickname { get; set; }

        public string Premiere { get; set; }

        public string Dedication { get; set; }

        public string Occasion { get; set; }

        public bool IsPopular { get; set; }

        public string Comment { get; set; }

        public int? KeyId { get; set; }

        public int? GenreId { get; set; }

        public int? InstrumentationId { get; set; }

        public int? CompositionCollectionId { get; set; }

        public virtual CompositionCollection CompositionCollection { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual Instrumentation Instrumentation { get; set; }

        public virtual Key Key { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Movement> Movements { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CatalogNumber> CatalogNumbers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composer> Composers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Link> Links { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recording> Recordings { get; set; }
    }
}
