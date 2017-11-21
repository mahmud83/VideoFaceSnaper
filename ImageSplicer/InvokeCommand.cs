using System;

namespace ImageSplicer
{
    internal class InvokeCommand
    {
        // 系统关闭的信号，关闭所有程序  
        private const int CTRL_SHUTDOWN_EVENT = 6;

        /// <summary>
        ///     退出程序
        /// </summary>
        public void Exit()
        {
            Console.WriteLine("确认退出该服务?(N)?  (Y/N)");
            Console.Write("JP>");
            var isExit = "n";
            isExit = Console.ReadLine();
            if (isExit.ToLower() == "y")
            {
                Program.HandlerRoutine(CTRL_SHUTDOWN_EVENT);
            }
        }

        /// <summary>
        ///     清屏
        /// </summary>
        public void Cls()
        {
            Console.Clear();
        }
    }
}