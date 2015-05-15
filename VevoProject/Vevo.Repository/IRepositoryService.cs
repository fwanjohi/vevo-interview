using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vevo.Models;

namespace Vevo.RepositoryService
{

    /// <summary>
    /// THIS IS THE CONTRACT THAT ANY DATA ACCESS PROVIDER SHOULD IMLEMENT
    /// </summary>
    public interface IRepositoryService
    {
        Models.Artist GetArtistById(string id);
        List<Artist> ListArtists();
        void DeleteArtist(string id);
        Artist UpdateArtist(Artist artist);
        Artist AddArtist(Artist artist);

        List<Video> ListArtistVideos(string artistUrlSafeName);

        Video GetVideo(string isrc);

        /// <summary>
        /// This string store whatever information the repository is going to use
        /// </summary>
        string ConfigData { get; set; }

    }
}
