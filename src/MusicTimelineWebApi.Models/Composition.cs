using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicTimelineWebApi.Models
{
    public class Composition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dates { get; set; }
        public string Key { get; set; }
        public IEnumerable<string> CatalogNumbers { get; set; }
    }
}
