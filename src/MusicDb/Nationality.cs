namespace NathanHarrenstein.MusicDB
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Nationality")]
    public partial class Nationality
    {
        public Nationality()
        {
            Composers = new ObservableCollection<Composer>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ObservableCollection<Composer> Composers { get; set; }
    }
}