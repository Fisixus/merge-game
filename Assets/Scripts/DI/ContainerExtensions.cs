using System;

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
                args[i] = container.Resolve(parameters[i].ParameterType);
            }

            return (T)ctor.Invoke(args);
        }
    }
}