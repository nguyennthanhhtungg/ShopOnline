using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Extensions
{
    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        public static Image Resize(this Image image, int width, int height)
        {
            width = width == 0 ? image.Width : width;
            height = height == 0 ? image.Height : height;

            var newImage = new Bitmap(width, height);

            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, width, height);
            }

            return newImage;
        }
    }
}
