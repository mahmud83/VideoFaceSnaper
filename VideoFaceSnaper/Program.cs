using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using VideoFace.Common;
using VideoFace.Common.Util;

namespace VideoFaceSnaper
{
    internal static class Program
    {
        private static int startMode = Convert.ToInt32(ConfigurationHelper.GetValue("StartMode", "2"));
        [STAThread]
        private static void Main()
        {
            //捕获未处理异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (startMode == 1)
            {
                Application.Run(new VideoSurveilance());
            }
            else
            {
                Application.Run(new VideoAlert());
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = ExceptionExtensions.GetOriginalException(e.Exception);
            Log4NetHelper.Instance.Error("程序出现错误", ex);

            Application.Exit();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            ex = ExceptionExtensions.GetOriginalException(ex);
            Log4NetHelper.Instance.Error("程序出现错误", ex);

            Application.Exit();
        }
    }
}
