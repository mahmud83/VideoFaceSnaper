using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using ImageProcessor;
using ImageSplicer.Common;

namespace ImageChecker
{
    public class ProcessImager2
    {
        private void BlendImageGdi(Bitmap srcImg1, Bitmap srcImg2, ImageSpliceSort eSort, out Bitmap destImg)
        {
            if (eSort == ImageSpliceSort.Vertical)
            {
                destImg = new Bitmap(srcImg1.Width, srcImg1.Height + srcImg2.Height, PixelFormat.Format32bppArgb);
                Graphics G = Graphics.FromImage(destImg);

                G.DrawImage(srcImg1, new Rectangle(0, 0, srcImg1.Width, srcImg1.Height), new Rectangle(0, 0, srcImg1.Width, srcImg1.Height), GraphicsUnit.Pixel);
                G.DrawImage(srcImg2, new Rectangle(0, srcImg1.Height, srcImg2.Width, srcImg2.Height), new Rectangle(0, 0, srcImg2.Width, srcImg2.Height), GraphicsUnit.Pixel);

                G.Dispose();
            }
            else if (eSort == ImageSpliceSort.Horizontal)
            {
                destImg = new Bitmap(srcImg1.Width + srcImg2.Width, srcImg1.Height, PixelFormat.Format32bppArgb);
                Graphics G = Graphics.FromImage(destImg);

                G.DrawImage(srcImg1, new Rectangle(0, 0, srcImg1.Width, srcImg1.Height), new Rectangle(0, 0, srcImg1.Width, srcImg1.Height), GraphicsUnit.Pixel);
                G.DrawImage(srcImg2, new Rectangle(srcImg1.Width, 0, srcImg2.Width, srcImg2.Height), new Rectangle(0, 0, srcImg2.Width, srcImg2.Height), GraphicsUnit.Pixel);

                G.Dispose();
            }
            else
            {
                destImg = new Bitmap(srcImg1.Width, srcImg1.Height, PixelFormat.Format32bppArgb);
                Graphics G = Graphics.FromImage(destImg);

                G.DrawImage(srcImg1, new Rectangle(0, 0, srcImg1.Width, srcImg1.Height), new Rectangle(0, 0, srcImg1.Width, srcImg1.Height), GraphicsUnit.Pixel);
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
                zoomRect.X = cropRect.X - (int)(cropRect.Width * 1.0F / zoom);
                zoomRect.Y = cropRect.Y - (int)(cropRect.Height * 1.0F / zoom);

                if (zoomRect.X < 0) zoomRect.X = 0;
                if (zoomRect.Y < 0) zoomRect.Y = 0;

                var srcimg3 = new Bitmap(srcimg2, srcimg2.Width * zoom, srcimg2.Height * zoom);
                zoomRect.Width = cropRect.Width * 2;
                zoomRect.Height = cropRect.Height * 2;

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

    }
}
