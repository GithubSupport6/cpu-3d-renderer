using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Math3D
{
    class Vec3f
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vec3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vec3f operator-(Vec3f v1,Vec3f v2)
        {
            return new Vec3f(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static float operator*(Vec3f v1, Vec3f v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public void Normalize()
        {
            float length = this.Length();
            X = X / Length();
            Y = Y / Length();
            Z = Z / Length();
        }

        public static Vec3f operator *(Vec3f v, float c)
        {
            //Не очень хорошо, надо исправить
            return new Vec3f((int)(v.X * c), (int)(v.Y * c), (int)(v.Z * c));
        }

        public static Vec3f VecMul(Vec3f v1, Vec3f v2)
        {
            return new Vec3f(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X
                );
        }

    }
}
