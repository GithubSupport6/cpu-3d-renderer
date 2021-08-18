using MyGL.Service.Math2D;
using MyGL.Service.Math3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Textures
{
    class Texture
    {
        private Bitmap data;

        public Texture(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                data = new Bitmap(fs);

        }

        public Color GetColor(int x, int y)
        {
            return data.GetPixel(x, y);
        }

        public Vec2i GetUV(Vec3f v)
        {
            return new Vec2i((int)(v.X* data.Width), (int)(v.Y * data.Height));
        }
    }
}
