using Ie.TUDublin.GE2.Components.Spaceship;
using Unity.Entities;

namespace Ie.TUDublin.GE2.Systems.Util {

    public class SceneCleanup : SystemBase {

        private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;
        
        protected override void OnCreate() {
            _entityCommandBufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
            
        }

        protected override void OnUpdate() {

            var ecb = _entityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            float timeElapsed = (float) Time.ElapsedTime;
            
            Entities
                .WithName("BulletCleanup")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, ref ProjectileSpawnData spawnData) => {
                    if (spawnData.DoDespawn == 1 && timeElapsed >= spawnData.SpawnTime + spawnData.ProjectileLifetime) {
                        ecb.DestroyEntity(entityInQueryIndex, entity);
                    }
                }).ScheduleParallel();
            
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);

        }
    }

}