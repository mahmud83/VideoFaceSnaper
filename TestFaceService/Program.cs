using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoFace.Common;
using VideoFace.Common.Lib;

namespace TestFaceService
{
    class Program
    {
        static  FaceService.FaceWcfServiceClient client = new FaceService.FaceWcfServiceClient();

        static void Main(string[] args)
        {
            //结果存储路径
            String outPath = AppDomain.CurrentDomain.BaseDirectory + "001.jpg";
            String outPath2 = AppDomain.CurrentDomain.BaseDirectory + "002.jpg";

            //源图片路径
            String filePath = AppDomain.CurrentDomain.BaseDirectory + @"Data\16413031.jpg";
            // TestGetFeature(filePath);
            // TestGetAvatar(filePath, outPath);
            TestGetRect(filePath, outPath);
            // TestGetCompareByFace(filePath, outPath, outPath2);
            // TestCompareByFeature(filePath, outPath, outPath2);

            String filePerson = AppDomain.CurrentDomain.BaseDirectory + @"Data\lwq2.jpg";
            String filePerson2 = AppDomain.CurrentDomain.BaseDirectory + @"Data\58060.jpg";
            // TestAddLibrary(filePerson, "lwq2");
            // TestGetCompareByFace(filePerson2, outPath, outPath2);
        }

        /// <summary>
        /// 获取人脸特征
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="outPath"></param>
        static byte[] TestGetFeature(String imagePath)
        {
            var feature = client.GetFeature(LoadImageFile(imagePath));
            return feature;
        }

        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="outPath"></param>
        static void TestGetAvatar(String imagePath, String outPath)
        {
            var imgbytes = client.GetAvatar(LoadImageFile(imagePath));
            foreach (var byt in imgbytes)
            {
                Save2ImgFile(byt, outPath);
            }
        }

        /// <summary>
        /// 人脸检测多个
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="outPath"></param>
        static void TestGetRect(String imagePath, String outPath)
        {
            var imgrects = client.GetRect(LoadImageFile(imagePath));
            if (imgrects != null && imgrects.Length > 0)
            {
                Bitmap img = new Bitmap(imagePath);
                using (Graphics g = Graphics.FromImage(img))
                {
                    Pen pen = new Pen(Brushes.Blue, 2);
                    foreach (Rectangle rect in imgrects)
                    {
                        g.DrawRectangle(pen, rect);
                    }
                }
                img.Save(outPath, ImageFormat.Jpeg);
                img.Dispose();
            }
        }

        /// <summary>
        /// 获取比对结果，通过人脸
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="outPath"></param>
        /// <param name="outPath2"></param>
        static void TestGetCompareByFace(String imagePath, String outPath, String outPath2)
        {
            byte[] imgbyte = LoadImageFile(imagePath);
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var compareFaces = client.GetCompareByFace(imgbyte);
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            Console.WriteLine(ts2.TotalMilliseconds);
            foreach (var face in compareFaces)
            {
                Save2ImgFile(face.PdbPhoto, outPath);
                Save2ImgFile(face.SourcePhoto, outPath2);
            }
            Console.WriteLine("OK!");
            Console.ReadLine();
        }

        /// <summary>
        /// 获取比对结果，通过FaceFeature
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="outPath"></param>
        /// <param name="outPath2"></param>
        static void TestCompareByFeature(String imagePath, String outPath, String outPath2)
        {
            var feature = TestGetFeature(imagePath);
            var compareFaces = client.GetCompareByFeature(feature);
            foreach (var face in compareFaces)
            {
                Save2ImgFile(face.PdbPhoto, outPath);
                Save2ImgFile(face.SourcePhoto, outPath2);
            }
            Console.WriteLine("OK!");
            Console.ReadLine();
        }

        /// <summary>
        /// 进行标准图像建模
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="personId"></param>
        static void TestAddLibrary(String imagePath, String personId)
        {
            bool badd = client.WritePdbID(LoadImageFile(imagePath), personId);
            if (badd)
            {
                Console.WriteLine("OK!");
            }
            Console.ReadLine();
        }

        /// <summary>
        /// 根据图片路径返回图片的字节流byte[]
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns>返回的字节流</returns>
        static byte[] LoadImageFile(string imagePath)
        {
            if (!File.Exists(imagePath)) return null;
            using (var files = new FileStream(imagePath, FileMode.Open))
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
        static void Save2ImgFile(byte[] byt, String outPath)
        {
            if (byt == null || byt.Length == 0) return;

            using (var ms = new MemoryStream(byt))
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                img.Save(outPath);
                img.Dispose();
            }
        }

        //public class CompareFaces
        //{
        //    /// <summary>
        //    /// 证件照
        //    /// </summary>
        //    public byte[] PdbPhoto { get; set; }
        //    /// <summary>
        //    /// 特征数据
        //    /// </summary>
        //    public byte[] PhotoFeature { get; set; }
        //    /// <summary>
        //    /// 比对结果
        //    /// </summary>
        //    public float Confidence { get; set; }
        //    /// <summary>
        //    /// 源照片
        //    /// </summary>
        //    public byte[] SourcePhoto { get; set; }
        //    /// <summary>
        //    /// 证件号码
        //    /// </summary>
        //    public string IDcardField { get; set; }
        //}

    }
}
