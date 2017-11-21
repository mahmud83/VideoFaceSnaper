using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace ImageSplicer
{
    internal class Options
    {
        // 短参数名称，长参数名称，是否是可选参数，默认值，帮助文本等
        // 第一个参数-d
        [Option('d', "dir", Required = true, HelpText = "Directory to read.")]
        public string AppDir { get; set; }

        // 第二个参数-s
        [Option('s', "step", DefaultValue = 30, HelpText = "The maximum steps in App to process.")]
        public int MaxSteps { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("ImageSplicer 2.0");
            usage.AppendLine("-d AppDir [-s MaxSteps=30]");
            return usage.ToString();
        }

    }
}
