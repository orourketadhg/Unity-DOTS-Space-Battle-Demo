using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    /// <summary>
    /// Job to calculate Flee steering forces
    /// </summary>
    [BurstCompile]
    public struct FleeJob : IJobEntityBatch{
        
        // Component handlers
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        [ReadOnly] public ComponentTypeHandle<BoidData> BoidHandle;
        [ReadOnly] public BufferTypeHandle<PursuerElementData> PursuerBufferHandle;
        
        public ComponentTypeHandle<FleeData> FleeHandle;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            // Get component arrays from batch
            var fleeData = batchInChunk.GetNativeArray(FleeHandle);
            var pursuerBuffer = batchInChunk.GetBufferAccessor(PursuerBufferHandle);
            var boidData = batchInChunk.GetNativeArray(BoidHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);
            
            for (int i = 0; i < batchInChunk.Count; i++) {
                // get entities components and data
                var flee = fleeData[i];
                var boid = boidData[i];
                var position = translationData[i].Value;
                var pursuers = pursuerBuffer[i];

                // check the entity is being pursued
                if (pursuers.IsEmpty) {
                    continue;
                }

                // check the pursuer exists
                var firstPursuer = pursuers[0];
                if (firstPursuer.PursuerEntity == Entity.Null) {
                    continue;
                }

                // calculate flee forces
                var desired = firstPursuer.PursuerPosition - position;
                desired = math.normalize(desired);
                desired *= boid.MaxSpeed;

                flee.Force = -(desired - boid.Velocity);
                
                // return data
                fleeData[i] = flee;
            }
            
        }
    }

}