using System;
using Leopotam.EcsLite;
using UnityEngine;
using Object = System.Object;

namespace EntityBuilder.Metadata
{
    [Serializable]
    public abstract class DefaultBuilder<T> : ComponentBuilder where T : struct
    {
        [SerializeField]
        public T Value;

        public override void Build(EcsWorld world, EntityView view)
        {
            ref var component = ref world.HasComponent<T>(view.EntityId)
                ? ref world.GetComponent<T>(view.EntityId)
                : ref world.AddComponent<T>(view.EntityId);
            component = Value;
        }

        public override void Kill(EcsWorld world, EntityView view)
        {
            // if (IsKillable && world.HasComponent<T>(view.EntityId))
            // {
            //     world.RemoveComponent<T>(view.EntityId);
            // }
        }
    }

    [Serializable]
    public abstract class DefaultTagBuilder<T> : ComponentBuilder where T : struct
    {
        public override void Build(EcsWorld world, EntityView view)
        {
            if (!world.HasComponent<T>(view.EntityId))
            {
                world.AddComponent<T>(view.EntityId);
            }
        }

        public override void Kill(EcsWorld world, EntityView view)
        {
            // if (IsKillable && world.HasComponent<T>(view.EntityId))
            // {
            //     world.RemoveComponent<T>(view.EntityId);
            // }
        }
    }

    [Serializable]
    public class DefaultNonGenericBuilder : ComponentBuilder
    {
        [SerializeField]
        [SerializeReference]
        public Object Value;
        [HideInInspector]
        public Type Type;

        public override void Build(EcsWorld world, EntityView view)
        {
            var pool = world.GetPoolByType(Type);

            if (!pool.Has(view.EntityId))
            {
                pool.AddRaw(view.EntityId, Value);
            }
        }

        public override void Kill(EcsWorld world, EntityView view)
        {
            // if (!IsKillable)
            // {
            //     return;
            // }
            //
            // var pool = world.GetPoolByType(Type);
            //
            // if (pool.Has(view.EntityId))
            // {
            //     pool.Del(view.EntityId);
            // }
        }
    }
}
