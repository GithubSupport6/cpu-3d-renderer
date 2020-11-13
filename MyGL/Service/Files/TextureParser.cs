using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Files
{
    class TextureParser
    {
        Bitmap data;

        public void Parse(string path)
        {
            data = (Bitmap)Bitmap.FromFile(path);
        }
    }
}
