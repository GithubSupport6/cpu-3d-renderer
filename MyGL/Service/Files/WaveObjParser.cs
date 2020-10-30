using MyGL.Service.Math3D;
using System;
using System.Collections.Generic;
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
            List<Vec3i> faces = new List<Vec3i>();

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
                            faces.Add(new Vec3i(face.ElementAt(0), face.ElementAt(3), face.ElementAt(6)));
                        }
                    }
                }
            }
            return new Object3D(vertexes.ToArray(),faces.ToArray());
        }
    }
}
