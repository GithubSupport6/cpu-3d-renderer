using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Math2D
{
    class Helper2D
    {
        public static int InterpolateLinear(int x1, int y1, int x2, int y2, int x)
        {
            return (int)((float)y1 + ((float)(y2 - y1) / (float)(x2 - x1)) * (float)(x - x1));
        }

        public static float InterpolateLinear(float x1, float y1, float x2, float y2, float x)
        {
            return y1 + ((y2 - y1) / (x2 - x1)) * (x - x1);
        }

        public static Vec2i InterpolateLinearForX(Vec2i v1, Vec2i v2, int y)
        {
            int x = InterpolateLinear(v1.X, v1.Y, v2.X, v2.Y, y);
            return new Vec2i(x, y);
        }
    }
}
