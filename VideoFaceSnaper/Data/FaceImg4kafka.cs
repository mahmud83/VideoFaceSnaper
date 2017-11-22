using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VideoFaceSnaper.Data
{
    public class FaceImg4kafka
    {
        [JsonProperty("ImgNum")]
        public string ImgNum { get; set; }

        [JsonProperty("CameraId")]
        public string CameraId { get; set; }

        [JsonProperty("CameraIp")]
        public string CameraIp { get; set; }

        [JsonProperty("PassTime")]
        public string PassTime { get; set; }

        [JsonProperty("FaceUrl")]
        public string FaceUrl { get; set; }
    }

}
