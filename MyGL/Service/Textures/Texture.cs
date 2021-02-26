using MyGL.Service.Math2D;
using MyGL.Service.Math3D;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            using (var bitmap = Bitmap.FromFile(path))
            {
                data = (Bitmap)bitmap;
            }
        }

        public Color GetColor(int x, int y)
        {
            return data.GetPixel(x, y);
        }

        public Vec2i GetUV(Vec3f v)
        {
            return new Vec2i(new Vec2f(v.X* data.Width, (int)v.Y * data.Height));
        }
    }
}
