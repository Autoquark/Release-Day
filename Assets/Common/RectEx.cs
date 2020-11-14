using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Common
{
    static class RectEx
    {
        public static Rect FromCorners(Vector2 one, Vector2 two) => new Rect(Mathf.Min(one.x, two.x), Mathf.Min(one.y, two.y), Mathf.Abs(one.x - two.x), Mathf.Abs(one.y - two.y));

        public static Rect FromCentreSize(Vector2 centre, Vector2 size) => new Rect(centre - size / 2, size);
    }
}
