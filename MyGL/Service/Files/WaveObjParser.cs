using MyGL.Service.Math3D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Files
{
    class WaveObjParser : IParser
    {


        

        public Object3D Parse(string path)
        {
            List<Vec3f> vertexes = new List<Vec3f>();
            List<Face> faces = new List<Face>();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var data = reader.ReadLine();

                        if (data.StartsWith("v "))
                        {
                            var vertex = data.Split(' ').Skip(1).Select(e => float.Parse(e.Replace('.',',')));
                            if (vertex.Count() > 3)
                            {
                                vertexes.Add(Helper3D.Triangulate(vertex.Skip(1).ToList()));
                            }
                            else
                                vertexes.Add(new Vec3f(vertex.ElementAt(0), vertex.ElementAt(1), vertex.ElementAt(2)));

                        }
                        else if (data.StartsWith("f "))
                        {
                            var face = data.Split(" /".ToArray()).Skip(1).Select(e => int.Parse(e));
                            int countOfSlash = face.Count() / 3;

                            List<Vertex> vertices = new List<Vertex>();
                            for (int i = 0; i < 3; i++)
                            {
                                Vertex vertex = new Vertex();
                                vertex.v = face.ElementAt(i * countOfSlash);
                                if (countOfSlash > 1)
                                {
                                    vertex.vt = face.ElementAt(i * countOfSlash + 1);
                                }
                                if (countOfSlash > 2)
                                {
                                    vertex.vn = face.ElementAt(i * countOfSlash + 2);
                                }
                                vertices.Add(vertex);
                            }


                            faces.Add(new Face(vertices.ElementAt(0), vertices.ElementAt(1), vertices.ElementAt(2)));
                        }
                    }
                }
            }
            return new Object3D(vertexes.ToArray(),faces.ToArray());
        }
    }
}
