using MyGL.Service.Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service
{
    class Object3D
    {
        public Vec3f[] Vertexes
        {
            get;
        }
        public Vec3i[] Faces
        {
            get;
        }

        public Object3D()
        {
            Vertexes = new Vec3f[0];
            Faces = new Vec3i[0];
        }

        public Object3D(Vec3f[] vertexes, Vec3i[] faces)
        {
            this.Vertexes = vertexes;
            this.Faces = faces;
        }

        public static Vec3i Triangulate(List<int> face)
        {
            throw new NotImplementedException();
        }

    }
}
