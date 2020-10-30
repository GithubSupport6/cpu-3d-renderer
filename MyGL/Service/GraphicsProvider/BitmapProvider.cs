using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.GraphicsProvider
{
    class BitmapProvider : IGraphicsProvider
    {
        public Bitmap image
        {
            get;
        }

        public BitmapProvider(Bitmap image) : base(image.Width, image.Height)
        {
            this.image = image; 
        }

        public override void SetPixel(int x, int y, Color color)
        {
            if (x > 0 && x < Width && y > 0 && y < Height)
                image.SetPixel(x, y, color);
        }

        public void Fill(Color color)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    image.SetPixel(i, j, color);
                }
            }
        }

    }
}
