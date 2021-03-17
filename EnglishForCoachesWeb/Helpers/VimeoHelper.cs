using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using EnglishForCoachesWeb.Database;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using EnglishForCoachesWeb.Database.Varios;

namespace EnglishForCoachesWeb.Helpers
{
    public static class VimeoHelper
    {
        private const bool test = false;
        private static readonly string domain = System.Configuration.ConfigurationManager.AppSettings["url_dominio"];
        private static readonly string emailError = "ibai@campusdeportivo.com";


        public static string UploadVideo(string videoUrl, int videoId)
        {
            String filename = "";
            string respuesta = "";
            //get the files
            try
            {
                AuthContext db = new AuthContext();
                Video video = db.Videos.Find(videoId);

                string url= domain+"/VimeoPHP/upload.php";
                if (test)
                {
                    url = "http://localhost:54252/VimeoPHP/upload.php";
                }
                respuesta = PostRequest(url,"video=" + Path.GetFileName(videoUrl) + "&tittle=" +
                                                 videoId);
                
                if (respuesta == "Video file did not exist!")
                {
                    return respuesta;
                }
                if (respuesta.Contains("error"))
                {

                    MailHelper.EnviarMail(emailError, "", "error vimeo", respuesta);
                    return respuesta;
                }
                if (respuesta.Contains("http://player.vimeo.com"))
                {
                    video.UrlVideo = respuesta.Replace("\r\n","");
                    db.Entry(video).State = EntityState.Modified;
                    db.SaveChanges();
                    GetThumbnail(respuesta,video.VideoId);
                }else
                {
                    return respuesta;
                }


            }
            catch (Exception e)
            {
                MailHelper.EnviarMail(emailError, "", "error vimeo", e.ToString());
                return e.ToString();
            }
            finally
            {
            }

            return "OK";
        }

        public static void GetThumbnail(string Url, int videoId)
        {
            string url = domain + "/VimeoPHP/videoThumbNail.php";
           
            if (test)
            {
                url = "http://localhost:54252/VimeoPHP/videoThumbNail.php";
            }
            string thumbnailResponse = PostRequest(url, "videoid=" + Url);

            if (!thumbnailResponse.Contains("Encountered an API error"))
            {
                AuthContext db = new AuthContext();
                Video video = db.Videos.Find(videoId);
                video.ThumbNail = thumbnailResponse;
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
            }

        }


        public static bool DeleteFiles(string Url)
        {
            //get the files
            try
            {
                String idVimeo = Url.Replace("http://player.vimeo.com/video/", "");
                //TODO: Do something with each uploaded file

                string url = domain + "/VimeoPHP/delete.php";
                if (test)
                {
                    url = "http://localhost:54252/VimeoPHP/delete.php";
                }
                string respuesta = PostRequest(url, "videoid=" + idVimeo);


                if (respuesta == "Video file did not exist!")
                {
                    return false;
                }
                if (respuesta.Contains("error"))
                {
                    return true;
                }
                if (respuesta.Contains("OK"))
                {
                }
            }
            catch (Exception e)
            {
                return true;
            }

            return true;
        }

        private static string PostRequest(string strURL, string strRequest)
        {

            HttpWebResponse objHttpWebResponse = null;
            UTF8Encoding encoding;
            string strResponse = "";

            HttpWebRequest objHttpWebRequest = (HttpWebRequest)WebRequest.Create(strURL);
            objHttpWebRequest.Referer = domain;
            objHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            objHttpWebRequest.PreAuthenticate = true;
            objHttpWebRequest.Timeout = 30000000;

            objHttpWebRequest.Method = "POST";

            //Prepare the request stream
            if (!string.IsNullOrEmpty(strRequest))
            {
                encoding = new UTF8Encoding();
                Stream objStream = objHttpWebRequest.GetRequestStream();
                Byte[] buffer = encoding.GetBytes(strRequest);
                // Post the request 
                objStream.Write(buffer, 0, buffer.Length);
                objStream.Close();
            }
            try
            {
                objHttpWebResponse = (HttpWebResponse)objHttpWebRequest.GetResponse();

                encoding = new UTF8Encoding();
                StreamReader objStreamReader = new StreamReader(objHttpWebResponse.GetResponseStream(), encoding);
                strResponse = objStreamReader.ReadToEnd();
                objHttpWebResponse.Close();
                objHttpWebRequest = null;

                return strResponse;
            


            }
            catch (Exception e)
            {
                MailHelper.EnviarMail(emailError, "", "error vimeo", e.ToString());
                return e.ToString();
            }
        }
    }
}