using MyGL.Service.Math2D;
using MyGL.Service.Math3D;
using MyGL.Service.Textures;
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

        private void DrawStraightLine(int x1, int x2, int y, Vec2f vt1, Vec2f vt2, int z, int[,] zbuffer, Texture texture, float intensity)
        {

            int xstart = x1;
            int xfinish = x2;

            Vec2f tstart = vt1;
            Vec2f tfinish = vt2;

            if (x1 > x2)
            {
                xstart = x2;
                xfinish = x1;
                tstart = vt2;
                tfinish = vt1;
            }

            for (int x = xstart; x < xfinish; x++)
            {
                if (y < 0 || x < 0 || x >= graphicsProvider.Width || y >= graphicsProvider.Height)
                {
                    continue;
                }

                if (zbuffer[x, y] < z)
                {
                    zbuffer[x, y] = z;
                    float xt = Helper2D.InterpolateLinear(xstart,tstart.X,xfinish,tfinish.X,x);
                    float yt = Helper2D.InterpolateLinear(xstart, tstart.Y, xfinish, tfinish.Y, x);

                    Vec2i t = texture.GetUV(xt, yt);
                    
                    //TODO: Костыль 2 потому что перевернутый
                    t = new Vec2i(t.X, texture.GetHeight() - t.Y);
                    Color color = texture.GetColor(t.X, t.Y);
                    color = Color.FromArgb((int)(color.R * intensity), (int)(color.G * intensity), (int)(color.B * intensity));
                    graphicsProvider.SetPixel(x, y, color);
                }
            }

        }

        private void DrawStraightLineGradient(int x1, int x2, int y, Color c1, Color c2)
        {

            int xstart = x1;
            int xfinish = x2;

            Color xtstart = c1;

            Color xtfinish = c2;


            if (x1 > x2)
            {
                xstart = x2;
                xfinish = x1;
                xtstart = c2;
                xtfinish = c1;
            }

            for (int x = xstart; x < xfinish; x++)
            {
                if (y < 0 || x < 0 || x >= graphicsProvider.Width || y >= graphicsProvider.Height)
                {
                    continue;
                }


                float r = Helper2D.InterpolateLinear(xstart, xtstart.R, xfinish, xtfinish.R, x);
                float g = Helper2D.InterpolateLinear(xstart, xtstart.G, xfinish, xtfinish.G, x);
                float b = Helper2D.InterpolateLinear(xstart, xtstart.B, xfinish, xtfinish.B, x);

                Color color = Color.Green;
                color = Color.FromArgb((int)r, (int)g, (int)b);
                graphicsProvider.SetPixel(x, y, color);
                
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

        public void DrawTriangle(Vec3i v1, Vec3i v2, Vec3i v3, Vec2f vt1, Vec2f vt2, Vec2f vt3, Textures.Texture texture, float intensity)
        {
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
            int xleft;
            int zleft;
            int xright;
            int zright;

            //Coordinates of pixel on texture
            float xt_left;
            float xt_right;
            float yt_left;
            float yt_right;

            // Rasterize two subtriangle v1 to v2  and v2 to v3
            //Upper half of triangle

            for (int i = v1.Y; i < v2.Y; i++ )
            {
                //Get coords on screen for line
                xleft = Helper2D.InterpolateLinear(v1.Y, v1.X, v2.Y, v2.X, i);
                zleft = Helper2D.InterpolateLinear(v1.Y, v1.Z, v2.Y, v2.Z, i);

                xright = Helper2D.InterpolateLinear(v1.Y, v1.X, v3.Y, v3.X, i);
                zright = Helper2D.InterpolateLinear(v1.Y, v1.Z, v3.Y, v3.Z, i);
                

                //Get texture coord on left side
                xt_left = Helper2D.InterpolateLinear(v1.Y, vt1.X, v2.Y, vt2.X, i);
                yt_left = Helper2D.InterpolateLinear(v1.Y, vt1.Y, v2.Y, vt2.Y, i);


                //Get texture coord on right side
                xt_right = Helper2D.InterpolateLinear(v1.Y, vt1.X, v3.Y, vt3.X, i);
                yt_right = Helper2D.InterpolateLinear(v1.Y, vt1.Y, v3.Y, vt3.Y, i);

                DrawStraightLine(xleft, xright, i, new Vec2f(xt_left,yt_left), new Vec2f(xt_right,yt_right), zright, zbuffer, texture, intensity);

            }
            //Lower half
            for (int i = v2.Y; i < v3.Y; i++)
            {
                xleft = Helper2D.InterpolateLinear(v1.Y, v1.X, v3.Y, v3.X, i);
                zleft = Helper2D.InterpolateLinear(v1.Y, v1.Z, v3.Y, v3.Z, i);

                xright = Helper2D.InterpolateLinear(v2.Y, v2.X, v3.Y, v3.X, i);
                zright = Helper2D.InterpolateLinear(v2.Y, v2.Z, v3.Y, v3.Z, i);


                xt_left = Helper2D.InterpolateLinear(v1.Y, vt1.X, v3.Y, vt3.X, i);
                xt_right = Helper2D.InterpolateLinear(v1.Y, vt1.X, v3.Y, vt3.X, i);
                //Get texture coord on right side
                yt_left = Helper2D.InterpolateLinear(v2.Y, vt2.Y, v3.Y, vt3.Y, i);
                yt_right = Helper2D.InterpolateLinear(v2.Y, vt2.Y, v3.Y, vt3.Y, i);
                

                DrawStraightLine(xleft, xright, i, new Vec2f(xt_left, yt_left), new Vec2f(xt_right, yt_right), zright, zbuffer, texture, intensity);
            }
        }

        public void DrawGradientTrianlge(Vec2i v1, Vec2i v2, Vec2i v3, Color vc1, Color vc2, Color vc3)
        {
            Color color1 = vc1;
            Color color2 = vc2;
            Color color3 = vc3;


            if (v1.Y > v2.Y)
            {
                Swap(ref v1, ref v2);
                Swap(ref color1, ref color2);
            }
            if (v1.Y > v3.Y)
            {
                Swap(ref v1, ref v3);
                Swap(ref color1, ref color3);
            }
            if (v2.Y > v3.Y)
            {
                Swap(ref v2, ref v3);
                Swap(ref color2, ref color3);
            }

            int xleft;
            int xright;


            //Coordinates of pixel on texture
            int rleft;
            int gleft;
            int bleft;

            int rright;
            int gright;
            int bright;

            // Rasterize two subtriangle v1 to v2  and v2 to v3
            //Upper half of triangle

            for (int i = v1.Y; i < v2.Y; i++)
            {
                //Get coords on screen for line
                xleft = Helper2D.InterpolateLinear(v1.Y, v1.X, v2.Y, v2.X, i);

                xright = Helper2D.InterpolateLinear(v1.Y, v1.X, v3.Y, v3.X, i);


                //Get texture coord on left side
                rleft = Helper2D.InterpolateLinear(v1.Y, color1.R, v2.Y, color2.R, i);
                gleft = Helper2D.InterpolateLinear(v1.Y, color1.G, v2.Y, color2.G, i);
                bleft = Helper2D.InterpolateLinear(v1.Y, color1.B, v2.Y, color2.B, i);

                //Get texture coord on right side
                rright = Helper2D.InterpolateLinear(v1.Y, color1.R, v3.Y, color3.R, i);
                gright = Helper2D.InterpolateLinear(v1.Y, color1.G, v3.Y, color3.G, i);
                bright = Helper2D.InterpolateLinear(v1.Y, color1.B, v3.Y, color3.B, i);

                DrawStraightLineGradient(xleft, xright, i, Color.FromArgb(rleft,gleft,bleft), Color.FromArgb(rright,gright,bright));

            }
            //Lower half
            for (int i = v2.Y; i < v3.Y; i++)
            {
                xleft = Helper2D.InterpolateLinear(v1.Y, v1.X, v3.Y, v3.X, i);

                xright = Helper2D.InterpolateLinear(v2.Y, v2.X, v3.Y, v3.X, i);


                rleft = Helper2D.InterpolateLinear(v1.Y, color1.R, v3.Y, color3.R, i);
                gleft = Helper2D.InterpolateLinear(v1.Y, color1.G, v3.Y, color3.G, i);
                bleft = Helper2D.InterpolateLinear(v1.Y, color1.B, v3.Y, color3.B, i);

                //Get texture coord on right side
                rright = Helper2D.InterpolateLinear(v2.Y, color2.R, v3.Y, color3.R, i);
                gright = Helper2D.InterpolateLinear(v2.Y, color2.G, v3.Y, color3.G, i);
                bright = Helper2D.InterpolateLinear(v2.Y, color2.B, v3.Y, color3.B, i);

                DrawStraightLineGradient(xleft, xright, i, Color.FromArgb(rleft, gleft, bleft), Color.FromArgb(rright, gright, bright));
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
                    //vertexes[i] = new Vec3i(v);
                    //TODO Костыль потому что перевернутый
                    vertexes[i] = new Vec3i(new Vec3f(Width / 2, Height / 2, 0) - new Vec3f(- v.X, v.Y,-v.Z));
                }


                Vec3f normal = Vec3f.VecMul(vertexesFromMem[2] - vertexesFromMem[0], vertexesFromMem[1] - vertexesFromMem[0]);
                normal.Normalize();
                float intensity = normal * lightDirection;
                if (intensity > 1.0f)
                {
                    //Костыль
                    intensity = 1.0f;
                }

                if (intensity < 0)
                {
                    intensity = 0;
                }
                Vec2f vt1 = new Vec2f(obj.VertexesTexture[face.v1.vt - 1]);
                Vec2f vt2 = new Vec2f(obj.VertexesTexture[face.v2.vt - 1]);
                Vec2f vt3 = new Vec2f(obj.VertexesTexture[face.v3.vt - 1]);
                

                if (intensity > 0)
                    DrawTriangle(vertexes[0], vertexes[1], vertexes[2], vt1, vt2, vt3, obj.Texture,intensity);

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
