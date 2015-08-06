namespace NathanHarrenstein.MusicDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CompositionLink")]
    public partial class CompositionLink
    {
        
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string URL { get; set; }

        public int CompositionID { get; set; }

        public virtual Composition Composition { get; set; }

        [StringLength(300)]
        public string Name { get; set; }
    }
}
