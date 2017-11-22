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
    public class FaceVerifySDK
    {
        static FaceService.FaceWcfServiceClient client = new FaceService.FaceWcfServiceClient();
        private FaceManagerSDK _faceManager = new FaceManagerSDK();

        /// <summary>
        /// 获取比对结果，通过图像文件
        /// </summary>
        /// <param name="faceimagePath"></param>
        /// <param name="alertInfoDetails"></param>
        public bool CompareByFace(String faceimagePath, out List<HitAlertInfoDetail> alertInfoDetails)
        {
            alertInfoDetails = null;
            byte[] vbytes = ImageHelper.LoadImageFile(faceimagePath);
            if (vbytes == null)
            {
                return false;
            }
            return CompareByFace(vbytes, out alertInfoDetails);
        }

        /// <summary>
        /// 获取比对结果，通过人脸图像
        /// </summary>
        /// <param name="faceimage"></param>
        /// <param name="alertInfoDetails"></param>
        public bool CompareByFace(Image faceimage, out List<HitAlertInfoDetail> alertInfoDetails)
        {
            alertInfoDetails = null;
            byte[] vbytes = ImageHelper.ImageToBytes(faceimage);
            if (vbytes == null)
            {
                return false;
            }
            return CompareByFace(vbytes, out alertInfoDetails);
        }

        /// <summary>
        /// 获取比对结果，通过人脸数据
        /// </summary>
        /// <param name="faceimgdata"></param>
        /// <param name="alertInfoDetails"></param>
        public bool CompareByFace(byte[] faceimgdata, out List<HitAlertInfoDetail> alertInfoDetails)
        {
            alertInfoDetails = null;
            try
            {
                var compareFaces = client.GetCompareByFace(faceimgdata);
                if (compareFaces != null && compareFaces.Length > 0)
                {
                    alertInfoDetails = new List<HitAlertInfoDetail>();
                    foreach (var face in compareFaces)
                    {
                        HitAlertInfoDetail detail = new HitAlertInfoDetail(face.Confidence, string.Empty, face.PdbPhoto);
                        alertInfoDetails.Add(detail);
                    }
                    alertInfoDetails.Sort();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Info("FaceService.GetCompareByFace出现异常：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取比对结果，通过图像文件
        /// </summary>
        /// <param name="faceimagePath"></param>
        /// <param name="alertInfoDetails"></param>
        public bool CompareByFeature(String faceimagePath, out List<HitAlertInfoDetail> alertInfoDetails)
        {
            alertInfoDetails = null;

            byte[] feature;
            bool lbRet = _faceManager.GetFeature(faceimagePath, out feature);
            if (!lbRet) return false;

            return CompareByFeature(feature, out alertInfoDetails);
        }

        /// <summary>
        /// 获取比对结果，通过人脸图像
        /// </summary>
        /// <param name="faceimage"></param>
        /// <param name="alertInfoDetails"></param>
        public bool CompareByFeature(Image faceimage, out List<HitAlertInfoDetail> alertInfoDetails)
        {
            alertInfoDetails = null;

            byte[] feature;
            bool lbRet = _faceManager.GetFeature(faceimage, out feature);
            if (!lbRet) return false;

            return CompareByFeature(feature, out alertInfoDetails);
        }

        /// <summary>
        /// 获取比对结果，通过人脸特征数据
        /// </summary>
        /// <param name="featuredata"></param>
        /// <param name="alertInfoDetails"></param>
        public bool CompareByFeature(byte[] featuredata, out List<HitAlertInfoDetail> alertInfoDetails)
        {
            alertInfoDetails = null;
            try
            {
                var compareFaces = client.GetCompareByFeature(featuredata);
                if (compareFaces != null && compareFaces.Length > 0)
                {
                    alertInfoDetails = new List<HitAlertInfoDetail>();
                    foreach (var face in compareFaces)
                    {
                        HitAlertInfoDetail detail = new HitAlertInfoDetail(face.Confidence, face.IDcard, face.PdbPhoto);
                        alertInfoDetails.Add(detail);
                    }
                    alertInfoDetails.Sort();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.Info("FaceService.CompareByFeature出现异常：" + ex.Message);
                return false;
            }
        }

    }
}
