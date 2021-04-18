using Ie.TUDublin.GE2.Components.Spaceship;
using Ie.TUDublin.GE2.Systems.Physics;
using Ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Ie.TUDublin.GE2.Systems.Spaceship {

    [UpdateBefore(typeof(SceneCleanupSystem))]
    public class DamageSystem : SystemBase {

        public JobHandle OutDependency => Dependency;

        public EndSimulationEntityCommandBufferSystem _entityCommandBuffer;

        private StepPhysicsWorld _stepPhysicsWorld;
        private BuildPhysicsWorld _buildPhysicsWorld;

        protected override void OnCreate() {
            _stepPhysicsWorld = World.GetExistingSystem<StepPhysicsWorld>();
            _buildPhysicsWorld = World.GetExistingSystem<BuildPhysicsWorld>();

            _entityCommandBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {
            
            Dependency = JobHandle.CombineDependencies(_stepPhysicsWorld.FinalSimulationJobHandle, Dependency);

            var ecb = _entityCommandBuffer.CreateCommandBuffer();

            var damageDataHandler = GetComponentDataFromEntity<DamageData>();
            var healthDataHandler = GetComponentDataFromEntity<HealthData>();

            var collisionDamageJob = new DamageTriggerJob() {
                ecb = ecb,
                damageData = damageDataHandler,
                healthData = healthDataHandler
            };
            
            var collisionDamageJobHandle = collisionDamageJob.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, Dependency);
            Dependency = JobHandle.CombineDependencies(Dependency, collisionDamageJobHandle);
            
            _entityCommandBuffer.AddJobHandleForProducer(Dependency);

        }
        
        
    }

}