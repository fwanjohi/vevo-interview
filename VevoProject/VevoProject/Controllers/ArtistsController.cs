using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vevo.Models;

namespace VevoProject.Controllers
{
    public class ArtistsController : ApiController
    {
        // GET api/artists/festu-wanjohi
        public HttpResponseMessage Get(string id)
        {

            try
            {
                var service = Helper.GetService();
                var artist = service.GetArtistById(id);

                if (artist != null)
                {
                    artist.videos = service.ListArtistVideos(artist.urlSafeName);
                    return this.Request.CreateResponse<Artist>(HttpStatusCode.OK, artist);
                }
                else
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artist not found");
                }
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }


        }

        //all artists
        public HttpResponseMessage Get()
        {
            try
            {
                var service = Helper.GetService();
                var list = service.ListArtists();

                return this.Request.CreateResponse<List<Artist>>(HttpStatusCode.OK, list);

            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT api/artists
        public HttpResponseMessage Put(string id,[FromBody]Artist artist)
        {
            try
            {
                var service = Helper.GetService();
                var existing = service.GetArtistById(id);

                //make sure you are updating a real artist
                if (existing != null)
                {
                    //update existing one with new data
                    existing.urlSafeName = artist.urlSafeName;
                    existing.name = artist.name;
                    var updated = service.UpdateArtist(existing);

                    return this.Request.CreateResponse<Artist>(HttpStatusCode.OK, updated);

                }
                else
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artist not found");
                }
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }


        }

        // POST api/artists/xyz
        public HttpResponseMessage Post([FromBody]Artist artist)
        {
            try
            {
                var service = Helper.GetService();
                var existing = service.GetArtistById(artist.urlSafeName);

                //make sure you the artist does not already exist
                if (existing == null)
                {

                    var added = service.AddArtist(artist);

                    return this.Request.CreateResponse<Artist>(HttpStatusCode.OK, added);

                }
                else
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artist not found");
                }
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }


        }

        // DELETE api/artists/5
        public void Delete(string id)
        {
            try
            {
                var service = Helper.GetService();

                //make sure you the artist does not already exist
                service.DeleteArtist(id);

            }
            catch (Exception ex)
            {
                this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
