using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Extensions
{
    static class ComponentExtensions
    {
        public static bool HasComponent<T>(this Component component) => component.GetComponent<T>() != null;
    }
}
