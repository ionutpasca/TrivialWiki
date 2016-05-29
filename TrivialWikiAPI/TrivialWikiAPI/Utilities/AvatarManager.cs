using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TrivialWikiAPI.Utilities
{
    public static class AvatarManager
    {
        public static byte[] ResizeImage(Bitmap originalImage, string fileName, int width, int height)
        {

            var newBitmap = new Bitmap(width, height);
            var fileExtension = GetImageFormat(GetFileExtension(fileName));
            using (var graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(originalImage, 0, 0, width, height);
            }
            using (var stream = new MemoryStream())
            {
                newBitmap.Save(stream, fileExtension);
                return stream.ToArray();
            }
        }

        public static ImageFormat GetImageFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case "jpg":
                    return System.Drawing.Imaging.ImageFormat.Jpeg;
                case "bmp":
                    return System.Drawing.Imaging.ImageFormat.Bmp;
                case "png":
                    return System.Drawing.Imaging.ImageFormat.Png;
            }
            return ImageFormat.Jpeg;
        }

        private static string GetFileExtension(string filename)
        {
            return Path.GetExtension(filename);
        }
    }
}