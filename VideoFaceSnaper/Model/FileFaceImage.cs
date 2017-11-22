using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoFace.Common.Lib;

namespace VideoFaceSnaper.Model
{
    /// <summary>
    /// 抓拍文件转换到对象
    /// </summary>
    public class FileFaceImage
    {
        public string TimeStamp { get; private set; }

        public DateTime CreateTime { get; private set; }

        public string CameraIp { get; private set; }

        public string ImgFilePath { get; private set; }

        public FileFaceImage(string timestamp, DateTime createTime, string cameraip, string filePath)
        {
            this.TimeStamp = timestamp;
            this.CreateTime = createTime;
            this.CameraIp = cameraip;
            this.ImgFilePath = filePath;
        }

    }
}
