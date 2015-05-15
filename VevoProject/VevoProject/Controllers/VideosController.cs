using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vevo.Models;

namespace VevoProject.Controllers
{
    public class VideosController : ApiController
    {
       

        // GET api/videos/5
        public    HttpResponseMessage Get(string id)
        { 
            try
            {
                var service = Helper.GetService();
                var video = service.GetVideo(id);

                if (video != null)
                {
                    return this.Request.CreateResponse<Video>(HttpStatusCode.OK, video);
                }
                else
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Video not found");
                }
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }


        }

        
    }
}
