using Ie.TUDublin.GE2.Components.Spaceship;
using Ie.TUDublin.GE2.Components.Steering;
using Ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Steering {

    public class SteeringForcesSystem : SystemBase {

        private EntityQuery _seekQuery;
        private EntityQuery _arriveQuery;
        private EntityQuery _pursueQuery;
        private EntityQuery _wanderQuery;
        private EntityQuery _constrainQuery;

        protected override void OnCreate() {
            
            // query descriptions
            var seekQueryDesc = new EntityQueryDesc() {
                All = new [] {
                    typeof(SeekData),
                    ComponentType.ReadOnly<BoidData>(), 
                    ComponentType.ReadOnly<TargetingData>(), 
                    ComponentType.ReadOnly<Translation>(),
                }
            };
            
            var arriveQueryDesc = new EntityQueryDesc() {
                All = new [] {
                    typeof(ArriveData),
                    ComponentType.ReadOnly<BoidData>(), 
                    ComponentType.ReadOnly<TargetingData>(), 
                    ComponentType.ReadOnly<Translation>(),
                }
            };
            
            var pursueQueryDesc = new EntityQueryDesc() {
                All = new [] {
                    typeof(PursueData),
                    ComponentType.ReadOnly<BoidData>(), 
                    ComponentType.ReadOnly<TargetingData>(), 
                    ComponentType.ReadOnly<Translation>(),
                }
            };

            var wanderQueryDesc = new EntityQueryDesc() {
                All = new [] {
                    typeof(wanderData),
                    ComponentType.ReadOnly<Translation>(),
                    ComponentType.ReadOnly<Rotation>(),
                }
            };
            
            var constrainQueryDesc = new EntityQueryDesc() {
                All = new [] {
                    typeof(ConstrainData),
                    ComponentType.ReadOnly<Translation>(),
                }
            };
            
            // queries
            _seekQuery = GetEntityQuery(seekQueryDesc);
            _arriveQuery = GetEntityQuery(arriveQueryDesc);
            _pursueQuery = GetEntityQuery(pursueQueryDesc);
            _wanderQuery = GetEntityQuery(wanderQueryDesc);
            _constrainQuery = GetEntityQuery(constrainQueryDesc);
            
        }

        protected override void OnUpdate() {

            // handlers and parameters
            
            var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;
            float dt = Time.DeltaTime;

            var translationHandle = GetComponentTypeHandle<Translation>();
            var rotationHandle = GetComponentTypeHandle<Rotation>();
            var boidHandle = GetComponentTypeHandle<BoidData>();
            var targetingHandle = GetComponentTypeHandle<TargetingData>();

            var seekHandle = GetComponentTypeHandle<SeekData>(); 
            var arriveHandle = GetComponentTypeHandle<ArriveData>();
            var pursueHandle = GetComponentTypeHandle<PursueData>();
            var wanderHandle = GetComponentTypeHandle<wanderData>();
            var constrainHandle = GetComponentTypeHandle<ConstrainData>();

            // job declarations
            var seekJob = new SeekJob() {
                TranslationHandle = translationHandle,
                BoidHandle = boidHandle,
                TargetHandle = targetingHandle,
                SeekHandle = seekHandle
            };
            
            var arriveJob = new ArriveJob() {
                TranslationHandle = translationHandle,
                BoidHandle = boidHandle,
                TargetHandle = targetingHandle,
                ArriveHandle = arriveHandle
            };
            
            var pursueJob = new PursueJob() {
                TranslationHandle = translationHandle,
                BoidHandle = boidHandle,
                TargetHandle = targetingHandle,
                PursueHandle = pursueHandle
            };

            var wanderJob = new WanderJob() {
                RandomArray = randomArray,
                DeltaTime = dt,

                TranslationHandle = translationHandle,
                RotationHandle = rotationHandle,
                JitterWanderHandle = wanderHandle
            };

            var constrainJob = new ConstrainJob() {
                TranslationHandle = translationHandle,
                ConstrainHandle = constrainHandle
            };
            
            // scheduling
            var seekJobHandle = seekJob.ScheduleParallel(_seekQuery, 1, Dependency);
            var arriveJobHandle = arriveJob.ScheduleParallel(_arriveQuery, 1, Dependency);
            var pursueJobHandle = pursueJob.ScheduleParallel(_pursueQuery, 1, Dependency);
            var wanderJobHandle = wanderJob.ScheduleParallel(_wanderQuery, 1, Dependency);
            var constrainJobHandle = constrainJob.ScheduleParallel(_constrainQuery, 1, Dependency);
            
            // dependencies
            Dependency = JobHandle.CombineDependencies(Dependency, seekJobHandle);
            Dependency = JobHandle.CombineDependencies(Dependency, arriveJobHandle);
            Dependency = JobHandle.CombineDependencies(Dependency, pursueJobHandle);
            Dependency = JobHandle.CombineDependencies(Dependency, wanderJobHandle);
            Dependency = JobHandle.CombineDependencies(Dependency, constrainJobHandle);
        }
        
    }

}