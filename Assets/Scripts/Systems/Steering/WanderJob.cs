using Ie.TUDublin.GE2.Components.Steering;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace Ie.TUDublin.GE2.Systems.Steering {

    [BurstCompile]
    public struct WanderJob : IJobEntityBatch {
        
        [NativeSetThreadIndex] private int _nativeThreadIndex;
        [NativeDisableParallelForRestriction] public NativeArray<Random> RandomArray;
        public float DeltaTime;
        
        public ComponentTypeHandle<JitterWander> JitterWanderHandle;
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        [ReadOnly] public ComponentTypeHandle<Rotation> RotationHandle;

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
                wander.target += displacement;
                wander.target = math.normalizesafe(wander.target);
                wander.target *= wander.Radius;

                var localTarget = ( math.forward() * wander.Distance ) + wander.Distance;
                
                var worldTarget = math.mul(rotation, localTarget) + position;
                wander.Force = ( worldTarget - position );

            }
            
            RandomArray[_nativeThreadIndex] = random;
            
        }
        
    }

}