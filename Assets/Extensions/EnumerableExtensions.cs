using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    static class EnumerableExtensions
    {
        public static T MinOrDefault<T>(this IEnumerable<T> sequence) => sequence.Any() ? sequence.Min() : default;
        public static T2 MinOrDefault<T1, T2>(this IEnumerable<T1> sequence, Func<T1, T2> selector) => sequence.Any() ? sequence.Min(selector) : default;
        public static T2 MinOrDefault<T1, T2>(this IEnumerable<T1> sequence, Func<T1, T2> selector, T2 def) => sequence.Any() ? sequence.Min(selector) : def;
    }
}
