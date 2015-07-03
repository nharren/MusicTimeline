namespace LibraryDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Recording")]
    public partial class Recording
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int MDBID { get; set; }

        [Required]
        [StringLength(260)]
        public string FilePath { get; set; }
    }
}
