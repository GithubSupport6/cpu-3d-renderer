using MyGL.Service.Math2D;
using MyGL.Service.Math3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyGL.Service
{
    class MyGraphics
    {
        Random random = new Random();

        IGraphicsProvider graphicsProvider;

        int[,] zbuffer;

        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        private void DrawStraightLine(int x1, int x2, int y, int z, int[,] zbuffer, Color color)
        {
            int xstart = Math.Min(x1, x2);
            int xfinish = Math.Max(x1, x2);



            for (int x = xstart; x < xfinish; x++)
            {
                if (y < 0 || x < 0 || x >= graphicsProvider.Width || y >= graphicsProvider.Height)
                {
                    continue;
                }

                if (zbuffer[x, y] < z)
                {
                    zbuffer[x, y] = z;
                    graphicsProvider.SetPixel(x, y, color);
                }
            }

        }

        public MyGraphics(IGraphicsProvider provider)
        {
            this.graphicsProvider = provider;
            zbuffer = new int[graphicsProvider.Width, graphicsProvider.Height];
            for (int i = 0; i < graphicsProvider.Width; i++)
            {
                for (int j = 0; j < graphicsProvider.Height; j++)
                {
                    zbuffer[i, j] = Int32.MinValue;
                }
            }
        }

        public void SetGraphicsProvider(IGraphicsProvider provider)
        {
            this.graphicsProvider = provider;
        }


        public void DrawLine(Vec2i v1,  Vec2i v2, Color color)
        {
            // Работет медленно, нужен целочисленный вариант
            int x1 = v1.X;
            int x2 = v2.X;
            int y1 = v1.Y;
            int y2 = v2.Y;

            int lenx = (int)Math.Abs(x1 - x2);
            int leny = (int)Math.Abs(y1- y2);
            bool steep = false;
            int dy = 1;

            if (lenx < leny)
            {
                Swap(ref x1, ref y1);
                Swap(ref x2, ref y2);
                Swap(ref lenx, ref leny);
                steep = true;
            }

            if (x1 > x2)
            {
                Swap(ref x1, ref x2);
                Swap(ref y2, ref y1);
            }

            float derror = 0;

            if (lenx != 0)
            {
                derror = leny / (float)lenx;
            }

            float error = 0;
            int y = y1;

            if (y2 < y1)
            {
                dy = -1;
            }

            for (int x = x1; x < x2; x++)
            {
                if (steep)
                    graphicsProvider.SetPixel(y, x,color);
                else
                    graphicsProvider.SetPixel(x, y,color);
                error += derror;
                if (error >= 1.0f)
                {
                    y += dy;
                    error -= 1.0f;
                }
            }
        }

        public void DrawTriangle(Vec3i v1, Vec3i v2, Vec3i  v3, Color color)
        {


            if (v1.Y > v2.Y)
            {
                Swap(ref v1, ref v2);
            }

            if (v1.Y > v3.Y)
            {
                Swap(ref v1, ref v3);
            }

            if (v2.Y > v3.Y)
            {
                Swap(ref v2, ref v3);
            }

            //Теперь v1 выше всех, v2 посердине, v3 внизу

            //Растеризация верхнего треугольника

            Vec3i vLeft = v2;
            Vec3i vRight = v3;
            if (vLeft.X > vRight.X)
            {
                Swap(ref vLeft, ref vRight);
            }

            float errLeft = 0;
            float errRight = 0;
            float derrLeft = Math.Abs((float)(v1.X - vLeft.X ) / (v1.Y - vLeft.Y));
            float derrRight = Math.Abs((float)(v1.X - vRight.X) / (v1.Y - vRight.Y));

            int xLeft = v1.X;
            int xRight = v1.X;

            int stepRight = 1;
            int stepLeft = 1;

            if (vLeft.X < v1.X)
            {
                stepLeft = -1;
            }
            if (vRight.X < v1.X)
            {
                stepRight = -1;
            }


            for (int y = v1.Y; y <= v2.Y; y++)
            {
                while (errLeft >= 1.0f)
                {
                    xLeft += stepLeft;
                    errLeft -= 1.0f;
                }
                while (errRight >= 1.0f)
                {
                    xRight += stepRight;
                    errRight -= 1.0f;
                }
                errLeft += derrLeft;
                errRight += derrRight;

                DrawStraightLine(xLeft-1, xRight+1, y, v1.Z, zbuffer, color);

            }

            //Растризация нижнего треугольника

            vLeft = v1;
            vRight = v2;
            if (vLeft.X > vRight.X)
            {
                Swap(ref vLeft, ref vRight);
            }

            stepRight = 1;
            stepLeft = 1;
            if (vLeft.X < v3.X)
            {
                stepLeft = -1;
            }
            if (vRight.X < v3.X)
            {
                stepRight = -1;
            }

            errLeft = 0;
            errRight = 0;
            derrLeft = Math.Abs((float)(v3.X - vLeft.X) / (v3.Y - vLeft.Y));
            derrRight = Math.Abs((float)(v3.X - vRight.X) / (v3.Y - vRight.Y));

            xLeft = v3.X;
            xRight = v3.X;

            for (int y = v3.Y; y >= v2.Y; y--)
            {
                while (errLeft >= 1.0f)
                {
                    xLeft += stepLeft;
                    errLeft -= 1.0f;
                }
                while (errRight >= 1.0f)
                {
                    xRight += stepRight;
                    errRight -= 1.0f;
                }
                errLeft += derrLeft;
                errRight += derrRight;
                DrawStraightLine(xLeft-1, xRight+1, y, v1.Z, zbuffer, color);
            }
            
        }

        public void DrawObject(Object3D obj, Color color, Vec3f lightDirection, float c = 5)
        {
            var Width = graphicsProvider.Width;
            var Height = graphicsProvider.Height;
            var vertexesFromMem = new Vec3f[3];
            var vertexes = new Vec3i[3];

            for (int i = 0; i < graphicsProvider.Width; i++)
            {
                for (int j = 0; j < graphicsProvider.Height; j++)
                {
                    zbuffer[i, j] = Int32.MinValue;
                }
            }


            foreach (var face in obj.Faces)
            {
                //Работает медленно
                vertexesFromMem[0] = (obj.Vertexes.ElementAt(face.X - 1));
                vertexesFromMem[1] = (obj.Vertexes.ElementAt(face.Y - 1));
                vertexesFromMem[2] = (obj.Vertexes.ElementAt(face.Z - 1));
                for (int i = 0;i<vertexes.Length; i++)
                {
                    Vec3f v = vertexesFromMem[i];
                    v *= c;
                    vertexes[i] = new Vec3i(new Vec3f(Width / 2, Height / 2, 0) - new Vec3f(- v.X,v.Y,- v.Z));
                }

                //color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                Vec3f normal = Vec3f.VecMul(vertexesFromMem[2] - vertexesFromMem[0], vertexesFromMem[1] - vertexesFromMem[0]);
                normal.Normalize();
                float intensity = normal * lightDirection;
                if (intensity > 1.0f)
                {
                    //Костыль
                    intensity = 1.0f;
                }
                if (intensity > 0)
                    DrawTriangle(vertexes[0], vertexes[1], vertexes[2], Color.FromArgb(
                        (int) (color.R * intensity),
                        (int)(color.G * intensity),
                        (int)(color.B * intensity)
                    ));

            }
        }

        public void DrawObject(Object3D obj, Color color, int c = 5)
        {
            var Width = graphicsProvider.Width;
            var Height = graphicsProvider.Height;
            var vertexes = new Vec3f[3];


            foreach (var face in obj.Faces)
            {

                //Работает медленно
                vertexes[0] = (obj.Vertexes.ElementAt(face.X - 1));
                vertexes[1] = (obj.Vertexes.ElementAt(face.Y - 1));
                vertexes[2] = (obj.Vertexes.ElementAt(face.Z - 1));


                for (int vertexIndex = 0; vertexIndex < 3; vertexIndex++)
                {
                    Vec2i v1 = new Vec2i(
                        (int)((vertexes.ElementAt(vertexIndex).X + 1) * c + Width / 2),
                        (int)((vertexes.ElementAt(vertexIndex).Y + 1) * c + Height / 2)
                        );

                    Vec2i v2 = new Vec2i(
                        (int)((vertexes.ElementAt((vertexIndex + 1) % 2).X + 1) * c + Width / 2),
                        (int)((vertexes.ElementAt((vertexIndex + 1) % 2).Y + 1) * c + Height / 2)
                        );

                    DrawLine(v1,v2,color);
                }
            }
        }

        public void DrawLight(Vec3f light, int c = 5)
        {


            graphicsProvider.SetPixel(
                (int)(light.X * c + graphicsProvider.Width / 2),
                (int)(light.Y * c + graphicsProvider.Height / 2),
                Color.Red);
        }
    }
}
