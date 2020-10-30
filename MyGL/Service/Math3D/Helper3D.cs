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
    }
}
