using Ie.TUDublin.GE2.Components.Steering;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace Ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    [BurstCompile]
    public struct WanderJob : IJobEntityBatch {
        
        [NativeSetThreadIndex] private int _nativeThreadIndex;
        [NativeDisableParallelForRestriction] public NativeArray<Random> RandomArray;
        public float DeltaTime;
        
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        [ReadOnly] public ComponentTypeHandle<Rotation> RotationHandle;
        
        public ComponentTypeHandle<WanderData> JitterWanderHandle;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            var wanderData = batchInChunk.GetNativeArray(JitterWanderHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);
            var rotationData = batchInChunk.GetNativeArray(RotationHandle);
            
            var random = RandomArray[_nativeThreadIndex];

            for (int i = 0; i < batchInChunk.Count; i++) {
                var wander = wanderData[i];
                var position = translationData[i].Value;
                var rotation = rotationData[i].Value;
                
                var displacement = wander.Jitter * random.NextFloat3Direction() * DeltaTime;
                wander.Target += displacement;
                wander.Target = math.normalize(wander.Target);
                wander.Target *= wander.Radius;
                
                wander.LocalTarget = ( new float3(0, 0, 1) * wander.Distance) + wander.Target;
                
                wander.WorldTarget = math.mul(rotation, wander.LocalTarget) + position;
                wander.Force = ( wander.WorldTarget - position );

                wanderData[i] = wander;
            }
            
            RandomArray[_nativeThreadIndex] = random;
            
        }
        
    }

}