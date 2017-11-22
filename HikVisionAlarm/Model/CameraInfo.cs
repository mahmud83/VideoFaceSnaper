using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HikVisionAlarm.Model
{
    /// <summary>
    /// 摄像机对象
    /// </summary>
    [Serializable]
    public class CameraInfo : IDisposable
    {
        [XmlIgnore]
        public int UserNum
        {
            get;
            set;
        }

        [XmlAttribute("Name")]
        public string CameraName
        {
            get;
            set;
        }

        [XmlElement("CameraIp")]
        public string CameraIp
        {
            get;
            set;
        }

        [XmlElement("Port")]
        public int Port
        {
            get;
            set;
        }

        [XmlElement("User")]
        public string User
        {
            get;
            set;
        }

        [XmlElement("Password")]
        public string Password
        {
            get;
            set;
        }

        public CameraInfo()
        {
            
        }

        public CameraInfo(string cameraName, string cameraIp, int port, string user, string password)
        {
            CameraName = cameraName;
            CameraIp = cameraIp;
            Port = port;
            User = user;
            Password = password;
        }

        #region IDisposable 成员

        public void Dispose()
        {

        }

        #endregion
    }
}
