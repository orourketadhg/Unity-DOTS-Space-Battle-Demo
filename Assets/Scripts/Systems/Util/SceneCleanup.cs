using Ie.TUDublin.GE2.Components.Spaceship;
using Ie.TUDublin.GE2.Components.Tags;
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
                .WithName("DeleteTagCleanup")
                .WithBurst()
                .WithAll<DeleteTag>()
                .ForEach((Entity entity, int entityInQueryIndex) => {
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                }).ScheduleParallel();
            
            Entities
                .WithName("BulletLifetimeCleanup")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, ref ProjectileSpawnData spawnData) => {
                    if (spawnData.DoDespawn == 1 && timeElapsed >= spawnData.SpawnTime + spawnData.ProjectileLifetime) {
                        ecb.DestroyEntity(entityInQueryIndex, entity);
                    }
                }).ScheduleParallel();

            Entities
                .WithName("BulletHealthCleanup")
                .WithBurst()
                .WithAll<ProjectileSpawnData>()
                .ForEach((Entity entity, int entityInQueryIndex, in HealthData healthData) => {
                    if (healthData.Value <= 0) {
                        ecb.DestroyEntity(entityInQueryIndex, entity);
                    }
                }).ScheduleParallel();
            
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);

        }
    }

}