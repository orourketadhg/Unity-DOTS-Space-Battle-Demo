using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    /// <summary>
    /// Job to calculate Arrive steering forces
    /// </summary>
    [BurstCompile]
    public struct SeekJob : IJobEntityBatch {
        
        // Component Handlers
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        [ReadOnly] public ComponentTypeHandle<TargetingData> TargetHandle;
        [ReadOnly] public ComponentTypeHandle<BoidData> BoidHandle;

        public ComponentTypeHandle<SeekData> SeekHandle;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            // Get component arrays from batch
            var seekData = batchInChunk.GetNativeArray(SeekHandle);
            var targetData = batchInChunk.GetNativeArray(TargetHandle);
            var boidData = batchInChunk.GetNativeArray(BoidHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);

            for (int i = 0; i < batchInChunk.Count; i++) {
                // get entities components and data
                var seek = seekData[i];
                var target = targetData[i];
                var boid = boidData[i];
                var position = translationData[i].Value;

                // check target exists
                if (target.Target == Entity.Null) {
                    continue;
                }

                // calculate seek forces
                var desired = target.TargetPosition - position;
                desired = math.normalize(desired);
                desired *= boid.MaxSpeed;

                seek.Force = desired - boid.Velocity;
                
                // return data
                seekData[i] = seek;
            }
            
        }
        
    }

}