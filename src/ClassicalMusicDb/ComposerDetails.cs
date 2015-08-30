namespace NathanHarrenstein.ClassicalMusicDb
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Composer")]
    public partial class ComposerDetails
    {
        [Key]
        public int ComposerId { get; set; }

        public int? BirthLocationId { get; set; }

        public int? DeathLocationId { get; set; }

        public string Biography { get; set; }

        public bool IsPopular { get; set; }

        public virtual Location BirthLocation { get; set; }

        public virtual Location DeathLocation { get; set; }

        public virtual Composer Composer { get; set; }
    }
}
