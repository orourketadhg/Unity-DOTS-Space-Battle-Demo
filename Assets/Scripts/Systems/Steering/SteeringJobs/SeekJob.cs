using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    [BurstCompile]
    public struct SeekJob : IJobEntityBatch {
        
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        [ReadOnly] public ComponentTypeHandle<TargetingData> TargetHandle;
        [ReadOnly] public ComponentTypeHandle<BoidData> BoidHandle;

        public ComponentTypeHandle<SeekData> SeekHandle;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            var seekData = batchInChunk.GetNativeArray(SeekHandle);
            var targetData = batchInChunk.GetNativeArray(TargetHandle);
            var boidData = batchInChunk.GetNativeArray(BoidHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);

            for (int i = 0; i < batchInChunk.Count; i++) {
                var seek = seekData[i];
                var target = targetData[i];
                var boid = boidData[i];
                var position = translationData[i].Value;

                if (target.Target == Entity.Null) {
                    continue;
                }

                var desired = target.TargetPosition - position;
                desired = math.normalize(desired);
                desired *= boid.MaxSpeed;

                seek.Force = desired - boid.Velocity;
                
                seekData[i] = seek;
            }
            
        }
        
    }

}