using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service
{
    abstract class IGraphicsProvider
    {
        public abstract void SetPixel(int x, int y, Color color);

        public IGraphicsProvider(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public int Width
        {
            get;
        }

        public int Height
        {
            get;
        }
    }
}
