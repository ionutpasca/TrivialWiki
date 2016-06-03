using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace WikiTrivia.Utilities
{
    public static class ImageManager
    {
        public static Image ConvertBase64ToImage(string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                var image = Image.FromStream(ms, true);
                return image;
            }
        }

        public static Image ConvertBytesToImage(byte[] imageBytes)
        {
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                var image = Image.FromStream(ms, true);
                return image;
            }
        }
        public static byte[] ResizeImage(Bitmap originalImage, int width, int height, string fileName = null)
        {
            var fileExtension = fileName != null ? GetImageFormat(GetFileExtension(fileName)) : ImageFormat.Jpeg;

            var newImage = new Bitmap(width, height);
            using (var gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(originalImage, new Rectangle(0, 0, width, height));
            }

            using (var stream = new MemoryStream())
            {
                newImage.Save(stream, fileExtension);
                return stream.ToArray();
            }
        }

        public static ImageFormat GetImageFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case "jpg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
            }
            return ImageFormat.Jpeg;
        }

        private static string GetFileExtension(string filename)
        {
            return Path.GetExtension(filename);
        }

        public static MemoryStream ImageToStream(Image image)
        {
            var memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
