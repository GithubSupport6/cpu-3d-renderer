using MyGL.Service.Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service
{
    class LightSource
    {
        Vec3f Position { get; }

        public LightSource(Vec3f position)
        {
            Position = position;
        }
    }
}
