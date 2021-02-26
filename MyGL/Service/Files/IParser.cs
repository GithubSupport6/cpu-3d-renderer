
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL.Service.Files
{
    interface IParser
    {
        Object3D Parse(string path, string texturePath);
    }
}
