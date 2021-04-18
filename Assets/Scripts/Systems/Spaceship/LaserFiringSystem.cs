using Ie.TUDublin.GE2.Components.Spaceship;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Spaceship {

    public class LaserFiringSystem : SystemBase {

        private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;
        
        protected override void OnCreate() {
            _entityCommandBufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {

            float time = (float) Time.ElapsedTime;
            var ecb = _entityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

            Entities
                .WithName("LaserBlastSpawning")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, ref LaserGunInternalSettingsData gunInternalSettingsData, in LaserGunSettingsData gunSettingsData, in LocalToWorld origin) => {

                    // if (gunInternalSettingsData.IsFiringEnabled == 0) {
                    //     return;
                    // }
                    
                    if (time >= gunInternalSettingsData.TimeOfLastFire + gunSettingsData.FiringRate) {
                        gunInternalSettingsData.TimeOfLastFire = time;
                        
                        var instance = ecb.Instantiate(entityInQueryIndex, gunSettingsData.LaserPrefab);

                        var instanceTranslation = new Translation() {Value = origin.Position + (origin.Forward * 0.1f)};
                        var instanceRotation = new Rotation() {Value = origin.Rotation};
                        var instanceVelocity = new PhysicsVelocity() {
                            Linear = origin.Forward * gunSettingsData.ProjectileSpeed,
                            Angular = float3.zero
                        };

                        var projectileSpawnData = new ProjectileSpawnData() {
                            DoDespawn = 1,
                            SpawnTime = time,
                            ProjectileLifetime = gunSettingsData.ProjectileLifetime
                        };
                        
                        ecb.SetComponent(entityInQueryIndex, instance, instanceTranslation);
                        ecb.SetComponent(entityInQueryIndex, instance, instanceRotation);
                        ecb.SetComponent(entityInQueryIndex, instance, instanceVelocity);
                        ecb.SetComponent(entityInQueryIndex, instance, projectileSpawnData);
                    }
                }).ScheduleParallel();
            
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }

}