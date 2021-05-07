using ie.TUDublin.GE2.Components.Steering;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    /// <summary>
    /// Job to calculate Constrain steering forces
    /// </summary>
    [BurstCompile]
    public struct ConstrainJob : IJobEntityBatch {
        
        // Component handlers
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        
        public ComponentTypeHandle<ConstrainData> ConstrainHandle;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            // Get component arrays from batch
            var constrainData = batchInChunk.GetNativeArray(ConstrainHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);

            for (int i = 0; i < batchInChunk.Count; i++) {
                // get entities components and data
                var constrain = constrainData[i];
                var position = translationData[i].Value;

                // calculate constrain forces
                var force = float3.zero;
                var toTarget = position - constrain.Origin;
                float length = math.length(toTarget);

                if (length > constrain.Radius) {
                    force = math.normalize(toTarget) * ( constrain.Radius - length );
                }

                constrain.Force = force;
                
                // return data
                constrainData[i] = constrain;
            }
            
        }
        
    }

}