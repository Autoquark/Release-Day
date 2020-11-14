using Assets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector3 ToVector3(this Vector2 vector) => vector;

        public static Vector2 RotateClockwise(this Vector2 vector, float degrees) => Quaternion.Euler(0, 0, -degrees) * vector;

        public static float DegreesToUp(this Vector2 vector) => Vector2.SignedAngle(Vector2.up, vector);

        public static float DegreesFromUp(this Vector2 vector) => Vector2.SignedAngle(vector, Vector2.up);
    }
}
