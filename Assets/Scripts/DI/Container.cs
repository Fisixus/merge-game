using System;
using System.Collections.Generic;
using System.Linq;

namespace DI
{
    public class Container
    {
        private readonly Dictionary<Type, List<Func<object>>> _bindings = new(); // Stores all factories
        private readonly Dictionary<Type, object> _singletons = new(); // Caches singleton instances

        private void AddBinding(Type type, Func<object> factory)
        {
            if (!_bindings.ContainsKey(type))
            {
                _bindings[type] = new List<Func<object>>();
            }

            _bindings[type].Add(factory);
        }

        // Bind as Singleton
        public void BindAsSingle<T>(Func<T> factory) where T : class
        {
            AddBinding(typeof(T), () =>
            {
                // Check if singleton is already cached
                if (_singletons.TryGetValue(typeof(T), out var instance))
                    return instance;

                // Create and cache the singleton instance
                instance = factory();
                _singletons[typeof(T)] = instance;
                return instance;
            });
        }

        public void BindAsSingleNonLazy<T>(Func<T> factory) where T : class
        {
            BindAsSingle(factory);
            Resolve<T>(); // Force creation immediately
        }

        public void BindAsTransient<T>(Func<T> factory) where T : class
        {
            AddBinding(typeof(T), () => factory());
        }

        public T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            if (_bindings.TryGetValue(type, out var factories) && factories.Count > 0)
            {
                var instance = factories[0]();
                InitializeIfPreInitializable(instance);
                return instance; // Return the first registered factory
            }

            throw new Exception($"Type {type.Name} not bound in container.");
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            if (_bindings.TryGetValue(type, out var factories))
            {
                return factories.Select(factory => factory());
            }

            throw new Exception($"Type {type.Name} not bound in container.");
        }

        private void InitializeIfPreInitializable(object instance)
        {
            if (instance is IPreInitializable preInit)
            {
                preInit.PreInitialize();
            }
        }
    }
}