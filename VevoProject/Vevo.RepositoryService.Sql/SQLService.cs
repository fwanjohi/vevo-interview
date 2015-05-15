using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Vevo.Models;
using Vevo.RepositoryService;

namespace Vevo.RepositoryService
{

    /// <summary>
    /// Any that have been configred must me copied to to the /bin folder of the installation, 
    /// together with any of its deppendencies.
    /// It should not be referenced anywhere in the code. 
    /// Its value should be in web.config file as follows.
    /// 
    /// <!--type data for the repository to use;-->
    ///<add key ="RepositoryProvider" value ="Vevo.RepositoryService.Sql.dll,Vevo.RepositoryService.SqlService"/>
    
    ///<!--Any configuration data needed by the repository-->
    //<add key = "RepositoryData" value="Server=(local);Initial Catalog=vevotest;Integrated Security=SSPI"/>
    /// </summary>
    /// <returns></returns>
    public class SqlService : IRepositoryService
    {
        /*
         * ALSO NOTE, I USED A DATA READER AND ADO FEATURES. THIS COULD ALSO HAVE BEEN DONE WITH ANY
         * DATA PROVIDER OR FRAMEWORK.
         * */
        public string ConfigData { get; set; }

        public Artist GetArtistById(string id)
        {
            Artist artist = null;

            using (SqlConnection con = new SqlConnection(this.ConfigData))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("GetArtistByUrlSafeName", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@urlSafeName", SqlDbType.VarChar).Value = id;

                        con.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            artist = ArtistFromReader(reader);
                            break;
                        }
                        reader.Close();

                    }
                }
                finally
                {
                    con.Close();
                }
            }

            return artist;
        }

        public List<Artist> ListArtists()
        {
            List<Artist> items = new List<Artist>();


            using (SqlConnection con = new SqlConnection(this.ConfigData))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("ListArtists", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            items.Add(ArtistFromReader(reader));
                        }
                        reader.Close();
                    }
                }
                finally
                {
                    con.Close();
                }

            }

            return items;

        }

        public void DeleteArtist(string id)
        {
            using (SqlConnection con = new SqlConnection(this.ConfigData))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteArtistByUrlSafeName", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@urlSafeName", SqlDbType.VarChar).Value = id;

                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public Artist UpdateArtist(Artist artist)
        {
            //UpdateArtistById(@artistId int, @newName nvarchar(50), @newUrlSafeName nvarchar(100))
            Artist savedArtist = null;
            using (SqlConnection con = new SqlConnection(this.ConfigData))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateArtistById", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@artistId", SqlDbType.Int).Value = artist.internalId;
                        cmd.Parameters.Add("@newName", SqlDbType.VarChar).Value = artist.name;
                        cmd.Parameters.Add("@newUrlSafeName", SqlDbType.VarChar).Value = artist.urlSafeName;

                        con.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            savedArtist = ArtistFromReader(reader);
                            break;
                        }
                        reader.Close();

                    }
                }
                finally
                {
                    con.Close();
                }
            }

            return savedArtist;
        }

        public Artist AddArtist(Artist artist)
        {
            Artist savedArtist = null;
            using (SqlConnection con = new SqlConnection(this.ConfigData))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("AddArtist", con))
                    {

                        //@artistName nvarchar(50), @urlSafeName nvarchar(100)
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@artistName", SqlDbType.VarChar).Value = artist.name;
                        cmd.Parameters.Add("@urlSafeName", SqlDbType.VarChar).Value = artist.urlSafeName;

                        con.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            savedArtist = ArtistFromReader(reader);
                            break;
                        }
                        reader.Close();

                    }
                }
                finally
                {
                    con.Close();
                }
            }

            return savedArtist;
        }

        public List<Video> ListArtistVideos(string artistUrlSafeName)
        {
            List<Video> items = new List<Video>();


            using (SqlConnection con = new SqlConnection(this.ConfigData))
            {
                try
                {
                    //BOLD-shakira
                    using (SqlCommand cmd = new SqlCommand("GetVideosByArtistUrlSafeName", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@urlSafeName", SqlDbType.VarChar).Value = artistUrlSafeName;

                        con.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var video = new Video
                            {
                                isrc = reader["ISRC"].ToString(),
                                urlSafeTitle = reader["UrlSafeTitle"].ToString(),
                                title = reader["Title"].ToString()

                            };

                            items.Add(video);


                        }
                        reader.Close();
                    }
                }
                finally
                {
                    con.Close();
                }

            }

            return items;

        }

        public Video GetVideo(string isrc)
        {
            Video video = null;


            using (SqlConnection con = new SqlConnection(this.ConfigData))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("GetVideo", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@isrc", SqlDbType.VarChar).Value = isrc;

                        con.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                             video = new Video
                            {
                                isrc = reader["ISRC"].ToString(),
                                urlSafeTitle = reader["UrlSafeTitle"].ToString(),
                                title = reader["Title"].ToString()

                            };
                            break;


                        }
                        reader.Close();
                    }
                }
                finally
                {
                    con.Close();
                }

            }

            return video;

        }

        private Artist ArtistFromReader(SqlDataReader reader)
        {
            Artist artist = null;

            try
            {
                artist = new Artist();
                artist.internalId = reader["artistId"].ToString();
                artist.name = reader["Name"].ToString();
                artist.urlSafeName = reader["UrlSafeName"].ToString();
            }
            catch (Exception)
            {
                
                throw;
            }

            return artist;
        }


    }
}
