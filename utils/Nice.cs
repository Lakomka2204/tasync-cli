using System.Collections;

namespace Tasync.Utils
{
    public static class NiceExtension
    {
        public static string Nice(this IEnumerable arr)
    {
      return $"[{string.Join(", ", arr.Cast<object>())}]";
    }
    }
}
