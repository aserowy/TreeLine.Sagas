using System;

namespace TreeLine.Messaging.Mapping
{
    internal static class TypeExtensions
    {
        public static bool IsCustom(this Type type)
        {
            return !type.Namespace.StartsWith("System", StringComparison.OrdinalIgnoreCase);
        }
    }
}
