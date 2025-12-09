using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace EntityBuilder
{
    public static class EcsUtils
    {
        private static readonly Dictionary<Type, EcsFilter> _filterCache = new();

        public static ref T AddComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();
            return ref pool.Add(entity);
        }

        public static void RemoveComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();
            pool.Del(entity);
        }

        public static bool HasComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();
            return pool.Has(entity);
        }

        public static ref T GetComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();
            return ref pool.Get(entity);
        }

        public static int GetFirstEntity<T>(this EcsWorld world) where T : struct
        {
            var pool = world.Filter<T>().End(1);
            foreach (var entity in pool)
            {
                return entity;
            }
            return -1;
        }

        public static ref T GetFirstComponent<T>(this EcsWorld world) where T : struct
        {
            var type = typeof(T);

            if (!_filterCache.TryGetValue(type, out var filter))
            {
                filter = world.Filter<T>().End(1);
                _filterCache[type] = filter;
            }

            var pool = world.GetPool<T>();
            foreach (var entity in filter)
            {
                return ref pool.Get(entity);
            }

            throw new Exception($"No entity with component {typeof(T)} found");
        }

        public static bool TryGetComponent<T>(this EcsWorld world, int entity, out T component) where T : struct
        {
            var pool = world.GetPool<T>();
            if (pool.Has(entity))
            {
                component = pool.Get(entity);
                return true;
            }

            component = default;
            return false;
        }
    }
}
