using System;
using System.Drawing;
using System.Windows.Forms;
using VideoFace.Common;

namespace VideoFace.CoreNetApiTest
{
    public partial class Form1 : Form
    {
        CoreNetApi.CoreNetApi api = new CoreNetApi.CoreNetApi();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnStar.Enabled = false;
            btnStop.Enabled = true;
            api.FaceEvent += api_FaceEvent;
            api.FrameEvent += api_FrameEvent;
            int result = api.StartPlay(txtIp.Text, double.Parse(minTxt.Text), double.Parse(maxTxt.Text));
        }

        void api_FrameEvent(int frameId, Bitmap image, System.Drawing.Rectangle[] facerectArray)
        {
            Image oldImage = picVideo.Image;
            Bitmap newImage = new Bitmap(image, picVideo.Size);
            picVideo.Image = newImage;
            image.Dispose();
            Invoke((MethodInvoker)delegate()
            {
                richTextBox1.Text = "FrameID=" + frameId;
                int index = 0;
                foreach (var item in facerectArray)
                {
                    richTextBox1.Text += index + "人脸 x=" + item.X + " y=" + item.Y + "\n";
                }
            });
            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }

        void api_FaceEvent(int frameId, Bitmap image, int FaceId, int FaceSerial, double Score)
        {
            BeginInvoke((MethodInvoker)delegate()
            {
                Image oldImage = picFace.Image;
                labScore.Text = "分值:" + Score.ToString();
                labFaceID.Text = "人脸编号" + FaceId.ToString();
                labNumber.Text = "该人第" + FaceSerial + "张图像";
                Bitmap newImage = new Bitmap(image, picFace.Size);
                picFace.Image = newImage;
                if (Score > 0.2)
                {
                    newImage.Save(AppDomain.CurrentDomain.BaseDirectory + Score.ToString() + "_"+ FaceId.ToString() + "_"+ FaceSerial.ToString()+ ".jpg");
                }
                image.Dispose();

                if (oldImage != null)
                {
                    oldImage.Dispose();
                }
            });

        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            int result = api.EndPlay();
            btnStar.Enabled = false;
            btnStop.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStar.Enabled = true;
            btnStop.Enabled = false;
            api.InitFaceDll();
        }
    }
}
