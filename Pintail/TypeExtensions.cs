using System;
using System.Collections.Generic;
using System.Linq;

namespace Nanoray.Pintail
{
    internal static class TypeExtensions
    {
        internal static Type GetNonRefType(this Type type)
        {
            return type.IsByRef ? (type.GetElementType() ?? type) : type;
        }

        internal static ISet<Type> GetInterfacesRecursively(this Type type)
        {
            return type.GetInterfacesRecursivelyAsEnumerable().ToHashSet();
        }

        private static IEnumerable<Type> GetInterfacesRecursivelyAsEnumerable(this Type type)
        {
            foreach (Type interfaceType in type.GetInterfaces())
            {
                yield return interfaceType;
                foreach (Type recursiveInterfaceType in interfaceType.GetInterfacesRecursivelyAsEnumerable())
                {
                    yield return recursiveInterfaceType;
                }
            }
        }
    }
}