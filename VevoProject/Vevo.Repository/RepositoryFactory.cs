using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Vevo.Models;
using Vevo.RepositoryService;

namespace Vevo.RepositoryService
{
   

    public  class RepositoryFactory
    {
      /// <summary>
      /// Returns a repositoryService from the webconfig file
      /// </summary>
      /// <param name="fileLocation">the actual physical path where the file is located</param>
      /// <returns></returns>
        public static IRepositoryService Create(string dllPath)
        {

            string info = ConfigurationManager.AppSettings["RepositoryProvider"];
            string configData = ConfigurationManager.AppSettings["RepositoryData"];

            try
            {
                string[] items = info.Split(new char[] { ',' });
                string dll = items[0];
                string className = items[1];

                string fullPath = Path.Combine(dllPath, "bin", dll);

                if (!File.Exists(fullPath))
                {

                    throw new Exception(string.Format("service Provider not well configured : File {0} not found",
                        fullPath));
                }

                var asm = Assembly.LoadFrom(fullPath);
                var type = asm.GetType(className);

                IRepositoryService provider = Activator.CreateInstance(type) as IRepositoryService;
                provider.ConfigData = configData;

                //TODO : ANY OTHER CONFIGURATION DEEMED NECESSARY

                return provider;
            }
            catch (Exception ex)
            {
                throw;
                //_logger.ErrorFormat("Reflection Error. Dll can not be loaded because ", ex.Message, ex);
                //throw new ScimProvisionServiceException(string.Format("Provision Provider not well configured : {0} ", ex.Message));
            }
        }
    }
}
