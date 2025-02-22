using System;
using System.Collections.Generic;
using System.Linq;

namespace DI
{
    public static class ContainerExtensions
    {
        public static T Construct<T>(this Container container) where T : class
        {
            var type = typeof(T);
            var ctors = type.GetConstructors();
            if (ctors.Length == 0)
                throw new Exception($"No public constructor found for {type.Name}");
            if (ctors.Length > 1)
                throw new Exception($"More than one constructor found for {type.Name}");
            var ctor = ctors[0];
            var parameters = ctor.GetParameters();
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var paramType = parameters[i].ParameterType;

                // Handle IEnumerable<T> directly in Construct
                if (paramType.IsGenericType && paramType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    var elementType = paramType.GetGenericArguments()[0];
                    var resolvedObjects = container.ResolveAll(elementType);
                    // Convert IEnumerable<object> to IEnumerable<T>
                    var castMethod = typeof(Enumerable)
                        .GetMethod(nameof(Enumerable.Cast))
                        ?.MakeGenericMethod(elementType);

                    args[i] = castMethod?.Invoke(null, new object[] { resolvedObjects });
                }
                else
                {
                    args[i] = container.Resolve(paramType);
                }
            }

            return (T)ctor.Invoke(args);
        }
    }
}