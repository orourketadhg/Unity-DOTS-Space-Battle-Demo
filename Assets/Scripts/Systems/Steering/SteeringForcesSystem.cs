using Ie.TUDublin.GE2.Components.Spaceship;
using Ie.TUDublin.GE2.Components.Statemachine;
using Ie.TUDublin.GE2.Components.Steering;
using Ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Steering {

    public class SteeringForcesSystem : SystemBase {

        private EntityQuery _seekQuery;
        private EntityQuery _wanderQuery;

        protected override void OnCreate() {
            
            var seekQueryDesc = new EntityQueryDesc() {
                All = new [] {
                    typeof(wanderData),
                    ComponentType.ReadOnly<Translation>(),
                    ComponentType.ReadOnly<Rotation>(),
                }
            };

            var wanderQueryDesc = new EntityQueryDesc() {
                All = new [] {
                    typeof(wanderData),
                    ComponentType.ReadOnly<Translation>(),
                    ComponentType.ReadOnly<Rotation>(),
                }
            };
            
            _seekQuery = GetEntityQuery(seekQueryDesc);
            _wanderQuery = GetEntityQuery(wanderQueryDesc);
        }

        protected override void OnUpdate() {

            var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;
            float dt = Time.DeltaTime;

            var translationHandle = GetComponentTypeHandle<Translation>();
            var rotationHandle = GetComponentTypeHandle<Rotation>();
            var boidHandle = GetComponentTypeHandle<BoidData>();
            var targetingHandle = GetComponentTypeHandle<TargetingData>();

            var seekHandle = GetComponentTypeHandle<SeekData>(); 
            var wanderHandle = GetComponentTypeHandle<wanderData>();

            var seekJob = new SeekJob() {
                TranslationHandle = translationHandle,
                BoidHandle = boidHandle,
                TargetHandle = targetingHandle,
                SeekHandle = seekHandle
            };

            var wanderJob = new WanderJob() {
                RandomArray = randomArray,
                DeltaTime = dt,

                TranslationHandle = translationHandle,
                RotationHandle = rotationHandle,
                JitterWanderHandle = wanderHandle
            };
            
            var seekJobHandle = wanderJob.ScheduleParallel(_seekQuery, 1, Dependency);
            var wanderJobHandle = wanderJob.ScheduleParallel(_wanderQuery, 1, Dependency);
            
            Dependency = JobHandle.CombineDependencies(Dependency, seekJobHandle);
            Dependency = JobHandle.CombineDependencies(Dependency, wanderJobHandle);
        }
        
    }

}