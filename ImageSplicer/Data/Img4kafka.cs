using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageSplicer.Data
{
    public class Img4kafka
    {
        [JsonProperty("vechileID")]
        public string VechileID { get; set; }

        [JsonProperty("kakouID")]
        public string KakouID { get; set; }

        [JsonProperty("deviceID")]
        public string DeviceID { get; set; }

        [JsonProperty("passTime")]
        public string PassTime { get; set; }

        [JsonProperty("roadNum")]
        public string RoadNum { get; set; }

        [JsonProperty("plateNum")]
        public string PlateNum { get; set; }

        [JsonProperty("plateColour")]
        public string PlateColour { get; set; }

        [JsonProperty("backPlateNum")]
        public string BackPlateNum { get; set; }

        [JsonProperty("backPlateColour")]
        public string BackPlateColour { get; set; }

        [JsonProperty("plateMatch")]
        public string PlateMatch { get; set; }

        [JsonProperty("speed")]
        public string Speed { get; set; }

        [JsonProperty("speedLimit")]
        public string SpeedLimit { get; set; }

        [JsonProperty("vechileLength")]
        public string VechileLength { get; set; }

        [JsonProperty("driveStatus")]
        public string DriveStatus { get; set; }

        [JsonProperty("vechileBrand")]
        public string VechileBrand { get; set; }

        [JsonProperty("vechileLook")]
        public string VechileLook { get; set; }

        [JsonProperty("vechileColour")]
        public string VechileColour { get; set; }

        [JsonProperty("vechileLogo")]
        public string VechileLogo { get; set; }

        [JsonProperty("vechileKind")]
        public string VechileKind { get; set; }

        [JsonProperty("plateType")]
        public string PlateType { get; set; }

        [JsonProperty("plateRegion")]
        public string PlateRegion { get; set; }

        [JsonProperty("pictureNum")]
        public string PictureNum { get; set; }

        [JsonProperty("jssj")]
        public string Jssj { get; set; }

        [JsonProperty("picturepath")]
        public string[] Picturepath { get; set; }

        [JsonProperty("xxbh")]
        public string Xxbh { get; set; }

        [JsonProperty("Belt")]
        public string Belt { get; set; }

        [JsonProperty("Call")]
        public string Call { get; set; }

        [JsonProperty("CDLX")]
        public string CDLX { get; set; }

        [JsonProperty("WFZT")]
        public string WFZT { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("cltz")]
        public string Cltz { get; set; }

        [JsonProperty("rect")]
        public string Rect { get; set; }

        [JsonProperty("tagnum")]
        public string Tagnum { get; set; }

        [JsonProperty("boxnum")]
        public string Boxnum { get; set; }

        [JsonProperty("caryear")]
        public string Caryear { get; set; }

        [JsonProperty("dropnum")]
        public string Dropnum { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("rksj")]
        public string Rksj { get; set; }

        [JsonProperty("fxbh")]
        public string Fxbh { get; set; }

        [JsonProperty("sbmc")]
        public string Sbmc { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("clzl")]
        public string Clzl { get; set; }

        [JsonProperty("sunflag")]
        public string Sunflag { get; set; }

        [JsonProperty("clpp")]
        public string Clpp { get; set; }

        [JsonProperty("carGNum")]
        public string CarGNum { get; set; }

        [JsonProperty("carANum")]
        public string CarANum { get; set; }

        [JsonProperty("carBNum")]
        public string CarBNum { get; set; }

        [JsonProperty("carCNum")]
        public string CarCNum { get; set; }

        [JsonProperty("carDNum")]
        public string CarDNum { get; set; }

        [JsonProperty("carENum")]
        public string CarENum { get; set; }

        [JsonProperty("carFNum")]
        public string CarFNum { get; set; }

        [JsonProperty("take")]
        public string Take { get; set; }

        [JsonProperty("tx1")]
        public string Tx1 { get; set; }

        [JsonProperty("IdentifyStatus")]
        public string IdentifyStatus { get; set; }

        [JsonProperty("frontTopLeft_x")]
        public int FrontTopLeftX { get; set; }

        [JsonProperty("frontTopLeft_y")]
        public int FrontTopLeftY { get; set; }

        [JsonProperty("frontBottomRight_x")]
        public int FrontBottomRightX { get; set; }

        [JsonProperty("frontBottomRight_y")]
        public int FrontBottomRightY { get; set; }

        [JsonProperty("backTopLeft_x")]
        public int BackTopLeftX { get; set; }

        [JsonProperty("backTopLeft_y")]
        public int BackTopLeftY { get; set; }

        [JsonProperty("backBottomRight_x")]
        public int BackBottomRightX { get; set; }

        [JsonProperty("backBottomRight_y")]
        public int BackBottomRightY { get; set; }

        [JsonProperty("IdentityTime")]
        public string IdentityTime { get; set; }

        [JsonProperty("pictureHttpPath")]
        public string[] PictureHttpPath { get; set; }

        [JsonProperty("tx2")]
        public string Tx2 { get; set; }

        [JsonProperty("hfBrandCode")]
        public string HfBrandCode { get; set; }
    }

}
