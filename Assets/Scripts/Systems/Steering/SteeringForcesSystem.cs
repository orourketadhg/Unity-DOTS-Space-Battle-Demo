using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Steering;
using ie.TUDublin.GE2.Systems.Spawning;
using ie.TUDublin.GE2.Systems.Steering.SteeringJobs;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering {

    public class SteeringForcesSystem : SystemBase {

        private EntityQuery _seekQuery;
        private EntityQuery _arriveQuery;
        private EntityQuery _pursueQuery;
        private EntityQuery _fleeQuery;
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
            
            var fleeQueryDesc = new EntityQueryDesc() {
                All = new [] {
                    typeof(FleeData),
                    ComponentType.ReadOnly<BoidData>(), 
                    ComponentType.ReadOnly<TargetingData>(), 
                    ComponentType.ReadOnly<Translation>(),
                }
            };

            var wanderQueryDesc = new EntityQueryDesc() {
                All = new [] {
                    typeof(WanderData),
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
            _fleeQuery = GetEntityQuery(fleeQueryDesc);
            _wanderQuery = GetEntityQuery(wanderQueryDesc);
            _constrainQuery = GetEntityQuery(constrainQueryDesc);
            
        }

        protected override void OnUpdate() {
            
            var shipSpawningDependency = World.GetExistingSystem<ShipSpawningSystem>().OutDependency;
            Dependency = JobHandle.CombineDependencies(Dependency, shipSpawningDependency);

            // handlers and parameters
            
            var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;
            float dt = Time.DeltaTime;

            var translationHandle = GetComponentTypeHandle<Translation>(true);
            var rotationHandle = GetComponentTypeHandle<Rotation>(true);
            var boidHandle = GetComponentTypeHandle<BoidData>(true);
            var targetingHandle = GetComponentTypeHandle<TargetingData>(true);
            var pursuerBufferHandle = GetBufferTypeHandle<PursuerElementData>(true);

            var seekHandle = GetComponentTypeHandle<SeekData>(); 
            var arriveHandle = GetComponentTypeHandle<ArriveData>();
            var pursueHandle = GetComponentTypeHandle<PursueData>();
            var fleeHandle = GetComponentTypeHandle<FleeData>();
            var wanderHandle = GetComponentTypeHandle<WanderData>();
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
            
            var fleeJob = new FleeJob() {
                TranslationHandle = translationHandle,
                BoidHandle = boidHandle,
                PursuerBufferHandle = pursuerBufferHandle,
                FleeHandle = fleeHandle
            };

            var wanderJob = new WanderJob() {
                RandomArray = randomArray,
                DeltaTime = dt,

                TranslationHandle = translationHandle,
                RotationHandle = rotationHandle,
                WanderHandle = wanderHandle
            };

            var constrainJob = new ConstrainJob() {
                TranslationHandle = translationHandle,
                ConstrainHandle = constrainHandle
            };
            
            // scheduling
            Dependency = seekJob.ScheduleParallel(_seekQuery, 1, Dependency);
            Dependency = arriveJob.ScheduleParallel(_arriveQuery, 1, Dependency);
            Dependency = pursueJob.ScheduleParallel(_pursueQuery, 1, Dependency);
            Dependency = fleeJob.ScheduleParallel(_fleeQuery, 1, Dependency);
            Dependency = wanderJob.ScheduleParallel(_wanderQuery, 1, Dependency);
            Dependency = constrainJob.ScheduleParallel(_constrainQuery, 1, Dependency);
            
        }
        
    }

}