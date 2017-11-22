using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoFace.Common.Lib
{
    /// <summary>
    /// Helper class for checking media formats ( audio, video, image ).
    /// </summary>
    public class MediaFormatHelper
    {
        private static IDictionary<string, bool> _audioFormats = new Dictionary<string, bool>();
        private static IDictionary<string, bool> _videoFormats = new Dictionary<string, bool>();
        private static IDictionary<string, bool> _imageFormats = new Dictionary<string, bool>();

        /// <summary>
        /// 获取支持的音频格式
        /// </summary>
        public static string[] AudioFormats { get { return _audioFormats.Keys.ToArray(); } }
        /// <summary>
        /// 获取支持的
        /// </summary>
        public static string[] VideoFormats { get { return _videoFormats.Keys.ToArray(); } }
        public static string[] ImageFormats { get { return _imageFormats.Keys.ToArray(); } }

        /// <summary>
        /// Initialize the formats for audio, video, image.
        /// </summary>
        static MediaFormatHelper()
        {
            _audioFormats[".wav"] = true;
            _audioFormats[".mp3"] = true;
            _audioFormats[".m4p"] = true;
            _audioFormats[".wma"] = true;
            _audioFormats[".aiff"] = true;
            _audioFormats[".au"] = true;
            _audioFormats[".wav"] = true;
            _audioFormats[".mp3"] = true;
            _audioFormats[".m4p"] = true;
            _audioFormats[".wma"] = true;
            _audioFormats[".aiff"] = true;
            _audioFormats[".au"] = true;

            //_videoFormats[".mpeg"] = true;
            _videoFormats[".mpeg"] = true;
            _videoFormats[".avi"] = true;
            _videoFormats[".mov"] = true;
            _videoFormats[".wmv"] = true;
            _videoFormats[".3gp"] = true;
            _videoFormats[".mkv"] = true;
            _videoFormats[".flv"] = true;
            _videoFormats[".rmvb"] = true;
            _videoFormats[".mp4"] = true;
            _videoFormats[".mp3"] = true;

            _imageFormats[".gif"] = true;
            _imageFormats[".tiff"] = true;
            _imageFormats[".jpg"] = true;
            _imageFormats[".jpeg"] = true;
            _imageFormats[".bmp"] = true;
            _imageFormats[".png"] = true;
            _imageFormats[".raw"] = true;
            _imageFormats[".jfif"] = true;
            _imageFormats[".gif"] = true;
            _imageFormats[".tiff"] = true;
            _imageFormats[".jpg"] = true;
            _imageFormats[".jpeg"] = true;
            _imageFormats[".bmp"] = true;
            _imageFormats[".png"] = true;
            _imageFormats[".raw"] = true;
            _imageFormats[".jfif"] = true;
        }

        /// <summary>
        /// 设置文件导出或导入的图像格式
        /// </summary>
        /// <returns></returns>
        public static string GetImageFileFilter()
        {
            return "jpeg|*.jpg|bitmap|*.bmp|gif|*.gif|tiff|*.tif";
        }

        /// <summary>
        /// 设置文件导出或导入的视频格式
        /// </summary>
        /// <returns></returns>
        public static string GetVideoFileFilter()
        {
            return "视频格式|*.avi;*.mp4;*.mkv;*.divx;*.asf;*.mov;*.mpg;*.swf;*.flv;*.wmv;*.3gp;*.avchd;*.mpeg|所有格式(*.*)|*.*";
        }

        /// <summary>
        /// 设置文件选择的视频格式
        /// </summary>
        /// <returns></returns>
        public static string SelectVideoFileSmpFilter()
        {
            return "视频格式|*.avi;*.mp4;*.wmv;*.mpeg|所有格式(*.*)|*.*";
        }

        /// <summary>
        /// 设置文件选择的视频格式
        /// </summary>
        /// <returns></returns>
        public static string SelectVideoFileFilter()
        {
            return "avi格式(*.avi)|*.avi|mp4格式(*.mp4)|*.mp4|所有格式(*.*)|*.*";
        }

        /// <summary>
        /// Whether or not the format supplied is an audio format.
        /// </summary>
        /// <param name="format">Name of format.</param>
        /// <returns>True if it's an audio format.</returns>
        public static bool IsAudioFormat(string format)
        {
            if (string.IsNullOrEmpty(format)) return false;

            format = format.Trim().ToLower();
            return _audioFormats.ContainsKey(format);
        }


        /// <summary>
        /// Whether or not the format supplied is an audio format.
        /// </summary>
        /// <param name="format">Name of format.</param>
        /// <returns>True if it's a video format.</returns>
        public static bool IsVideoFormat(string format)
        {
            if (string.IsNullOrEmpty(format)) return false;

            format = format.Trim().ToLower();
            return _videoFormats.ContainsKey(format);
        }


        /// <summary>
        /// Whether or not the format supplied is an audio format.
        /// </summary>
        /// <param name="format">Name of format.</param>
        /// <returns>True if it's an image format.</returns>
        public static bool IsImageFormat(string format)
        {
            if (string.IsNullOrEmpty(format)) return false;

            format = format.Trim().ToLower();
            return _imageFormats.ContainsKey(format);
        }
    }
}
