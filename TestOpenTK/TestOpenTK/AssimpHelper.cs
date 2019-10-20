using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOpenTK
{
    static class AssimpHelper
    {
        public static Vector3 ToVector3(this Assimp.Vector3D v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Assimp.Vector2D XY(this Assimp.Vector3D v)
        {
            return new Assimp.Vector2D(v.X, v.Y);
        }

        public static Vector2 ToVector2(this Assimp.Vector2D v)
        {
            return new Vector2(v.X, v.Y);
        }
    }
}
