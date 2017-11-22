using HikVisionAlarm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HikVisionAlarm.Model;
using System.Threading;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.IO;
using VideoFaceSnaper.Common.Lib;

namespace HikVisionAlarmTest
{
    class Program
    {
        public delegate bool ConsoleCtrlDelegate(int dwCtrlType);

        // 关闭程序的控制标志
        private const int CTRL_CLOSE_EVENT = 2;
        // 用户退出系统时系统的关闭标志
        private const int CTRL_LOGOFF_EVENT = 5;
        // 系统关闭的信号，关闭所有程序
        private const int CTRL_SHUTDOWN_EVENT = 6;
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine, bool add);

        private static FaceDetectAlarm faceDetectAlarm = new FaceDetectAlarm();
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            
            CameraInfo camera1 = new CameraInfo("TEST1", "172.31.108.248", 8000, "admin", "1234Qwer");
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    //1、启动
                    faceDetectAlarm.Initial(null, 0.6);

                    //2、登录摄像机
                    int irc = faceDetectAlarm.LoginCamera(camera1);
                    if (irc <= 0)
                    {
                        Debug.Print("LOGIN ERROR");
                        return;
                    }

                    //3、启动布防
                    faceDetectAlarm.StartAlarm();

                    faceDetectAlarm.NoticeFaceSnapEvent += delegate (string strSnapTime, string strDevIP, string strFilePath, Rectangle rectFace)
                    {
                        Debug.Print(strSnapTime + "," + strDevIP + "," + strFilePath);
                        Console.WriteLine(strSnapTime + "," + strDevIP + "," + strFilePath + ","+ rectFace.ToString());
                    };

                    //4、启动监听
                    faceDetectAlarm.StartListen(PortHelper.GetFirstAvailablePort());

                });

                Thread.Sleep(1000);

                ConsoleCtrlDelegate newDategate = HandlerRoutine;
                var re = SetConsoleCtrlHandler(newDategate, true);
                if (!re)
                {
                    Console.WriteLine("Set SetConsoleCtrlHandler Error! ");
                }

                // Request cancellation from the UI thread.
                if (Console.ReadKey().KeyChar == 'c')
                {
                    HandlerRoutine(CTRL_CLOSE_EVENT);
                }

                // Keep the console window open while the
                // task completes its output.
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Application Exception：" + ex.Message);
                Console.ForegroundColor = ConsoleColor.Black;
            }
        }

        public static void Serialize(List<CameraInfo> list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (List<CameraInfo>));
            using (TextWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "CameraList.xml"))
            {
                serializer.Serialize(writer, list);
            }
        }

        public static List<CameraInfo> DeSerialize()
        {
            if (false == File.Exists(AppDomain.CurrentDomain.BaseDirectory + "CameraList.xml")) return null;

            XmlSerializer deserializer = new XmlSerializer(typeof(List<CameraInfo>));
            using (TextReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "CameraList.xml"))
            {
                object obj = deserializer.Deserialize(reader);
                List<CameraInfo> xmlData = (List<CameraInfo>) obj;
                reader.Close();

                return xmlData;
            }
        }

        /// <summary>
        ///     注册系统退出的事件处理
        /// </summary>
        /// <param name="ctrlType"></param>
        /// <returns></returns>
        public static bool HandlerRoutine(int ctrlType)
        {
            switch (ctrlType)
            {
                case CTRL_CLOSE_EVENT:
                case CTRL_LOGOFF_EVENT:
                case CTRL_SHUTDOWN_EVENT:
                    Console.WriteLine("Application shutting....");

                    //1、关闭
                    faceDetectAlarm.UnInitial();

                    Thread.Sleep(2000);
                    Environment.Exit(-1);
                    break;
            }
            return false;
        }

        /// <summary>
        ///     未处理异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("出现未处理异常退出:" + e.ExceptionObject, EventLogEntryType.Error);

            Thread.Sleep(5000);
            Environment.Exit(-1);
        }
    }
}
