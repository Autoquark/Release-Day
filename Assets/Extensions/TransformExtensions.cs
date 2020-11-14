using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Extensions
{
    static class TransformExtensions
    {
        public static IEnumerable<Transform> Children(this Transform transform) => transform.Cast<Transform>();
    }
}
