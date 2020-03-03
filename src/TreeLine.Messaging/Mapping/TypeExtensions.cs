using System;

namespace TreeLine.Messaging.Mapping
{
    internal static class TypeExtensions
    {
        public static bool IsCustom(this Type type)
        {
            var result = !type.Namespace.StartsWith("System.", StringComparison.OrdinalIgnoreCase);
            if (result)
            {
                result = !type.Namespace.Equals("System", StringComparison.OrdinalIgnoreCase);
            }

            return result;
        }
    }
}
