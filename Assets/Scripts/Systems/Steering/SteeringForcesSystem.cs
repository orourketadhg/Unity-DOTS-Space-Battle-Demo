using Ie.TUDublin.GE2.Components.Statemachine;
using Ie.TUDublin.GE2.Components.Steering;
using Ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Steering {

    public class SteeringForcesSystem : SystemBase {

        private EntityQuery _jitterWanderQuery;
        
        protected override void OnCreate() {

            var wanderQueryDesc = new EntityQueryDesc() {
                All = new [] {
                    typeof(JitterWanderData),
                    ComponentType.ReadOnly<Translation>(),
                    ComponentType.ReadOnly<Rotation>(),
                }
            };

            _jitterWanderQuery = GetEntityQuery(wanderQueryDesc);
        }

        protected override void OnUpdate() {

            var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;
            float dt = Time.DeltaTime;

            var translationHandle = GetComponentTypeHandle<Translation>();
            var rotationHandle = GetComponentTypeHandle<Rotation>();
            var boidHandle = GetComponentTypeHandle<BoidData>();
            var jitterWanderHandle = GetComponentTypeHandle<JitterWanderData>();

            var wanderJob = new WanderJob() {
                RandomArray = randomArray,
                DeltaTime = dt,

                JitterWanderHandle = jitterWanderHandle,
                TranslationHandle = translationHandle,
                RotationHandle = rotationHandle
            };
            
            var jitterWanderJobHandle = wanderJob.ScheduleParallel(_jitterWanderQuery, 1, Dependency);
            
            Dependency = JobHandle.CombineDependencies(Dependency, jitterWanderJobHandle);
        }
        
    }

}