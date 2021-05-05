using Ie.TUDublin.GE2.Components.Spaceship;
using Ie.TUDublin.GE2.Components.Steering;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    public struct FleeJob : IJobEntityBatch{
        
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        [ReadOnly] public ComponentTypeHandle<TargetingData> TargetHandle;
        [ReadOnly] public ComponentTypeHandle<BoidData> BoidHandle;

        public ComponentTypeHandle<FleeData> FleeHandle;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            var fleeData = batchInChunk.GetNativeArray(FleeHandle);
            var targetData = batchInChunk.GetNativeArray(TargetHandle);
            var boidData = batchInChunk.GetNativeArray(BoidHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);

            for (int i = 0; i < batchInChunk.Count; i++) {
                var flee = fleeData[i];
                var target = targetData[i];
                var boid = boidData[i];
                var position = translationData[i].Value;

                if (target.Target == Entity.Null) {
                    continue;
                }

                var desired = target.TargetPosition - position;
                desired = math.normalize(desired);
                desired *= boid.MaxSpeed;

                flee.Force = -(desired - boid.Velocity);
                
                fleeData[i] = flee;
            }
            
        }
    }

}