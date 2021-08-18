using MyGL.Service;
using MyGL.Service.Files;
using MyGL.Service.GraphicsProvider;
using MyGL.Service.Math2D;
using MyGL.Service.Math3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGL
{
    public partial class MainForm : Form
    {
        IGraphicsProvider provider;
        Bitmap image;
        MyGraphics graphics;
        IParser Parser = new WaveObjParser();
        int x = 1;
        int y = 1;
        float z = 0.5f;
        float dz = 0.001f;
        Object3D obj;
        Random random = new Random();
        float c = 200;

        public MainForm()
        {
            InitializeComponent();
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                MainPanel,
                new object[] { true }
            );
            MainPanel.MouseWheel += MainPanel_MouseWheel;
            KeyPreview = true;
        }

        private void MainPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            //z += e.Delta * dz;
            c += (float)e.Delta / 30;
            if (c < 1)
            {
                c = 1;
            }
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            if (obj == null)
            {
                return;
            }

            e.Graphics.Clear(Color.Black);

            image = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);

            provider = new BitmapProvider(image);
            graphics = new MyGraphics(provider);
            //graphics.Fill(Color.Black);

            int Width = e.ClipRectangle.Width;
            int Height = e.ClipRectangle.Height;

            Vec3f zero = new Vec3f(0,0,0);

            Vec3f ligth = new Vec3f(
                ((float)e.ClipRectangle.Width / 2 - x)  / (Width / 2),
                (y - (float)e.ClipRectangle.Height / 2) / (Height / 2),
                z);

            ligth = zero - ligth;

            ligth.Normalize();

            //graphics.DrawLine(new Vec2i(200, 200), new Vec2i(x, y), Color.White);
            graphics.DrawObject(obj,Color.White,ligth, c);

            //graphics.DrawGradientTrianlge(new Vec2i(10, 10), new Vec2i(250, 50), new Vec2i(300, 450), Color.Red, Color.Blue, Color.Green);

            graphics.DrawLight(ligth, Width / 2);
            e.Graphics.DrawImage(image,0,0);
        }

        private void MainPanel_MouseMove(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            MainPanel.Invalidate();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = OpenFileDialog.FileName;
                string diffuse = Path.Combine(Path.GetDirectoryName(path),Path.GetFileNameWithoutExtension(path)) + "_diffuse.bmp";
                obj = Parser.Parse(path, diffuse);
            }
        }

        private void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                z += 1f;
            }
            else if (e.KeyCode == Keys.S)
            {
                z -= 1f;
            }
            MainPanel.Refresh();
            Console.WriteLine(z);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
