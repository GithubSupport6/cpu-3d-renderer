using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Math3D
{
    class Matrix4f
    {
        float[,] data { get; }

        private static float[,] proj = new float[4, 4]{
            { 1,0,0,0 },
            { 0,1,0,0 },
            { 0,0,1,0 },
            { 0,0,1,1 }
        };

        private static Matrix4f Projection = new Matrix4f(proj);

        public Matrix4f(float[,] data)
        {
            if (data.GetLength(0) != 4 || data.GetLength(1) != 4)
            {
                throw new Exception("Incorrect size of matrix");
            }
            this.data = data;
        }

        public void Set(int x,int y,float value)
        {
            this.data[y, x] = value;
        }

        public float Get(int x, int y)
        {
            if (x < 4 && y < 4)
            {
                return this.data[y, x];
            }
            else
            {
                throw new Exception("Incorrect index");
            }
        }

        public static Matrix4f Mult(Matrix4f matrix1, Matrix4f matrix2)
        {
            float[,] data = new float[4,4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0;j<4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        data[i, j] = matrix1.data[i, k] + matrix2.data[k, j];
                    }   
                }
            }
            return new Matrix4f(data);
        }

        public static Matrix4f GetProjectionMatrix(float c)
        {
            Projection.Set(2, 3, -1 / c);
            return Projection;
        }
    }
}
