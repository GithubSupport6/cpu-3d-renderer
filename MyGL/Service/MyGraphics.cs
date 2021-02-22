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

        public void DrawTriangle(Vec3i v1, Vec3i v2, Vec3i v3, Color color)
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

            //Now v1 < v2 < v3

            int height = v3.Y - v1.Y;
            
            for (int y = v1.Y; y < v2.Y; y++)
            {
                int halfHeight = v2.Y - v1.Y + 1;
                float alpha = (float)(y - v1.Y) / height;
                float beta = (float)(y - v1.Y) / halfHeight;

                Vec3i A = v1 + (v3 - v1) * alpha;
                Vec3i B = v1 + (v2 - v1) * beta;
                DrawStraightLine(A.X, B.X, y, B.Z,zbuffer,color);
            }

            for (int y = v2.Y; y < v3.Y; y++)
            {
                int halfHeight = v3.Y - v2.Y + 1;
                float alpha = (float)(y - v2.Y) / height;
                float beta = (float)(y - v2.Y) / halfHeight;

                Vec3i A = v1 + (v3 - v1) * alpha;
                Vec3i B = v2 + (v3 - v2) * beta;
                DrawStraightLine(A.X, B.X, y, B.Z, zbuffer, color);
            }
        }

        public void DrawTriangle(Vec3i v1, Vec3i v2, Vec3i v3, Face face, Vec3f[] vt, Textures.Texture texture, float intensity)
        {
            Vec3f vt1 = vt[face.v1.vt] * texture.data.;
            Vec3f vt2 = vt[face.v2.vt];
            Vec3f vt3 = vt[face.v3.vt];

            if (v1.Y > v2.Y)
            {
                Swap(ref v1, ref v2);
                Swap(ref vt1,ref vt2);
            }
            if (v1.Y > v3.Y)
            {
                Swap(ref v1, ref v3);
                Swap(ref vt1, ref vt3);
            }
            if (v2.Y > v3.Y)
            {
                Swap(ref v2, ref v3);
                Swap(ref vt2, ref vt3);
            }
            int xleft = 0;
            int xright = 0;
            

            for (int i = v1.Y; i < v2.Y; i++)
            {
                xleft = Helper3D.InterpolateLinearByX(v1, v2, i);
                xright = Helper3D.InterpolateLinearByX(v1, v3, i);
                DrawStraightLine(xleft, xright, i, 0, zbuffer, Color.FromArgb((int)(intensity * 255), Color.White));
            }
            for (int i = v2.Y; i < v3.Y; i++)
            {
                xleft = Helper3D.InterpolateLinearByX(v2, v3, i);
                xright = Helper3D.InterpolateLinearByX(v1, v3, i);
                DrawStraightLine(xleft, xright, i, 0, zbuffer, Color.FromArgb((int)(intensity*255),Color.White));
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
                vertexesFromMem[0] = obj.Vertexes.ElementAt(face.v1.v - 1);
                vertexesFromMem[1] = obj.Vertexes.ElementAt(face.v2.v - 1);
                vertexesFromMem[2] = obj.Vertexes.ElementAt(face.v3.v - 1);
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
                    DrawTriangle(vertexes[0], vertexes[1], vertexes[2], face, obj.VertexesTexture, obj.Texture,intensity);

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
