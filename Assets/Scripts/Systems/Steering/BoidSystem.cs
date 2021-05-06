using ie.TUDublin.GE2.Components.Steering;
using Unity.Entities;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering {

    [UpdateAfter(typeof(SteeringForcesSystem))]
    public class BoidSystem : SystemBase {

        private EntityQuery _boidQuery;

        protected override void OnCreate() {

            var boidQueryDesc = new EntityQueryDesc() {
                Any = new[] {
                    typeof(Translation),
                    typeof(Rotation),
                    typeof(BoidData),
                    ComponentType.ReadOnly<SteeringData>(),
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
            
            var seekHandle = GetComponentTypeHandle<SeekData>(true); 
            var arriveHandle = GetComponentTypeHandle<ArriveData>(true);
            var pursueHandle = GetComponentTypeHandle<PursueData>(true);
            var fleeHandle = GetComponentTypeHandle<FleeData>(true);
            var wanderHandle = GetComponentTypeHandle<WanderData>(true);
            var constrainHandle = GetComponentTypeHandle<ConstrainData>(true);
            var steeringHandle = GetComponentTypeHandle<SteeringData>(true);

            var translationHandle = GetComponentTypeHandle<Translation>();
            var rotationHandle = GetComponentTypeHandle<Rotation>();
            var boidHandle = GetComponentTypeHandle<BoidData>();
            
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

            Dependency = boidJob.ScheduleParallel(_boidQuery, 1, Dependency);
            
        }

    }

}