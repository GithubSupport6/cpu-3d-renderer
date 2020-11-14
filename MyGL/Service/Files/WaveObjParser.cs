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

        private Vec3f ParseVertexes(string data)
        {
            var vertex = data.Split(' ').Skip(1).Select(e => float.Parse(e.Replace('.', ',')));
            if (vertex.Count() > 3)
            {
                return Helper3D.Triangulate(vertex.Skip(1).ToList());
            }
            else
                return new Vec3f(vertex.ElementAt(0), vertex.ElementAt(1), vertex.ElementAt(2));
        }

        private Face ParseFaces(string data)
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
            return new Face(vertices.ElementAt(0), vertices.ElementAt(1), vertices.ElementAt(2));
        }

        private Vec3f ParseTexture(string data)
        {
            var vertex = data.Split(' ')
                               .Where(e => e != "")
                               .Skip(1)
                               .Select(e => float.Parse(e.Replace('.', ',')));

            if (vertex.Count() > 3)
            {
                return Helper3D.Triangulate(vertex.Skip(1).ToList());
            }
            else
                return new Vec3f(vertex.ElementAt(0), vertex.ElementAt(1), vertex.ElementAt(2));
        }

        public Object3D Parse(string path)
        {
            List<Vec3f> vertexes = new List<Vec3f>();
            List<Face> faces = new List<Face>();
            List<Vec3f> vTexture = new List<Vec3f>();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var data = reader.ReadLine();

                        if (data.StartsWith("v "))
                        {
                           vertexes.Add(ParseVertexes(data));
                        }
                        else if (data.StartsWith("vt "))
                        {
                           vTexture.Add(ParseTexture(data));
                        }
                        else if (data.StartsWith("f "))
                        {
                          faces.Add(ParseFaces(data));
                        }

                    }
                }
            }
            return new Object3D(vertexes.ToArray(), faces.ToArray(), vTexture.ToArray());
        }                 
    }
}
