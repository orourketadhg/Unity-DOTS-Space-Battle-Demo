using Ie.TUDublin.GE2.Components.Steering;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Steering {

    [UpdateAfter(typeof(SteeringForcesSystem))]
    public class BoidSystem : SystemBase {

        private EntityQuery _boidQuery;

        protected override void OnCreate() {

            var boidQueryDesc = new EntityQueryDesc() {
                Any = new[] {
                    typeof(Translation),
                    typeof(Rotation),
                    typeof(BoidData),
                    ComponentType.ReadOnly<ActiveSteeringData>(),
                    ComponentType.ReadOnly<SeekData>(),
                    ComponentType.ReadOnly<ArriveData>(),
                    ComponentType.ReadOnly<PursueData>(),
                    ComponentType.ReadOnly<FleeData>(),
                    ComponentType.ReadOnly<WanderData>(),
                    ComponentType.ReadOnly<ConstrainData>(),

                }
            };

            _boidQuery = GetEntityQuery(boidQueryDesc);

        }

        protected override void OnUpdate() {

            float dt = Time.DeltaTime;
            
            var seekHandle = GetComponentTypeHandle<SeekData>(); 
            var arriveHandle = GetComponentTypeHandle<ArriveData>();
            var pursueHandle = GetComponentTypeHandle<PursueData>();
            var fleeHandle = GetComponentTypeHandle<FleeData>();
            var wanderHandle = GetComponentTypeHandle<WanderData>();
            var constrainHandle = GetComponentTypeHandle<ConstrainData>();

            var translationHandle = GetComponentTypeHandle<Translation>();
            var rotationHandle = GetComponentTypeHandle<Rotation>();
            var boidHandle = GetComponentTypeHandle<BoidData>();
            var steeringHandle = GetComponentTypeHandle<ActiveSteeringData>();

            var boidJob = new BoidJob() {
                DeltaTime = dt,

                SeekHandle = seekHandle,
                ArriveHandle = arriveHandle,
                PursueHandle = pursueHandle,
                FleeHandle = fleeHandle,
                WanderHandle = wanderHandle,
                ConstrainHandle = constrainHandle,
                SteeringHandle = steeringHandle,
                TranslationHandle = translationHandle,
                RotationHandle = rotationHandle,
                BoidData = boidHandle
            };

            var boidJobHandle = boidJob.ScheduleParallel(_boidQuery, 1, Dependency);
            Dependency = JobHandle.CombineDependencies(Dependency, boidJobHandle);


        }

    }

}