using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.GraphicsProvider
{
    class WinFormsProvider : IGraphicsProvider
    {

        Graphics graphics;
        Brush brush;

        public WinFormsProvider(int Width, int Height, Graphics graphics) : base (Width,Height)
        {
            this.graphics = graphics;
            graphics.Clear(Color.Black);
            brush = new SolidBrush(Color.White);
        }

        public void SetGraphics(Graphics graphics)
        {
            this.graphics = graphics;
            
        }

        public void SetColor(Color color)
        {
            brush = new SolidBrush(color);
        }

        public override void SetPixel(int x, int y, Color color)
        {
            graphics.FillRectangle(brush, x, y, 1, 1);
        }
    }
}
