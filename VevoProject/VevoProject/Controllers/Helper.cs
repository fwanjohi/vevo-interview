using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using Vevo.RepositoryService;

namespace VevoProject.Controllers
{
    public class Helper
    {
        /// <summary>
        /// Any that have been configred must me copied to to the /bin folder of the installation, 
        /// together with any of its deppendencies.
        /// It should not be referenced anywhere in the code. 
        /// </summary>
        /// <returns></returns>
        public static IRepositoryService GetService()
        {
            var serverPath = HttpContext.Current.Server.MapPath("");

            int pos = serverPath.IndexOf("api");

            string rootPath = serverPath.Substring(0, pos);

            var service = RepositoryFactory.Create(rootPath);
            return service;
        }

    }
}