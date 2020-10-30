using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Math2D
{
    class Vec2i
    {
        static int DerPow2(int x, int y)
        {
            return (x - y) * (x-y );
        }

        public int X
        {
            get;
        }

        public int Y
        {
            get;
        }

        public Vec2i(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vec2i operator*(Vec2i v, int c)
        {
            //Не очень хорошо, надо исправить
            return new Vec2i(v.X * c, v.Y * c);
        }

        public static Vec2i operator *(Vec2i v, float c)
        {
            //Не очень хорошо, надо исправить
            return new Vec2i( (int)(v.X * c), (int)(v.Y * c));
        }

        public static Vec2i operator +(Vec2i v, int c)
        {
            //Не очень хорошо, надо исправить
            return new Vec2i(v.X + c, v.Y + c);
        }

        public static Vec2i operator +(Vec2i v1, Vec2i v2)
        {
            //Не очень хорошо, надо исправить
            return new Vec2i(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vec2i operator -(Vec2i v1, Vec2i v2)
        {
            //Не очень хорошо, надо исправить
            return new Vec2i(v1.X - v2.X, v1.Y - v2.Y);
        }

        public Vec2i(Vec2f v)
        {
            this.X = (int)Math.Round(v.X);
            this.Y = (int)Math.Round(v.Y);
        }

        public static int Distance(Vec2i x, Vec2i y)
        {
            return (int)Math.Sqrt(DerPow2(x.X,y.X) + DerPow2(y.X,y.Y));
        }

    }
}
