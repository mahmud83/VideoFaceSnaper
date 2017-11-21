using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using ImageProcessor;
using ImageSplicer.Common;

namespace ImageSplicer
{
    public class ProcessImager
    {
        //图片叠加
        protected void WaterMark(string path, string markpath)
        {
            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(path);
            System.Drawing.Image imgWarter = System.Drawing.Image.FromFile(markpath);
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(imgWarter, new Rectangle(imgSrc.Width - imgWarter.Width,
                    imgSrc.Height - imgWarter.Height,
                    imgWarter.Width,
                    imgWarter.Height),
                    0, 0, imgWarter.Width, imgWarter.Height, GraphicsUnit.Pixel);
            }

            string newpath = path + "000";
            imgSrc.Save(newpath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        //图片写字
        protected void FontMark(string path, string addText)
        {
            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(path);

            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(imgSrc, 0, 0, imgSrc.Width, imgSrc.Height);
                using (Font f = new Font("宋体", 20))
                {
                    using (Brush b = new SolidBrush(Color.Red))
                    {
                        g.DrawString(addText, f, b, 100, 20);
                    }
                }
            }
            string newpath = path + "000";
            imgSrc.Save(newpath, System.Drawing.Imaging.ImageFormat.Jpeg);

        }

        private void BlendImageGdi(Bitmap srcImg1, Bitmap srcImg2, ImageSpliceSort eSort, out Bitmap destImg)
        {
            if (eSort == ImageSpliceSort.Vertical)
            {
                destImg = new Bitmap(srcImg1.Width, srcImg1.Height + srcImg2.Height, PixelFormat.Format32bppArgb);
                Graphics G = Graphics.FromImage(destImg);

                G.DrawImage(srcImg1, new Rectangle(0, 0, srcImg1.Width, srcImg1.Height), new Rectangle(0, 0, srcImg1.Width, srcImg1.Height),GraphicsUnit.Pixel);
                G.DrawImage(srcImg2, new Rectangle(0, srcImg1.Height, srcImg2.Width, srcImg2.Height), new Rectangle(0, 0, srcImg2.Width, srcImg2.Height), GraphicsUnit.Pixel);

                G.Dispose();
            }
            else if (eSort == ImageSpliceSort.Horizontal)
            {
                destImg = new Bitmap(srcImg1.Width + srcImg2.Width, srcImg1.Height, PixelFormat.Format32bppArgb);
                Graphics G = Graphics.FromImage(destImg);

                G.DrawImage(srcImg1, new Rectangle(0, 0, srcImg1.Width, srcImg1.Height), new Rectangle(0, 0, srcImg1.Width, srcImg1.Height),GraphicsUnit.Pixel);
                G.DrawImage(srcImg2, new Rectangle(srcImg1.Width, 0, srcImg2.Width, srcImg2.Height), new Rectangle(0, 0, srcImg2.Width, srcImg2.Height), GraphicsUnit.Pixel);

                G.Dispose();
            }
            else
            {
                destImg = new Bitmap(srcImg1.Width, srcImg1.Height, PixelFormat.Format32bppArgb);
                Graphics G = Graphics.FromImage(destImg);

                G.DrawImage(srcImg1, new Rectangle(0, 0, srcImg1.Width, srcImg1.Height), new Rectangle(0, 0, srcImg1.Width, srcImg1.Height),GraphicsUnit.Pixel);
                G.DrawImage(srcImg2, new Rectangle(0, 0, srcImg2.Width, srcImg2.Height), new Rectangle(0, 0, srcImg2.Width, srcImg2.Height), GraphicsUnit.Pixel);
                G.Dispose();
            }
            
        }

        private void BlendImageGdi(Bitmap srcImg1, Bitmap srcImg2, Rectangle rectangle, out Bitmap destImg)
        {
            destImg = new Bitmap(srcImg1.Width, srcImg1.Height, PixelFormat.Format32bppArgb);
            Graphics G = Graphics.FromImage(destImg);

            G.DrawImage(srcImg1, new Rectangle(0, 0, srcImg1.Width, srcImg1.Height), new Rectangle(0, 0, srcImg1.Width, srcImg1.Height), GraphicsUnit.Pixel);
            G.DrawImage(srcImg2, rectangle, new Rectangle(0, 0, srcImg2.Width, srcImg2.Height), GraphicsUnit.Pixel);
            G.Dispose();

        }

