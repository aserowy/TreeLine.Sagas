//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace TreeLine.Messaging.Typing
//{
//    public static class EnumerationBaseExtensions
//    {
//        public static IEnumerable<T> GetAll<T>(this T enumeration) where T : EnumerationBase
//        {
//            if (enumeration is null)
//            {
//                return Enumerable.Empty<T>();
//            }

//            var fields = enumeration
//                .GetType()
//                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

//            return fields.Select(fld => fld.GetValue(null)).Cast<T>();
//        }

//        public static T FromString<T>(this T enumeration, string name) where T : EnumerationBase
//        {
//            if (enumeration is null)
//            {
//                throw new ArgumentNullException(nameof(enumeration));
//            }

//            return enumeration.GetAll().Single(enmrtn => string.Equals(enmrtn.Name, name, StringComparison.OrdinalIgnoreCase));
//        }

//        public static T FromValue<T>(this T enumeration, int value) where T : EnumerationBase
//        {
//            if (enumeration is null)
//            {
//                throw new ArgumentNullException(nameof(enumeration));
//            }

//            return enumeration.GetAll().Single(enmrtn => enmrtn.Name.Equals(value));
//        }
//    }
//}
