//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClassicalMusicDbService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sample
    {
        public int SampleId { get; set; }
        public string Title { get; set; }
        public string Artists { get; set; }
        public int ComposerID { get; set; }
    
        public virtual Composer Composer { get; set; }
    }
}
