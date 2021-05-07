using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Steering;
using ie.TUDublin.GE2.Components.Tags;
using Unity.Entities;

namespace ie.TUDublin.GE2.Systems.Spaceship {

    /// <summary>
    /// Ship to destroy entities based on their health
    /// </summary>
    public class SpaceshipHealthSystem : SystemBase {

        private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;
        
        protected override void OnCreate() {
            _entityCommandBufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
            
        }

        protected override void OnUpdate() {

            var ecb = _entityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            
            Entities
                .WithName("SpaceshipCleanup")
                .WithBurst()
                .WithAll<BoidData>()
                .WithAny<AlliedTag, EnemyTag>()
                .ForEach((Entity entity, int entityInQueryIndex, in HealthData healthData) => {
                    if (healthData.Value <= 0) {
                        ecb.DestroyEntity(entityInQueryIndex, entity);
                    }
                }).ScheduleParallel();
            
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);

        }
    }

}