        /// <summary>
        /// 图像无损压缩
        /// </summary>
        /// <param name="sFile">源文件</param>
        /// <param name="dFile">目标文件</param>
        /// <param name="dHeight">压缩后的高度</param>
        /// <param name="dWidth">压缩后的宽度</param>
        /// <param name="scale">压缩比</param>
        /// <param name="flag">压缩质量参数，一般可设50</param>
        /// <returns></returns>
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, float scale, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (dHeight == 0 && dWidth == 0 && scale != 0)
            {
                dHeight = Convert.ToInt32(scale * iSource.Height);
                dWidth = Convert.ToInt32(scale * iSource.Width);
            }
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }

                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }

        /// <summary>
        /// 图片数据叠加-文件加载
        /// </summary>
        public void CmdImage(string srcimgPath, string descimgpath, string outputpath)
        {
            var srcimg1 = new Bitmap(srcimgPath);
            var srcimg2 = new Bitmap(descimgpath);

            ImageSpliceSort eSort = ImageSpliceSort.Vertical;
            Bitmap destImg = null;

            BlendImageGdi(srcimg1, srcimg2, eSort, out destImg);

            if (destImg != null)
            {
                destImg.Save(outputpath, ImageFormat.Jpeg);
                destImg.Dispose();
            }

            srcimg1.Dispose();
            srcimg2.Dispose();
        }

        /// <summary>
        /// 图片数据叠加
        /// </summary>
        public void CmdImage(Bitmap srcimg, Bitmap srcimg2, string outputpath)
        {
            ImageSpliceSort eSort = ImageSpliceSort.Vertical;
            Bitmap destImg = null;

            BlendImageGdi(srcimg, srcimg2, eSort, out destImg);

            if (destImg != null)
            {
                destImg.Save(outputpath, ImageFormat.Jpeg);
                destImg.Dispose();
            }
        }

        /// <summary>
        /// 图片数据叠加-文件加载
        /// </summary>
        public bool CmdImage(string srcimgPath, Rectangle cropRect, string outputpath, out string errorMsg,
            int iOver = 0)
        {
            errorMsg = null;
            var srcimg1 = new Bitmap(srcimgPath);
            bool lbRet = CmdImage(srcimg1, cropRect, outputpath, out errorMsg, iOver);
            if (srcimg1 != null)
            {
                srcimg1.Dispose();
            }
            return lbRet;
        }

        /// <summary>
        /// 图片数据叠加
        /// </summary>
        public bool CmdImage(Bitmap srcimg, Rectangle cropRect, string outputpath, out string errorMsg, int iOver = 0)
        {
            try
            {
                errorMsg = null;
                if (srcimg.Width < cropRect.X + cropRect.Width)
                {
                    errorMsg = "截图区域宽度不符合要求:" + srcimg.Width.ToString() + "-" + (cropRect.X + cropRect.Width).ToString();
                    return false;
                }
                if (srcimg.Height < cropRect.Y + cropRect.Height)
                {
                    errorMsg = "截图区域高度不符合要求:" + srcimg.Height.ToString() + "-" + (cropRect.Y + cropRect.Height).ToString();
                    return false;
                }

                Bitmap srcimg2 = null;
                CropImage(srcimg, cropRect, out srcimg2);
                if ((srcimg2 == null) || (srcimg2.Width < 5) || (srcimg2.Height < 5))
                {
                    errorMsg = "截图区域小于5个像素";
                    if (srcimg2 != null)
                    {
                        srcimg2.Dispose();
                    }
                    return false;
                }

                // 放大或缩小
                int zoom = 2;
                Rectangle zoomRect = new Rectangle();
                zoomRect.X = cropRect.X - (int) (cropRect.Width*1.0F/zoom);
                zoomRect.Y = cropRect.Y - (int) (cropRect.Height*1.0F/zoom);

                if (zoomRect.X < 0) zoomRect.X = 0;
                if (zoomRect.Y < 0) zoomRect.Y = 0;

                var srcimg3 = new Bitmap(srcimg2, srcimg2.Width * zoom, srcimg2.Height * zoom);
                zoomRect.Width = cropRect.Width*2;
                zoomRect.Height = cropRect.Height*2;

                Bitmap destImg = null;
                if (iOver == 0)
                {
                    Rectangle selRect = SelectRectangle(srcimg, cropRect, zoomRect);
                    if ((selRect.Width > 0 && selRect.Height > 0))
                    {
                        BlendImageGdi(srcimg, srcimg3, selRect, out destImg);
                    }
                    else
                    {
                        errorMsg = "截图区域无法叠加";
                        srcimg2.Dispose();
                        srcimg3.Dispose();
                        return false;
                    }
                }
                else
                {
                    ImageSpliceSort eSort = ImageSpliceSort.Default;
                    if (iOver == 1)
                    {
                        eSort = ImageSpliceSort.Vertical;
                        BlendImageGdi(srcimg3, srcimg, eSort, out destImg);
                    }
                    else if (iOver == 3)
                    {
                        eSort = ImageSpliceSort.Vertical;
                        BlendImageGdi(srcimg, srcimg3, eSort, out destImg);
                    }
                    else if (iOver == 2)
                    {
                        eSort = ImageSpliceSort.Horizontal;
                        BlendImageGdi(srcimg3, srcimg, eSort, out destImg);
                    }
                    else
                    {
                        eSort = ImageSpliceSort.Horizontal;
                        BlendImageGdi(srcimg, srcimg3, eSort, out destImg);
                    }
                }

                if (destImg != null)
                {
                    destImg.Save(outputpath, ImageFormat.Jpeg);
                    destImg.Dispose();

                    srcimg2.Dispose();
                    srcimg3.Dispose();
                    return true;
                }
                errorMsg = "截图对象为空";
                srcimg2.Dispose();
                srcimg3.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                errorMsg = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据截图图像位置判断
        /// </summary>
        /// <param name="srcimg">原图像对象</param>
        /// <param name="cropRect">截图区域</param>
        /// <param name="zoomRect">放大的区域</param>
        /// <returns></returns>
        private Rectangle SelectRectangle(Bitmap srcimg, Rectangle cropRect, Rectangle zoomRect)
        {
            Rectangle leftbutton = new Rectangle(0, srcimg.Height - zoomRect.Height, zoomRect.Width, zoomRect.Height);
            Rectangle rightbutton = new Rectangle(srcimg.Width - zoomRect.Width, srcimg.Height - zoomRect.Height, zoomRect.Width, zoomRect.Height);
            Rectangle lefttop = new Rectangle(0, 0, zoomRect.Width, zoomRect.Height);
            Rectangle righttop = new Rectangle(srcimg.Width - zoomRect.Width, 0, zoomRect.Width, zoomRect.Height);

            Rectangle selRect;
            if (cropRect.IntersectsWith(leftbutton))
            {
                if (cropRect.IntersectsWith(rightbutton))
                {
                    if (cropRect.IntersectsWith(righttop))
                    {
                        if (cropRect.IntersectsWith(lefttop))
                        {
                            // 位置选择无合适的情况,采用叠加到外部方式
                            selRect = new Rectangle(0, 0, 0, 0);
                        }
                        else
                        {
                            selRect = lefttop;
                        }
                    }
                    else
                    {
                        selRect = righttop;
                    }
                }
                else
                {
                    selRect = rightbutton;
                }
            }
            else
            {
                selRect = leftbutton;
            }

            return selRect;
        }

        /// <summary>
        /// 根据截图图像位置判断
        /// </summary>
        /// <param name="srcimg">原图像对象</param>
        /// <param name="cropRect">截图区域</param>
        /// <returns></returns>
        private Rectangle SelectRectangle(Bitmap srcimg, Rectangle cropRect)
        {
            return SelectRectangle(srcimg, cropRect, cropRect);
        }

        /// <summary>
        /// 进行截图处理
        /// </summary>
        /// <param name="srcimg">原图像对象</param>
        /// <param name="cropRect">截图区域</param>
        /// <param name="cropimg">截图结果</param>
        private void CropImage(Bitmap srcimg, Rectangle cropRect, out Bitmap cropimg)
        {
            cropimg = null;
            // 加载图像
            ImageFactory imgFactory = new ImageFactory();
            using (MemoryStream m = new MemoryStream())
            {
                srcimg.Save(m, ImageFormat.Jpeg);
                imgFactory.Load(m);
            }

            imgFactory.Crop(cropRect);

            using (MemoryStream stream = new MemoryStream())
            {
                imgFactory.Save(stream);
                Image result = Image.FromStream(stream);
                cropimg = new Bitmap(result);
            }
        }

        /// <summary>
        /// 获取抓拍图像的数据
        /// </summary>
        /// <param name="snapimgUrl">支持外部的http或ftp文件路径</param>
        /// <param name="bmpdata">摄像机比对模板文件路径</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool GetImageByUrl(string snapimgUrl, out Bitmap bmpdata, out string errorMsg)
        {
            bmpdata = null;
            errorMsg = null;
            try
            {
                // 暂时不用
                //var ssnapimgUrl = HttpUtility.UrlEncode(snapimgUrl);
                var cookies = new CookieCollection();

                HttpWebResponse response = HttpWebReqUtility.CreateGetHttpResponse(snapimgUrl, null, null, cookies);
                int responseCode = Convert.ToInt32(response.StatusCode);
                if (responseCode == 200)
                {
                    Stream myResponseStream = response.GetResponseStream();
                    if (myResponseStream != null)
                    {
                        var myStreamReader = new StreamReader(myResponseStream);
                        var bmprecv = Image.FromStream(myResponseStream);
                        bmpdata = new Bitmap(bmprecv);

                        myStreamReader.Close();
                        myResponseStream.Close();

                        return true;
                    }
                }
                else if (responseCode == 404)
                {
                    Log4NetHelper.Instance.Info("获取HTTP图片数据：" + snapimgUrl + "，找不到对应的图片");
                }
                return false;
            }
            catch (Exception ex)
            {
                errorMsg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
                Log4NetHelper.Instance.Info("获取HTTP图片数据错误：" + errorMsg);
                return false;
            }
        }

        public Rectangle GetRectangle(string rectstr)
        {
            if (string.IsNullOrEmpty(rectstr)) return new Rectangle(0, 0, 0, 0);

            string[] splitstr = rectstr.Split(',');
            if (splitstr.Length == 4)
            {
                return new Rectangle(int.Parse(splitstr[0]), int.Parse(splitstr[1]), int.Parse(splitstr[2]), int.Parse(splitstr[3]));
            }
            return new Rectangle(0, 0, 0, 0);
        }

        #region 测试代码
        /// <summary>
        /// 图片数据叠加
        /// </summary>
        private void TestBlendImageCode()
        {
            var srcimg1= new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "2016-01-12-15h.png");
            var srcimg2 = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "49.jpg");

            ImageSpliceSort eSort = ImageSpliceSort.Vertical;
            Bitmap destImg = null;

            BlendImageGdi(srcimg1, srcimg2, eSort, out destImg);
             
            destImg.Save(AppDomain.CurrentDomain.BaseDirectory + "49_2.jpg",ImageFormat.Jpeg);
            destImg.Dispose();

            eSort = ImageSpliceSort.Horizontal;
            Bitmap destImg2 = null;
            BlendImageGdi(srcimg1, srcimg2, eSort, out destImg2);

            destImg2.Save(AppDomain.CurrentDomain.BaseDirectory + "49_3.jpg", ImageFormat.Jpeg);
            destImg2.Dispose();

            srcimg1.Dispose();
            srcimg2.Dispose();
        }

        /// <summary>
        /// 像素方式访问
        /// </summary>
        private void TestBlendImageCode2()
        {
            var bmp = new Bitmap(AppDomain.CurrentDomain.BaseDirectory+ "2016-01-12-15h.png");
            Benchmark.Start();
            LockBitmap lockBitmap = new LockBitmap(bmp);
            lockBitmap.LockBits();

            Color compareClr = Color.FromArgb(255, 255, 255, 255);
            for (int y = 0; y < lockBitmap.Height; y++)
            {
                for (int x = 0; x < lockBitmap.Width; x++)
                {
                    if (lockBitmap.GetPixel(x, y) == compareClr)
                    {
                        lockBitmap.SetPixel(x, y, Color.Red);
                    }
                }
            }
            lockBitmap.UnlockBits();
            Benchmark.End();
            double seconds = Benchmark.GetSeconds();

            bmp.Dispose();
        }

        /// <summary>
        /// 图片数据截取并叠加
        /// </summary>
        private void TestBlendImageCode3()
        {
            string srcimgPath = AppDomain.CurrentDomain.BaseDirectory + "14140889701(1139,919,507,439).jpg";
            var srcimg1 = new Bitmap(srcimgPath);
            Bitmap srcimg2 = null;
            Rectangle cropRect = new Rectangle(1139, 919, 507, 439);
            CropImage(srcimg1, cropRect, out srcimg2);

            ImageSpliceSort eSort = ImageSpliceSort.Vertical;
            Bitmap destImg = null;

            BlendImageGdi(srcimg1, srcimg2, eSort, out destImg);

            destImg.Save(AppDomain.CurrentDomain.BaseDirectory + "14140889701.jpg", ImageFormat.Jpeg);
            destImg.Dispose();

            srcimg1.Dispose();
            srcimg2.Dispose();
        }

        /// <summary>
        /// 图片数据截取并叠加-寻找位置
        /// </summary>
        private void TestBlendImageCode4()
        {
            string srcimgPath = AppDomain.CurrentDomain.BaseDirectory + "09224018601.jpg";
            var srcimg1 = new Bitmap(srcimgPath);
            Bitmap srcimg2 = null;
            Rectangle cropRect = new Rectangle(821, 820, 502, 539);
            CropImage(srcimg1, cropRect, out srcimg2);

            Bitmap destImg = null;
            Rectangle selRect = SelectRectangle(srcimg1, cropRect);
            if (selRect.Width > 0 && selRect.Height > 0)
            {
                BlendImageGdi(srcimg1, srcimg2, selRect, out destImg);
            }
            else
            {
                BlendImageGdi(srcimg1, srcimg2, ImageSpliceSort.Vertical, out destImg);
            }

            destImg.Save(AppDomain.CurrentDomain.BaseDirectory + "09224018601_2.jpg", ImageFormat.Jpeg);
            destImg.Dispose();

            srcimg1.Dispose();
            srcimg2.Dispose();
        }

        /// <summary>
        /// 图片数据截取并叠加-放大寻找位置
        /// </summary>
        private void TestBlendImageCode5()
        {
            string srcimgPath = AppDomain.CurrentDomain.BaseDirectory + "09224018601.jpg";
            var srcimg1 = new Bitmap(srcimgPath);
            Bitmap srcimg2 = null;
            Rectangle cropRect = new Rectangle(821, 820, 502, 539);
            CropImage(srcimg1, cropRect, out srcimg2);

            // 放大或缩小
            int zoom = 2;
            Rectangle zoomRect = new Rectangle();
            zoomRect.X = cropRect.X - (int)(cropRect.Width * 1.0F / zoom);
            zoomRect.Y = cropRect.Y - (int)(cropRect.Height * 1.0F / zoom);

            if (zoomRect.X < 0) zoomRect.X = 0;
            if (zoomRect.Y < 0) zoomRect.Y = 0;

            zoomRect.Width = cropRect.Width * 2;
            zoomRect.Height = cropRect.Height * 2;

            Bitmap destImg = null;
            Rectangle selRect = SelectRectangle(srcimg1, cropRect, zoomRect);
            if (selRect.Width > 0 && selRect.Height > 0)
            {
                BlendImageGdi(srcimg1, srcimg2, selRect, out destImg);
            }
            else
            {
                BlendImageGdi(srcimg1, srcimg2, ImageSpliceSort.Vertical, out destImg);
            }

            destImg.Save(AppDomain.CurrentDomain.BaseDirectory + "09224018601_2.jpg", ImageFormat.Jpeg);
            destImg.Dispose();

            srcimg1.Dispose();
            srcimg2.Dispose();
        }
        #endregion
    }
}
