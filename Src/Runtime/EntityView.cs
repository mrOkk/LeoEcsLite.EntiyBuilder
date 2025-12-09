using Leopotam.EcsLite;
using UnityEngine;
using EntityBuilder.Metadata;

namespace EntityBuilder
{
    public class EntityView : MonoBehaviour
    {
        public int EntityId = -1;
        public int ParentEntityId = -1;

        public bool IsViewRoot;
        [SerializeReference]
        public ComponentBuilder[] Builders;

        public EntityView[] SubViews;

        public void Build(EcsWorld world)
        {
            Build(world, EntityId < 0 ? world.NewEntity() : EntityId);
        }

        public void Build(EcsWorld world, int entityId)
        {
            EntityId = entityId;

            foreach (var builder in Builders)
            {
                builder.Build(world, this);
            }

            foreach (var subEntityView in SubViews)
            {
                subEntityView.BuildSubView(world, EntityId);
            }
        }

        public virtual void Spawn(EcsWorld world, int spawnRequestId)
        {
            Build(world);
        }

        public virtual void EcsInit(EcsWorld world)
        {
        }

        public virtual void EcsUpdate(EcsWorld world)
        {
        }

        public virtual void EcsDestroy(EcsWorld world)
        {
            world.DelEntity(EntityId);
            EntityId = -1;
            ParentEntityId = -1;
        }

        protected virtual void BuildSubView(EcsWorld world, int parentEntityId)
        {
            ParentEntityId = parentEntityId;
            Build(world);
        }
    }
}
