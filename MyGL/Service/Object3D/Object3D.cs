using MyGL.Service.Math3D;
using MyGL.Service.Textures;
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

        public Vec3f[] VertexesTexture
        {
            get;
        }

        public Face[] Faces
        {
            get;
        }

        public Texture Texture 
        {
            get;
        }

        public Object3D()
        {
            Vertexes = new Vec3f[0];
            Faces = new Face[0];
        }


        public Object3D(Vec3f[] vertexes, Face[] faces, Vec3f[] vertexesTexture, Texture texture = null)
        {
            this.Vertexes = vertexes;
            this.Faces = faces;
            this.Texture = texture;
            this.VertexesTexture = vertexesTexture;
            if (texture != null)
                this.Texture = texture;
        }


        public static Vec3i Triangulate(List<int> face)
        {
            throw new NotImplementedException();
        }

    }
}
