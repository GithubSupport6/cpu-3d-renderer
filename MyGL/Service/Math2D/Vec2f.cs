using MyGL.Service.Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Math2D
{
    class Vec2f
    {
        public float X
        {
            get;
        }

        public float Y
        {
            get;
        }

        public Vec2f(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vec2f(Vec3f v)
        {
            this.X = v.X;
            this.Y = v.Y;
        }

        public static Vec2f operator *(Vec2f v, int c)
        {
            //Не очень хорошо, надо исправить
            return new Vec2f(v.X * c, v.Y * c);
        }

        public static Vec2f operator +(Vec2f v, int c)
        {
            //Не очень хорошо, надо исправить
            return new Vec2f(v.X + c, v.Y + c);
        }

        public static Vec2f operator +(Vec2f v1, Vec2f v2)
        {
            //Не очень хорошо, надо исправить
            return new Vec2f(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vec2f operator -(Vec2f v1, Vec2f v2)
        {
            //Не очень хорошо, надо исправить
            return new Vec2f(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static float Distance(Vec2f x, Vec2f y)
        {
            return (float)Math.Sqrt(Math.Pow(x.X - y.X, 2) + Math.Pow(y.X - y.Y, 2));
        }

    }
}
