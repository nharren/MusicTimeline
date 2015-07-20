namespace NathanHarrenstein.MusicDB
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Sample")]
    public partial class Sample
    {
        public string Artists { get; set; }

        public byte[] Audio { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ID { get; set; }

        public string Title { get; set; }

        public short ComposerID { get; set; }

        public virtual Composer Composer { get; set; }
    }
}