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

namespace EnglishForCoachesWeb.Helpers
{
    public static class FileUploadHelper
    {

        private static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            double ratioX = double.MaxValue;
            if (maxWidth > 0)
            {
                 ratioX = (double)maxWidth / image.Width;
            }
            double ratioY = double.MaxValue;
            if (maxHeight > 0)
            {
                 ratioY = (double)maxHeight / image.Height;
            }
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        private static byte[] ImageToByte2(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public static void SubirImagenArticulo(HttpPostedFileBase templateFile, int width, int height,string path)
        {
            SubirImagenCropScale(templateFile, width, height, path, ImageFormat.Jpeg);
        }

        public static void SubirImagenCropScale(HttpPostedFileBase templateFile, int width, int height, string path, ImageFormat imageFormat)
        {
            var image = Image.FromStream(templateFile.InputStream, true, true);


            var croppedImage = CropBitmap(image, width, height,true);
                var scaledImage = ScaleImage(croppedImage, width, height);


            scaledImage.Save(path, imageFormat);
        }
        public static void SubirImagenCuadrada(HttpPostedFileBase templateFile, int width, int height, string path, ImageFormat imageFormat)
        {
            var image = Image.FromStream(templateFile.InputStream, true, true);


            var croppedImage = CropBitmap(image, width, height,true);
            var scaledImage = ScaleImage(croppedImage, width, height);


            scaledImage.Save(path, imageFormat);
        }

        public static void SubirAvatar(HttpPostedFileBase templateFile, string path)
        {
            SubirImagenArticulo(templateFile,50,50,path);


        }


        private static string SubirImagenCuerpo(string base64, int Id, string dir)
        {
            string guid = Guid.NewGuid().ToString();
            string virtualRoute = "~/"+ dir + Id + "/" + guid + ".jpg";
            string path = HttpContext.Current.Server.MapPath(virtualRoute);
            while (File.Exists(path))
            {
                guid = Guid.NewGuid().ToString();
                virtualRoute = "~/"+ dir + Id + "/" + guid + ".jpg";
                path = HttpContext.Current.Server.MapPath(virtualRoute);
            }
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            var bytes = Convert.FromBase64String(base64);
            using (var imageFile = new FileStream(path, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
            return virtualRoute.Replace("~", "");
        }

        internal static string ActualizarImagenesCuerpo(string cuerpo,int id, string dir)
        {
            List<string> listaTipos = new List<string>() {"jpeg","png" };

            foreach (string tipo in listaTipos)
            {
                MatchCollection matches = Regex.Matches(cuerpo, @"<img src=""data:image/"+tipo+";base64,[^>]*>");

                foreach (var match in matches)
                {
                    string image = match.ToString();
                    string pattern = @"src=""data:image/" + tipo + ";base64,[^\"]*\"";
                    Match srcMatch = Regex.Match(image, pattern);
                    string base64 = srcMatch.ToString().Replace("src=\"data:image/"+tipo+";base64,", "").Replace("\"", "");


                    string virtualRoute = SubirImagenCuerpo(base64, id,dir);

                    cuerpo = cuerpo.Replace(srcMatch.ToString(), "src=\"" + virtualRoute + "\"");
                }
            }
            return cuerpo;
        }

    

        private static Image CropBitmap(Image image, int targetWidth, int targetHeight,bool mantainRatio)
        {

            var ratioOriginal = image.Width / image.Height;
            var ratioNuevo = targetWidth / targetHeight;
            Rectangle rect;
            var cropWidth = image.Width;
            var cropHeight = image.Height;
            var cropX = 0;
           var cropY = 0;

            if (mantainRatio)
            {
                if (ratioOriginal < ratioNuevo)
                {
                     cropHeight = image.Width * targetHeight / targetWidth;
                     cropY = (image.Height - cropHeight) / 2;                    
                }
                else
                {
                     cropWidth = image.Height * targetWidth / targetHeight;
                     cropX = (image.Width - cropWidth) / 2;
                }
            }else
            {
                if (image.Height > image.Width)
                {
                    cropY = (image.Height - image.Width) / 2;
                    cropHeight = image.Height - cropY;
                }
                else
                {
                    cropX = (image.Width - image.Height) / 2;
                    cropWidth = image.Width - cropY;
                }

            }
            rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
            Bitmap cropped = ((Bitmap)image).Clone(rect, image.PixelFormat);

            return cropped;
        }
        
    }
}