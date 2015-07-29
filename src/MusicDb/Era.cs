namespace NathanHarrenstein.MusicDB
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Era")]
    public partial class Era : ICloneable
    {
        public Era()
        {
            Composers = new ObservableCollection<Composer>();
        }

        public virtual ObservableCollection<Composer> Composers { get; set; }

        [Required]
        [StringLength(9)]
        public string Dates { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ID { get; set; }

        [Required]
        [StringLength(12)]
        public string Name { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}