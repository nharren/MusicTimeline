using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicTimelineWebApi.Models
{
    public class ComposerDetail : Composer
    {
        public string BirthLocation { get; set; }
        public string DeathLocation { get; set; }
        public IEnumerable<Composition> Compositions { get; set; }
        public string Biography { get; set; }
        public IEnumerable<string> Images { get; set; }
        public IEnumerable<Composer> Influences { get; set; }
        public IEnumerable<Composer> Influenced { get; set; }
        public IEnumerable<Sample> Samples { get; set; }
        public IEnumerable<string> Links { get; set; }
    }
}
