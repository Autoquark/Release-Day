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
        public static void DestroyChildren(this Transform transform)
        {
            foreach(var child in transform.Children())
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static void DestroyChild(this Transform transform, int index)
        {
            int count = 0;

            foreach (var child in transform.Children())
            {
                if (count == index)
                {
                    GameObject.Destroy(child.gameObject);
                    break;
                }

                count++;
            }
        }
    }
}
