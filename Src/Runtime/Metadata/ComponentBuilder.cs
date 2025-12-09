using System;
using Leopotam.EcsLite;

namespace EntityBuilder.Metadata
{
    [Serializable]
    public abstract class ComponentBuilder
    {
        public abstract void Build(EcsWorld world, EntityView view);
        public abstract void Kill(EcsWorld world, EntityView view);
    }
}
