using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Math3D
{
    class Vec3i
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Vec3i(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vec3i operator *(Vec3i v, int c)
        {
            //Не очень хорошо, надо исправить
            return new Vec3i(v.X * c, v.Y * c, v.Z * c);
        }

        public static Vec3i operator *(Vec3i v, float c)
        {
            //Не очень хорошо, надо исправить
            return new Vec3i((int)(v.X * c), (int)(v.Y * c), (int)(v.Z * c));
        }

        public static Vec3i operator +(Vec3i v, int c)
        {
            //Не очень хорошо, надо исправить
            return new Vec3i(v.X + c, v.Y + c, v.Z + c);
        }

        public static Vec3i operator +(Vec3i v1, Vec3i v2)
        {
            //Не очень хорошо, надо исправить
            return new Vec3i(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vec3i operator -(Vec3i v1, Vec3i v2)
        {
            //Не очень хорошо, надо исправить
            return new Vec3i(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public Vec3i(Vec3f v)
        {
            this.X = (int)Math.Round(v.X);
            this.Y = (int)Math.Round(v.Y);
            this.Z = (int)Math.Round(v.Z);
        }

    }
}
