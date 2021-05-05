using ie.TUDublin.GE2.Components.Steering;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    [BurstCompile]
    public struct ConstrainJob : IJobEntityBatch {
        
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;

        public ComponentTypeHandle<ConstrainData> ConstrainHandle;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            var constrainData = batchInChunk.GetNativeArray(ConstrainHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);

            for (int i = 0; i < batchInChunk.Count; i++) {
                var constrain = constrainData[i];
                var position = translationData[i].Value;

                var force = float3.zero;
                var toTarget = position - constrain.Origin;
                float length = math.length(toTarget);

                if (length > constrain.Radius) {
                    force = math.normalize(toTarget) * ( constrain.Radius - length );
                }

                constrain.Force = force;
                
                constrainData[i] = constrain;
            }
            
        }
        
    }

}