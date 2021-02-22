using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Math3D
{
    class Helper3D
    {
        public static Vec3f Triangulate(List<float> elements)
        {
            return new Vec3f(elements.ElementAt(0), elements.ElementAt(1), elements.ElementAt(2));
        }

        public static int InterpolateLinear(int x1, int y1, int x2, int y2, int x)
        {
            return (int)((float)y1 + ((float)(y2 - y1) / (float)(x2 - x1)) * (float)(x - x1));
        }

        public static Vec3i InterpolateLinearForXZ(Vec3i v1, Vec3i v2, int y)
        {
            int x = InterpolateLinear(v1.Y, v1.X, v2.Y, v2.X, y);
            int z = InterpolateLinear(v1.Y, v1.Z, v2.Y, v2.Z, y);
            return new Vec3i(x, y, z);
        }
    }
}
