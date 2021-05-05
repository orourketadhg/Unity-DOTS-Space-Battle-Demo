using Ie.TUDublin.GE2.Components.Spaceship;
using Ie.TUDublin.GE2.Components.Steering;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Steering {

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

                var desired = target.TargetPosition - position;
                desired = math.normalize(desired);
                desired *= boid.MaxSpeed;

                seek.Value = desired - boid.Velocity;
                
                seekData[i] = seek;
            }
            
        }
        
    }

}