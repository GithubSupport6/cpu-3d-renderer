using MyGL.Service.Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Math4D
{
    class Vec4f
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Vec4f(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vec4f(Vec3f vec, float w)
        {
            X = vec.X;
            Y = vec.Y;
            Z = vec.Z;
            W = w;
        }

        public static Vec4f operator -(Vec4f v1, Vec4f v2)
        {
            return new Vec4f(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.W - v2.W);
        }

        public static float operator *(Vec4f v1, Vec4f v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        public Vec3f Project()
        {
            return new Vec3f(X / W, Y / W, Z / W);
        }

        public static Vec4f Mult(Vec4f vec, Matrix4f matrix)
        {
            //can be better
            float x = matrix.Get(0, 0) * vec.X + matrix.Get(1, 0) * vec.Y + matrix.Get(2, 0) * vec.Z + matrix.Get(3, 0) * vec.W;
            float y = matrix.Get(0, 1) * vec.X + matrix.Get(1, 1) * vec.Y + matrix.Get(2, 1) * vec.Z + matrix.Get(3, 1) * vec.W;
            float z = matrix.Get(0, 2) * vec.X + matrix.Get(1, 2) * vec.Y + matrix.Get(2, 2) * vec.Z + matrix.Get(3, 2) * vec.W;
            float w = matrix.Get(0, 3) * vec.X + matrix.Get(1, 3) * vec.Y + matrix.Get(2, 3) * vec.Z + matrix.Get(3, 3) * vec.W;
            return new Vec4f(x, y, z, w);
        }

    }
}
