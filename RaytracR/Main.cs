using System;
using System.Drawing;
using System.Windows.Forms;

namespace RaytracR
{
    public partial class Main : Form
    {
        public const string PATH = @"C:\Users\Adrien\RaytracR\";
        public const int FRAME_COUNT = 90;

        public Main()
        {
            InitializeComponent();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            Scene1.Init();
        }

        private void OnShown(object sender, EventArgs e)
        {
            RenderPreview();
        }

        private void RenderPreview()
        {
            renderBox.Image = Scene1.Render(renderBox.Width, renderBox.Height);
        }

        private void RenderVideo()
        {
            Vector3 a = Camera.main.transform.position;
            Vector3 b = Camera.main.transform.position + new Vector3(-9, 0, 0);

            Bitmap bitmap = null;

            for (int i = 0; i < FRAME_COUNT; i++)
            {
                Text = $"{((float)i / FRAME_COUNT * 100f).ToString("N1")}% ({i}/{FRAME_COUNT})";

                Vector3 pos = Vector3.Lerp(a, b, (float)i / FRAME_COUNT);
                Camera.main.transform.position = pos;

                for (int j = 0; j < Scene1.spheres.Length; j++)
                {
                    GameObject sphereGo = Scene1.spheres[j];
                    float offset = (float)Math.Sin((float)i / FRAME_COUNT * (j + 1) * 10) * 0.1f;
                    sphereGo.transform.position += new Vector3(0, 0, offset);
                }

                if (bitmap != null)
                    bitmap.Dispose();

                bitmap = Scene1.Render(renderBox.Width, renderBox.Height);
                bitmap.Save(PATH + i.ToString() + ".bmp");
                renderBox.Image = bitmap;

                Application.DoEvents();
            }
        }
    }
}