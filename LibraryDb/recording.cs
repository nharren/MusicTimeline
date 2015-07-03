using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NathanHarrenstein.LibraryDb
{
    [Table("recording")]
    public partial class Recording
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int ID { get; set; }

        [Column("mdbid")]
        public int MDBID { get; set; }

        [Column("path", TypeName = "text")]
        [Required]
        public string Path { get; set; }
    }
}