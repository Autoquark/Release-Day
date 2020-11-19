using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Common
{
    static class MathEx
    {
        public static float ClosestToZero(float a, float b) => Mathf.Abs(a) < Mathf.Abs(b) ? a : b;
    }
}
