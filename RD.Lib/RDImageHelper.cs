using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.Lib
{
    public static class RDImageHelper
    {
        public static bool SaveImageFromBase64(string p_sBase64ImgStr, string p_sImagePath, string p_sImageName)
        {
            if (!System.IO.Directory.Exists(p_sImagePath)) System.IO.Directory.CreateDirectory(p_sImagePath);

            string imageName = p_sImageName;
            string imgPath = Path.Combine(p_sImagePath, imageName);
            byte[] imageBytes = Convert.FromBase64String(p_sBase64ImgStr);

            File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }

        public static bool SaveImageFromByte(byte[] p_bytImageInByte, string p_sImagePath, string p_sImageName)
        {
            if (!System.IO.Directory.Exists(p_sImagePath)) System.IO.Directory.CreateDirectory(p_sImagePath);

            string imageName = p_sImageName;
            string imgPath = Path.Combine(p_sImagePath, imageName);
            File.WriteAllBytes(imgPath, p_bytImageInByte);

            return true;
        }

        public static byte[] ImageToByteArray(string p_sImageSourcePath)
        {
            return System.IO.File.ReadAllBytes(p_sImageSourcePath);
        }

        public static void CompressAndSaveImage(string p_sImageSourcePath, string p_sOutputPath, int p_iQuality, int p_iWidth, int p_iHeight, int p_iDensity)
        {
            using (MagickImage mImage = new MagickImage(p_sImageSourcePath))
            {
                //mImage.UnsharpMask(0, 2, 1, 0);
                mImage.Resize(p_iWidth, p_iHeight);
                mImage.Quality = p_iQuality;
                mImage.Density = new Density(p_iDensity);

                mImage.Write(p_sOutputPath);
            }

        }
    }
}
