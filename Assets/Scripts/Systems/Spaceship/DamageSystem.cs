using Ie.TUDublin.GE2.Components.Spaceship;
using Ie.TUDublin.GE2.Systems.Physics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Ie.TUDublin.GE2.Systems.Spaceship {

    public class DamageSystem : SystemBase {

        public JobHandle OutDependency => Dependency;

        private StepPhysicsWorld _stepPhysicsWorld;
        private BuildPhysicsWorld _buildPhysicsWorld;

        protected override void OnCreate() {
            _stepPhysicsWorld = World.GetExistingSystem<StepPhysicsWorld>();
            _buildPhysicsWorld = World.GetExistingSystem<BuildPhysicsWorld>();
        }

        protected override void OnUpdate() {
            
            Dependency = JobHandle.CombineDependencies(_stepPhysicsWorld.FinalSimulationJobHandle, Dependency);

            var damageDataHandler = GetComponentDataFromEntity<DamageData>();
            var healthDataHandler = GetComponentDataFromEntity<HealthData>();

            var collisionDamageJob = new DamageTriggerJob() {
                damageData = damageDataHandler,
                healthData = healthDataHandler
            };
            
            collisionDamageJob.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, Dependency);

        }
        
        
    }

}