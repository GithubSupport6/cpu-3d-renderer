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
        Bitmap data;

        public Texture(string path)
        {
            using (var bitmap = Bitmap.FromFile(path))
            {
                data = (Bitmap)bitmap;
            }
        }
    }
}
