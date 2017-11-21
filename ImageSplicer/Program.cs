using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageSplicer.Common;
using CommandLine;
using CommandLine.Text;

namespace ImageSplicer
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

        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        static ImgkafkaActor _imgkafkaActor = new ImgkafkaActor();
        static NotifyAnalyImager _notifyImager = new NotifyAnalyImager();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            // 判断是否已经启动过，禁止启动多个
            var runflag = false;
            var mutex = new Mutex(true, "ImageSplicer", out runflag);
            if (!runflag)
            {
                return;
            }

            Console.Title = "图像处理接口程序V2.1.0703";
            Console.ForegroundColor = ConsoleColor.Green;
            try
            {
                //1、图像处理线程
                 _notifyImager.StartListen();

                //2、数据获取线程
                _imgkafkaActor.Initial();

                Console.WriteLine("已启动正常运行!", EventLogEntryType.Warning);
                Log4NetHelper.Instance.Info("图像处理接口程序-已启动正常运行!");

                var options = new Options();
                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    string file = options.AppDir;
                    int newMaxSteps = options.MaxSteps;
                }
                else
                {
                    // Console.WriteLine(options.GetUsage());
                }

                var command = new InvokeCommand();
                while (true)
                {
                    ConsoleCtrlDelegate newDategate = HandlerRoutine;
                    var re = SetConsoleCtrlHandler(newDategate, true);
                    if (!re)
                    {
                        Console.WriteLine("Set SetConsoleCtrlHandler Error! ");
                    }

                    var lCommand = "";
                    Console.Write("JP>");
                    lCommand = Console.ReadLine();

                    if (!string.IsNullOrEmpty(lCommand))
                    {
                        var commandName = lCommand.ToLower();
                        commandName = commandName.Substring(0, 1).ToUpper() + commandName.Substring(1);
                        var method = typeof(InvokeCommand).GetMethod(commandName);
                        if (method == null)
                        {
                            Console.WriteLine("不能识别的命令,键入help查看帮助。\n");
                        }
                        else
                        {
                            try
                            {
                                method.Invoke(command, null);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                Console.WriteLine("按下回车键以退出...");
                Console.ReadLine();
                Environment.Exit(-1);
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

                    // 1、结束接收数据
                    _imgkafkaActor.Unload();

                    //2、结束处理数据
                    _notifyImager.StopListen();

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

            try
            {
                Log4NetHelper.Instance.Error("调度程序出现未处理异常退出:" + e.ExceptionObject);
            }
            catch
            {
            }

            Thread.Sleep(5000);
            Environment.Exit(-1);
        }
    }
}
