using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicTimelineWebApi.Models
{
    public class Composer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dates { get; set; }
        public string Thumbnail { get; set; }
        public IEnumerable<string> Nationalities { get; set; }
        public IEnumerable<int> Eras { get; set; }
    }
}
