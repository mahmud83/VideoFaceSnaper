using System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using VideoFace.Common;
using System.Drawing;
using VideoFace.Common.Lib;

namespace MsFaceSDK
{
    /// <summary>
    /// 人脸比对的标准访问接口
    /// </summary>
    public class FaceManagerSDK
    {
        static FaceService.FaceWcfServiceClient client = new FaceService.FaceWcfServiceClient();

        /// <summary>
        /// 获取人脸特征，通过图像文件
        /// </summary>
        /// <param name="faceImgPath"></param>
        /// <param name="feature"></param>
        public bool GetFeature(String faceImgPath, out byte[] feature)
        {
            feature = null;
            byte[] vbytes = ImageHelper.LoadImageFile(faceImgPath);
            if (vbytes == null)
            {
                return false;
            }
            return GetFeature(vbytes, out feature);
        }

        /// <summary>
        /// 获取人脸特征，通过图像对象
        /// </summary>
        /// <param name="faceimage"></param>
        /// <param name="feature"></param>
        public bool GetFeature(Image faceimage, out byte[] feature)
        {
            feature = null;
            if (faceimage == null) return false;

            byte[] vbytes = ImageHelper.ImageToBytes(faceimage);
            if (vbytes == null)
            {
                return false;
            }
            return GetFeature(vbytes, out feature);
        }

        /// <summary>
        /// 获取人脸特征
        /// </summary>
        /// <param name="faceimagedata"></param>
        /// <param name="feature"></param>
        public bool GetFeature(byte[] faceimagedata, out byte[] feature)
        {
            feature = null;
            try
            {
                feature = client.GetFeature(faceimagedata);
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Info("FaceService.GetFeature出现异常：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取人脸头像，通过图像文件
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="faceImgs"></param>
        public bool GetAvatar(String imagePath, out List<Image> faceImgs)
        {
            faceImgs = null;
            byte[] vbytes = ImageHelper.LoadImageFile(imagePath);
            if (vbytes == null)
            {
                return false;
            }
            return GetAvatar(vbytes, out faceImgs);
        }

        /// <summary>
        /// 获取人脸头像
        /// </summary>
        /// <param name="imagedata"></param>
        /// <param name="faceImgs"></param>
        public bool GetAvatar(byte[] imagedata, out List<Image> faceImgs)
        {
            faceImgs = null;
            try
            {
                if (imagedata == null || imagedata.Length == 0)
                {
                    return false;
                }
                var imgbytes = client.GetAvatar(imagedata);
                if (imgbytes != null && imgbytes.Length > 0)
                {
                    faceImgs = new List<Image>();
                    foreach (var byt in imgbytes)
                    {
                        using (var ms = new MemoryStream(byt))
                        {
                            System.Drawing.Image img = Image.FromStream(ms);
                            faceImgs.Add(img);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Info("FaceService.GetAvatar出现异常：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取人脸矩形区域，通过图像文件
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="facerects"></param>
        public bool GetFaceRect(String imagePath, out List<Rectangle> facerects)
        {
            facerects = null;
            byte[] vbytes = ImageHelper.LoadImageFile(imagePath);
            if (vbytes == null)
            {
                return false;
            }
            return GetFaceRect(vbytes, out facerects);
        }

        /// <summary>
        /// 获取人脸矩形区域，通过图像对象
        /// </summary>
        /// <param name="image"></param>
        /// <param name="facerects"></param>
        public bool GetFaceRect(Image image, out List<Rectangle> facerects)
        {
            facerects = null;
            if(image == null) return false;

            byte[] vbytes = ImageHelper.ImageToBytes(image);
            if (vbytes == null)
            {
                return false;
            }
            return GetFaceRect(vbytes, out facerects);
        }

        /// <summary>
        /// 获取人脸矩形区域
        /// </summary>
        /// <param name="imagedata"></param>
        /// <param name="facerects"></param>
        public bool GetFaceRect(byte[] imagedata, out List<Rectangle> facerects)
        {
            facerects = null;
            try
            {
                var imgrects = client.GetRect(imagedata);
                if (imgrects != null && imgrects.Length > 0)
                {
                    facerects = new List<Rectangle>(imgrects);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Info("FaceService.GetFaceRect出现异常：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 进行标准图像建模，通过人脸图像文件
        /// </summary>
        /// <param name="faceimagePath"></param>
        /// <param name="personId"></param>
        public bool AddLibrary(String faceimagePath, String personId)
        {
            byte[] vbytes = ImageHelper.LoadImageFile(faceimagePath);
            if (vbytes == null)
            {
                return false;
            }
            return AddLibrary(vbytes, personId);
        }

        /// <summary>
        /// 进行标准图像建模，通过图像对象
        /// </summary>
        /// <param name="faceimage"></param>
        /// <param name="personId"></param>
        public bool AddLibrary(Image faceimage, String personId)
        {
            if (faceimage == null) return false;
            byte[] vbytes = ImageHelper.ImageToBytes(faceimage);
            if (vbytes == null)
            {
                return false;
            }
            return AddLibrary(vbytes, personId);
        }

        /// <summary>
        /// 进行标准图像建模
        /// </summary>
        /// <param name="faceimagedata"></param>
        /// <param name="personId"></param>
        public bool AddLibrary(byte[] faceimagedata, String personId)
        {
            try
            {
                if (faceimagedata == null || string.IsNullOrEmpty(personId))
                {
                    return false;
                }

                bool badd = client.WritePdbID(faceimagedata, personId);
                if (badd)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Info("FaceService.AddLibrary出现异常：" + ex.Message);
                return false;
            }
        }
    }
}
