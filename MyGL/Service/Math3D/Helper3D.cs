using MyGL.Service.Math2D;
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

        /// <summary>
        /// Gets (x,y,z) where (x,z) a function of y. Differently f(y) = x and f(z) = x. Y = input
        /// </summary>
        public static Vec3i InterpolateLinearForXZ(Vec3i v1, Vec3i v2, int y)
        {
            int x = Helper2D.InterpolateLinear(v1.Y, v1.X, v2.Y, v2.X, y);
            int z = Helper2D.InterpolateLinear(v1.Y, v1.Z, v2.Y, v2.Z, y);
            return new Vec3i(x, y, z);
        }



        public static Vec3f InterpolateLinearForXZ(Vec3f v1, Vec3f v2, int y)
        {
            float x = Helper2D.InterpolateLinear(v1.Y, v1.X, v2.Y, v2.X, y);
            float z = Helper2D.InterpolateLinear(v1.Y, v1.Z, v2.Y, v2.Z, y);
            return new Vec3f(x, y, z);
        }


    }
}
