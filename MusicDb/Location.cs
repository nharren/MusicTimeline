namespace NathanHarrenstein.MusicDB
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Location")]
    public partial class Location
    {
        public Location()
        {
            BirthLocationComposers = new ObservableCollection<Composer>();
            DeathLocationComposers = new ObservableCollection<Composer>();
            Recordings = new ObservableCollection<Recording>();
        }

        public virtual ObservableCollection<Composer> BirthLocationComposers { get; set; }

        public virtual ObservableCollection<Composer> DeathLocationComposers { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ObservableCollection<Recording> Recordings { get; set; }
    }
}