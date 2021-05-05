using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    [BurstCompile]
    public struct ArriveJob : IJobEntityBatch {
        
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        [ReadOnly] public ComponentTypeHandle<TargetingData> TargetHandle;
        [ReadOnly] public ComponentTypeHandle<BoidData> BoidHandle;

        public ComponentTypeHandle<ArriveData> ArriveHandle;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            var arriveData = batchInChunk.GetNativeArray(ArriveHandle);
            var targetData = batchInChunk.GetNativeArray(TargetHandle);
            var boidData = batchInChunk.GetNativeArray(BoidHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);

            for (int i = 0; i < batchInChunk.Count; i++) {
                var arrive = arriveData[i];
                var target = targetData[i];
                var boid = boidData[i];
                var position = translationData[i].Value;

                if (target.Target == Entity.Null) {
                    continue;
                }

                var toTarget = target.TargetPosition - position;
                float distanceToTarget = math.length(toTarget);

                if (distanceToTarget > 0) {

                    float ramped = boid.MaxSpeed * ( distanceToTarget / arrive.SlowingDistance );
                    float clamped = math.min(ramped, boid.MaxSpeed);
                    var desired = clamped * ( toTarget / distanceToTarget );

                    arrive.Force = desired - boid.Velocity;

                }
                else {
                    arrive.Force = float3.zero;
                }
                
                arriveData[i] = arrive;
            }
            
        }
        
    }

}