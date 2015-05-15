using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Vevo.Models
{
    public class Artist
    {
        [JsonIgnore]
        public string internalId { get; set; }

        public string name { get; set; }
        public string urlSafeName { get; set; }

        public List<Video> videos { get; set; }
 
    }

    public class Video
    {
        public string isrc { get; set; }
        public string urlSafeTitle { get; set; }
        public string title { get; set; }
    }



}
