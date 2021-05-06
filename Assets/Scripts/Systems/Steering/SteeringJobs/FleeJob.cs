using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    public struct FleeJob : IJobEntityBatch{
        
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        [ReadOnly] public ComponentTypeHandle<BoidData> BoidHandle;
        [ReadOnly] public BufferTypeHandle<PursuerElementData> PursuerBufferHandle;

        public ComponentTypeHandle<FleeData> FleeHandle;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            var fleeData = batchInChunk.GetNativeArray(FleeHandle);
            var pursuerBuffer = batchInChunk.GetBufferAccessor(PursuerBufferHandle);
            var boidData = batchInChunk.GetNativeArray(BoidHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);

            for (int i = 0; i < batchInChunk.Count; i++) {
                var flee = fleeData[i];
                var boid = boidData[i];
                var position = translationData[i].Value;
                var pursuers = pursuerBuffer[i];

                if (pursuers.IsEmpty) {
                    continue;
                }

                var firstPursuer = pursuers[0];
                if (firstPursuer.PursuerEntity == Entity.Null) {
                    continue;
                }

                var desired = firstPursuer.PursuerPosition - position;
                desired = math.normalize(desired);
                desired *= boid.MaxSpeed;

                flee.Force = -(desired - boid.Velocity);
                
                fleeData[i] = flee;
            }
            
        }
    }

}