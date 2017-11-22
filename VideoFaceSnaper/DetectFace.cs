using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace VideoFaceSnaper
{
   public  class DetectFace
   {
      public void Detect(IInputArray image, String faceFileName, List<Rectangle> faces, out long detectionTime)
      {
         Stopwatch watch;

         using (InputArray iaImage = image.GetInputArray())
         {
               //Read the HaarCascade objects
               using (CascadeClassifier face = new CascadeClassifier(faceFileName))
               {
                  watch = Stopwatch.StartNew();

                  using (UMat ugray = new UMat())
                  {
                     CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                     //normalizes brightness and increases contrast of the image
                     CvInvoke.EqualizeHist(ugray, ugray);

                     //Detect the faces  from the gray scale image and store the locations as rectangle
                     //The first dimensional is the channel
                     //The second dimension is the index of the rectangle in the specific channel                     
                     Rectangle[] facesDetected = face.DetectMultiScale(
                        ugray,
                        1.1,
                        10,
                        new Size(20, 20));

                     faces.AddRange(facesDetected);
                  }
                  watch.Stop();
               }
            }
            detectionTime = watch.ElapsedMilliseconds;
         }
   }
}
