using Assets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Common
{
    class Rotation
    {
        public static Quaternion ZeroTo(float degrees) => ZeroTo(Vector(degrees));

        public static Quaternion ZeroTo(Vector2 direction) => Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction));

        public static Vector2 Vector(float degrees) => Vector2.up.RotateClockwise(degrees);
    }
}
