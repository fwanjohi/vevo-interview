using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vevo.Models;
using VevoProject.Controllers;

namespace VevoProject.Tests.Controllers
{
    [TestClass]
    public class VevoUnitTests
    {
        [TestMethod]
        public void Get()
        {
            HttpClient client = GetClient();
            HttpResponseMessage response =
                client.GetAsync("artists").Result;

            var result = response.Content.ReadAsAsync<List<Artist>>().Result;

            Assert.IsNotNull(result);

            Assert.IsTrue(result.Count > 1, "After Imprting SampleData, there should be values");

        }

        [TestMethod]
        public void GetSpecificArtist()
        {
            HttpClient client = GetClient();

            //Get a known artist
            HttpResponseMessage response =
                client.GetAsync("artists/shakira").Result;

            var result = response.Content.ReadAsAsync<Artist>().Result;

            Assert.IsNotNull(result, "shakira should exist");

            //get a fake artist

            HttpResponseMessage negTest =
               client.GetAsync("artists/877rtwr8").Result;

            result = response.Content.ReadAsAsync<Artist>().Result;

            Assert.IsNull(result, "877rtwr8 should NOT exist");


        }

        [TestMethod]
        public void GetSpecificArtistWithVideos()
        {
            HttpClient client = GetClient();

            HttpResponseMessage response =
                client.GetAsync("artists/shakira").Result;

            var result = response.Content.ReadAsAsync<Artist>().Result;

            Assert.IsNotNull(result, "shakira should exist");

            Assert.IsNotNull(result.videos, "SpecificArtist shoult have videos");

            Assert.IsNotNull(result.videos.Count > 1, "Shakira shoult have videos");


        }

        [TestMethod]
        public void GetSpecificVideos()
        {
            HttpClient client = GetClient();

            //get a real video
            HttpResponseMessage response =
                client.GetAsync("videos/summer").Result;

            var result = response.Content.ReadAsAsync<Artist>().Result;

            Assert.IsNotNull(result, "summer should exist");

            //get a fake video
            HttpResponseMessage fakeResponse =
                client.GetAsync("videos/summer-of-200000").Result;

            Assert.IsTrue(fakeResponse.StatusCode == HttpStatusCode.NotFound, "should return Not Found");

            var fake = response.Content.ReadAsAsync<Artist>().Result;
            Assert.IsNull(fake, "summer-of-2000 should  not");



        }



        [TestMethod]
        public void Post()
        {
            var artist = new Artist
            {
                name = "postTest",
                urlSafeName = "PostTest-" + DateTime.Now.Ticks
            };

            string testUrl = artist.urlSafeName;


            var client = GetClient();

            //Uri uri = new Uri(url);

            HttpResponseMessage response;

            response = client.PostAsJsonAsync("artists", artist).Result;

            var posted = response.Content.ReadAsAsync<Artist>().Result;

            HttpResponseMessage postResponse =
                client.GetAsync("artists/" + posted.urlSafeName).Result;

            var existing = postResponse.Content.ReadAsAsync<Artist>().Result;

            Assert.IsNotNull(existing, string.Format("{0} should exist as new artist", testUrl));



        }

        [TestMethod]
        public void Put()
        {
            var artist = new Artist
            {
                name = "putTest",
                urlSafeName = "PutTest" + DateTime.Now.Ticks
            };

            string testUrl = artist.urlSafeName;


            var client = GetClient();

            //Uri uri = new Uri(url);

            HttpResponseMessage response;

            response = client.PostAsJsonAsync("artists", artist).Result;

            var posted = response.Content.ReadAsAsync<Artist>().Result;

            HttpResponseMessage postResponse =
                client.GetAsync("artists/" + testUrl).Result;

            var existing = postResponse.Content.ReadAsAsync<Artist>().Result;

            Assert.IsNotNull(existing, string.Format("{0} should exist as new artist", testUrl));


            //Now Change
            existing.urlSafeName = existing.urlSafeName + "-CHANGED";
            existing.name = existing.name + "-CHANGED";
            response = client.PutAsJsonAsync("artists/" + testUrl, existing).Result;

            var changed = response.Content.ReadAsAsync<Artist>().Result;
            
            Assert.AreNotEqual(changed.urlSafeName, posted.urlSafeName, "artist urlSafeName did not change");
            Assert.AreNotEqual(changed.name , posted.name, "artist name did not change");

            HttpResponseMessage changedRespose =
                client.GetAsync("artists/" + existing.urlSafeName).Result;

            var reallyChanged = postResponse.Content.ReadAsAsync<Artist>().Result;

            Assert.AreNotEqual(changed.urlSafeName, posted.urlSafeName, "artist urlSafeName did not change in database");
            Assert.AreNotEqual(changed.name, posted.name, "artist name did not change in database");



        }

        [TestMethod]
        public void Delete()
        {

            var artist = new Artist
            {
                name = "DeleteTest",
                urlSafeName = "DeleteTest" + DateTime.Now.Ticks
            };

            string testUrl = artist.urlSafeName;


            var client = GetClient();

            HttpResponseMessage response;

            response = client.PostAsJsonAsync("artists", artist).Result;

            var posted = response.Content.ReadAsAsync<Artist>().Result;

            //make sure created ok
            HttpResponseMessage postResponse =
                client.GetAsync("artists/" + testUrl).Result;

            var existing = postResponse.Content.ReadAsAsync<Artist>().Result;

            Assert.IsNotNull(existing, string.Format("{0} should exist as new artist", testUrl));

            //Delete the artist
            HttpResponseMessage deleteResponser =
                client.DeleteAsync("artists/" + testUrl).Result;

            var deleted = deleteResponser.Content.ReadAsAsync<Artist>().Result;

            Assert.IsTrue(deleteResponser.StatusCode == HttpStatusCode.NoContent);


            //Get the detede artist. Should not exist
            HttpResponseMessage confirmResponse =
                client.GetAsync("artists/" + testUrl).Result;

            var confirmDeleted = deleteResponser.Content.ReadAsAsync<Artist>().Result;

            Assert.IsTrue(confirmResponse.StatusCode == HttpStatusCode.NotFound);


        }

        public void CreateTestData()
        {


        }


        public HttpClient GetClient()
        {
            //http://localhost:38311/api/videos/MSRTEST

            string baseUrl = "http://localhost:38311/api/";
            var webClient = new HttpClient();
            webClient.BaseAddress = new Uri(baseUrl);

            ServicePointManager.ServerCertificateValidationCallback =
                (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) =>
                {
                    return true;
                };


            return webClient;
        }



    }
}
