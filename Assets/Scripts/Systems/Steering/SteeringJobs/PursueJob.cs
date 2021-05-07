using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    /// <summary>
    /// Job to calculate Pursue steering forces
    /// </summary>
    [BurstCompile]
    public struct PursueJob : IJobEntityBatch {
        
        // Component Handlers
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        [ReadOnly] public ComponentTypeHandle<TargetingData> TargetHandle;
        [ReadOnly] public ComponentTypeHandle<BoidData> BoidHandle;

        public ComponentTypeHandle<PursueData> PursueHandle;
        
        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            // Get component arrays from batch
            var pursueData = batchInChunk.GetNativeArray(PursueHandle);
            var targetData = batchInChunk.GetNativeArray(TargetHandle);
            var boidData = batchInChunk.GetNativeArray(BoidHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);

            for (int i = 0; i < batchInChunk.Count; i++) {
                // get entities components and data
                var pursue = pursueData[i];
                var target = targetData[i];
                var boid = boidData[i];
                var position = translationData[i].Value;

                // check if target exists
                if (target.Target == Entity.Null) {
                    continue;
                }
                
                // calculate pursue forces
                float distanceToTarget = math.length(target.TargetPosition - position);
                float time = distanceToTarget / boid.MaxSpeed;

                var targetPosition = target.TargetPosition + pursue.TargetVelocity * time;
                
                var desired = targetPosition - position;
                desired = math.normalize(desired);
                desired *= boid.MaxSpeed;

                pursue.Force = desired - boid.Velocity;
                
                // return data
                pursueData[i] = pursue;
            }
        }
        
    }

}