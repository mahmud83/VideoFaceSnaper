using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Web;
using VideoFace.Common;

namespace VideoFace.Common.Lib
{
   public class ImageHelper
    {
        #region  图片转换为字节数值
        /// <summary>
        /// 图片转换为字节数值
        /// </summary>
        /// <param name="pBitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(Bitmap pBitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                pBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }

        /// <summary>
        /// 图片转换为字节数组-默认图片格式Bmp
        /// </summary>
        public static byte[] BitmapToByteArray(Image image)
        {
            return BitmapToByteArray(image, ImageFormat.Bmp);
        }

        /// <summary>
        /// 图片转换为字节数值
        /// </summary>
        /// <param name="image">图像</param>
        /// <param name="format">格式类型枚举</param>
        public static byte[] BitmapToByteArray(Image image, ImageFormat format)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, format);
                var byteArrayOut = stream.ToArray();
                return byteArrayOut;
            }
        }

        /// <summary>
        /// 图片转字节数组-自动判断格式
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            if (image == null)
            {
                return null;
            }

            ImageFormat fmt = ImageFormat.Jpeg;
            try
            {
                fmt = new ImageFormat(image.RawFormat.Guid);
            }
            catch (ArgumentException)
            {
                // 
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap bmp = new Bitmap(image);
                bmp.Save(ms, fmt);
                return ms.ToArray();
            }

        }
        #endregion

        #region 字节数值转换为图片
        /// <summary>
        /// 字节数值转换为图片
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(byte[] bytes)
        {
            MemoryStream stream = null;
            Bitmap reBitmap = null;
            try
            {
                if (bytes != null)
                {
                    stream = new MemoryStream(bytes);
                    stream.Seek(0, SeekOrigin.Begin);
                    reBitmap = new Bitmap(stream);
                }
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
            return reBitmap;
        }
        #endregion

        /// <summary>
        /// 根据图片路径返回图片的字节流byte[]
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns>返回的字节流</returns>
        public static byte[] LoadImageFile(string imagePath)
        {
            if (!File.Exists(imagePath)) return null;

            using (var files = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] imgByte = new byte[files.Length];
                files.Read(imgByte, 0, imgByte.Length);
                files.Close();
                return imgByte;
            }
        }

        /// <summary>
        /// 字节流转换成图片-存储
        /// </summary>
        /// <param name="byt">要转换的字节流</param>
        /// <returns>转换得到的Image对象</returns>
        public static void Save2ImgFile(byte[] byt, String outPath)
        {
            using (var ms = new MemoryStream(byt))
            {
                System.Drawing.Image img = Image.FromStream(ms);
                img.Save(outPath, ImageFormat.Jpeg);
                img.Dispose();
            }
        }

        /// <summary>
        /// 字节数组转换为图片
        /// </summary>
        /// <param name="imageIn">图片</param>
        /// <returns></returns>
        public static Bitmap ByteArrayToBitmap(byte[] imageIn)
        {
            if (imageIn != null && imageIn.Length == 0)
                return null;

			using (MemoryStream ms1 = new MemoryStream(imageIn))
            {
                Bitmap bmp = (Bitmap) Image.FromStream(ms1);
                ms1.Flush();
                return bmp;
            }
			/*
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(imageIn);
                return new Bitmap(stream);
            }
			*/
        }

        #region 图片转换为base64位字符串

       /// <summary>
       /// 图片转换为base64位字符串
       /// </summary>
       /// <param name="pic"></param>
       /// <returns></returns>
       public static string Base64EncodeFromImage(Image pic)
       {
           Bitmap mBitmap = new Bitmap(pic);
           byte[] imgbt = BitmapToBytes(mBitmap);
           string base64 = Convert.ToBase64String(imgbt);
           return base64;
       }

       public static string Base64EncodeFromBitmapPath(string bmpfilePath)
       {
           if (string.IsNullOrEmpty(bmpfilePath))
           {
               return null;
           }
           if (!File.Exists(bmpfilePath))
           {
               return null;
           }
           return Base64EncodeFromBitmap(new Bitmap(bmpfilePath));
       }

       public static string Base64EncodeFromBitmap(Bitmap bmp)
       {
           byte[] imgbt = BitmapToBytes(bmp);
           string base64 = Convert.ToBase64String(imgbt);
           return base64;
       }
       #endregion

        #region base64位字符串转换为图片
        /// <summary>
        /// base64位字符串转换为图片
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static Image Base64ToImage(string base64String)
        { 
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public static Bitmap Base64ToBitmap(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return new Bitmap(image);
        }
        #endregion

        #region 转缩略图

       /// <summary>
       /// 转缩略图
       /// </summary>
       /// <param name="imgByte"></param>
       /// <param name="intResizedWidth"></param>
       /// <param name="intResizedHeight"></param>
       /// <returns></returns>
       public static byte[] ResizePhoto(byte[] imgByte, int intResizedWidth, int intResizedHeight)
       {
           MemoryStream imgMs = new MemoryStream(imgByte);
           Image oImg = Image.FromStream(imgMs);
           imgMs.Dispose();

           Image oThumbNail = ResizePhoto(oImg, intResizedWidth, intResizedHeight);
           
           MemoryStream ms = new MemoryStream();
           oThumbNail.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
           byte[] photo_byte = ms.GetBuffer();
           ms.Dispose();
           oThumbNail.Dispose();

           return photo_byte;
       }

        /// <summary>
        /// 转缩略图
        /// </summary>
        /// <param name="srcimage"></param>
        /// <param name="intResizedWidth"></param>
        /// <param name="intResizedHeight"></param>
        /// <returns></returns>
        public static Image ResizePhoto(Image srcimage, int intResizedWidth, int intResizedHeight)
        {
            if (srcimage == null) return null;

            int intNewWidth = 0;
            int intNewHeight = 0;
            int intOldWidth = srcimage.Width;
            int intOldHeight = srcimage.Height;
            double dblCoef;
            int intMaxSide;
            int intMaxSideSize;
            if (intOldWidth / (double)intOldHeight > intResizedWidth / (double)intResizedHeight)
            {
                intMaxSide = intOldWidth;
                intMaxSideSize = intResizedWidth;
            }
            else
            {
                intMaxSide = intOldHeight;
                intMaxSideSize = intResizedHeight;
            }
            dblCoef = intMaxSideSize / (double)intMaxSide;
            intNewWidth = Convert.ToInt32(dblCoef * intOldWidth);
            intNewHeight = Convert.ToInt32(dblCoef * intOldHeight);
            Image oThumbNail = new Bitmap(intNewWidth, intNewHeight);
            Graphics oGraphic = Graphics.FromImage(oThumbNail);
            oGraphic.CompositingQuality = CompositingQuality.HighQuality;
            oGraphic.SmoothingMode = SmoothingMode.HighQuality;
            oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            oGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle oRectangle = new Rectangle(0, 0, intNewWidth, intNewHeight);
            oGraphic.DrawImage(srcimage, oRectangle);
            srcimage.Dispose();

            return oThumbNail;
        }

        #endregion

        #region 将数据图片转换成JPG
        /// <summary>
        /// 将数据图片转换成JPG
        /// </summary>
        /// <param name="imgByte"></param>
        /// <returns></returns>
        public static byte[] ConvertJpg(byte[] imgByte)
        {
            MemoryStream imgMs = new MemoryStream(imgByte);
            Image oImg = Image.FromStream(imgMs);
            oImg.Save(imgMs, ImageFormat.Jpeg);
            byte[] photobyte = imgMs.GetBuffer();
            imgMs.Dispose();
            return photobyte;
        }
        #endregion

       /// <summary>
       /// 获取限定规格的图像
       /// </summary>
       /// <param name="img"></param>
       /// <param name="maxSize"></param>
       /// <returns></returns>
        public static Size GetPictureLimitSize(Image img, Size maxSize)
        {
            Size size = new Size();
            if ((img.Width > maxSize.Width) || (img.Height > maxSize.Height))
            {
                double num = Convert.ToDouble(img.Height) / ((double)img.Width);
                double num2 = Convert.ToDouble(maxSize.Height) / ((double)maxSize.Width);
                if (num > num2)
                {
                    size.Height = maxSize.Height;
                    size.Width = Convert.ToInt32((double)(((double)size.Height) / num));
                    return size;
                }
                size.Width = maxSize.Width;
                size.Height = Convert.ToInt32((double)(size.Width * num));
                return size;
            }
            return img.Size;
        }

       /// <summary>
       /// 图像剪切处理,获取区域
       /// </summary>
       /// <param name="b"></param>
       /// <param name="StartX"></param>
       /// <param name="StartY"></param>
       /// <param name="iWidth"></param>
       /// <param name="iHeight"></param>
       /// <returns></returns>
        public static Image KiCut(Image b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            int width = b.Width;
            int height = b.Height;
            if ((StartX >= width) || (StartY >= height))
            {
                return null;
            }
            if ((((StartX == 0) && (StartY == 0)) && (iWidth == width)) && (iHeight == height))
            {
                return b;
            }
            if ((StartX + iWidth) > width)
            {
                iWidth = width - StartX;
            }
            if ((StartY + iHeight) > height)
            {
                iHeight = height - StartY;
            }
            try
            {
                Bitmap image = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics graphics = Graphics.FromImage(image);
                graphics.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                graphics.Dispose();
                return image;
            }
            catch
            {
                return null;
            }
        }

       /// <summary>
       /// 图像进行大小调整
       /// </summary>
       /// <param name="bmp"></param>
       /// <param name="newW"></param>
       /// <param name="newH"></param>
       /// <param name="Mode"></param>
       /// <returns></returns>
        public static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                if ((bmp.Size.Width == newH) && (bmp.Height == newH))
                {
                    return bmp;
                }
                Bitmap image = new Bitmap(newW, newH);
                Graphics graphics = Graphics.FromImage(image);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                graphics.Dispose();
                return image;
            }
            catch
            {
                return null;
            }
        }

        private const int PixelFormat32bppCMYK = 8207;

        // win7下Bitmap.Clone方法处理CMYK图片OutOfMemory异常的解决办法
        // 所有的图像都转化为 Format24bppRgb 或 Format32bppArgb  格式的图像，然后再进行处理，对于不是 Format24bppRgb 或 Format32bppArgb  这两种格式的图像，则使用 Bitmap.Clone()方法进行转化，而这个方法，在处理 PixelFormat 值为 8207 的图像时抛出了异常。
        // 8207 是一个未定义的 PixelFormat 枚举值，它应该是 PixelFormat32bppCMYK 格式的图像
       /// <summary>
       /// 显示特定格式的图像
       /// </summary>
       /// <param name="srcImg">原图像</param>
       /// <param name="destW">目标图像的宽度</param>
       /// <param name="destH">目标图像的高度</param>
       /// <returns></returns>
       public static Bitmap DisplayImage(Bitmap srcImg, int destW, int destH)
        {
            PixelFormat format = srcImg.PixelFormat;
           switch (format)
           {
               case PixelFormat.Format24bppRgb:
                   break;
               case PixelFormat.Format32bppArgb:
                   break;
               default:
                   if ((int) format == PixelFormat32bppCMYK)
                   {
                        //获取图片水平和垂直的分辨率
                       float dpiX = srcImg.HorizontalResolution;
                       float dpiY = srcImg.VerticalResolution;

                       Bitmap bmPhoto = new Bitmap(destW, destH, PixelFormat.Format32bppRgb);
                       bmPhoto.SetResolution(dpiX, dpiY);
                       using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
                       {
                           grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                           grPhoto.DrawImage(srcImg,
                               new System.Drawing.Rectangle(0, 0, bmPhoto.Width, bmPhoto.Height),
                               new System.Drawing.Rectangle(0, 0, srcImg.Width, srcImg.Height),
                               GraphicsUnit.Pixel);
                       }
                       return bmPhoto;
                   }
                   else
                   {
                       format = PixelFormat.Format32bppArgb;
                   }
                   break;
           }
           return srcImg.Clone(new Rectangle(0, 0, destW, destH), format);
        }

       /// <summary>
       /// 加载图像数据(对不同来源，如http，ftp或本地文件）
       /// </summary>
       /// <param name="strUrlPath"></param>
       /// <returns></returns>
       public static Bitmap GetBitmapFromPath(string strUrlPath)
       {
           if (string.IsNullOrEmpty(strUrlPath)) return null;

           if (strUrlPath.StartsWith("http") || strUrlPath.StartsWith("ftp"))
           {
                string strserverPath = HttpUtility.UrlDecode(strUrlPath);
                try
               {
                    Image webmage = Image.FromStream(
                       WebRequest.Create(strserverPath).GetResponse().GetResponseStream());

                   Bitmap bitmapRet = (Bitmap)webmage.Clone();
                   webmage.Dispose();
                   webmage = null;

                   return bitmapRet;
               }
               catch (Exception ex)
               {
                    Log4NetHelper.Instance.Error("加载网络图片：" + strserverPath + "出现错误", ex);
                   return null;
               }
           }
           else
           {
               Bitmap tempbmp = new Bitmap(strUrlPath);
               Bitmap bitmapRet = (Bitmap)tempbmp.Clone();
               tempbmp.Dispose();
               tempbmp = null;

               return bitmapRet;
           }
       }

       /// <summary>
       /// 获取指定点位集合的图像
       /// </summary>
       /// <param name="orialbmp"></param>
       /// <param name="points"></param>
       /// <param name="bDisposeOrial"></param>
       /// <returns></returns>
       public static Bitmap GetRegionBitmap(Image orialbmp, Point[] points, bool bDisposeOrial = true)
       {
           if (orialbmp == null) return null;
           if (points == null || points.Length == 0) return (Bitmap)orialbmp;

           try
           {
               if (points.Length == 2)
               {
                   Rectangle selection = new Rectangle(points[0].X, points[0].Y, Math.Abs(points[1].X - points[0].X),
                       Math.Abs(points[1].Y - points[0].Y));
                   Bitmap bmpNew = ((Bitmap)orialbmp).Clone(selection, orialbmp.PixelFormat);
                   if (bDisposeOrial)
                   {
                       orialbmp.Dispose();
                       orialbmp = null;
                   }
                   return bmpNew;
               }
               else if (points.Length == 3)
               {
                   System.Drawing.Drawing2D.GraphicsPath lPath = new System.Drawing.Drawing2D.GraphicsPath();
                   lPath.AddPolygon(points);
                   Region region = new Region(new RectangleF(0, 0, orialbmp.Width, orialbmp.Height));
                   region.Xor(lPath);
                   Bitmap bmpNew = (Bitmap) orialbmp.Clone();
                   Graphics lGraphics = Graphics.FromImage(bmpNew);
                   lGraphics.FillRegion(Brushes.White, region);
                   lGraphics.Dispose();
                   if (bDisposeOrial)
                   {
                       orialbmp.Dispose();
                       orialbmp = null;
                   }
                   return bmpNew;
               }
               else if (points.Length == 4)
               {
                   Rectangle selection = new Rectangle(points[0].X, points[0].Y, Math.Abs(points[3].X - points[0].X),
                       Math.Abs(points[3].Y - points[0].Y));
                   Bitmap bmpNew = ((Bitmap) orialbmp).Clone(selection, orialbmp.PixelFormat);
                   if (bDisposeOrial)
                   {
                       orialbmp.Dispose();
                       orialbmp = null;
                   }
                   return bmpNew;
               }
           }
           catch (Exception ex1)
           {
                Log4NetHelper.Instance.Error("ImageHelper.GetRegionBitmap方法,截取局部图像区域出现错误：" + (ex1.InnerException ==null? ex1.Message: ex1.InnerException.Message));
           }
           return null;
       }
    }
}
