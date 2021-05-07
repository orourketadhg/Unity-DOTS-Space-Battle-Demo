using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    /// <summary>
    /// Job to calculate Arrive steering forces
    /// </summary>
    [BurstCompile]
    public struct ArriveJob : IJobEntityBatch {
        
        // Component handlers
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        [ReadOnly] public ComponentTypeHandle<TargetingData> TargetHandle;
        [ReadOnly] public ComponentTypeHandle<BoidData> BoidHandle;
        
        public ComponentTypeHandle<ArriveData> ArriveHandle;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            
            // Get component arrays from batch
            var arriveData = batchInChunk.GetNativeArray(ArriveHandle);
            var targetData = batchInChunk.GetNativeArray(TargetHandle);
            var boidData = batchInChunk.GetNativeArray(BoidHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);

            for (int i = 0; i < batchInChunk.Count; i++) {
                
                // get entities components and data
                var arrive = arriveData[i];
                var target = targetData[i];
                var boid = boidData[i];
                var position = translationData[i].Value;

                // check the ship has a target
                if (target.Target == Entity.Null) {
                    continue;
                }

                // get distance to target
                var toTarget = target.TargetPosition - position;
                float distanceToTarget = math.length(toTarget);

                // calculate arrive forces based on distance
                if (distanceToTarget > 0) {

                    float ramped = boid.MaxSpeed * ( distanceToTarget / arrive.SlowingDistance );
                    float clamped = math.min(ramped, boid.MaxSpeed);
                    var desired = clamped * ( toTarget / distanceToTarget );

                    arrive.Force = desired - boid.Velocity;

                }
                else {
                    arrive.Force = float3.zero;
                }
                
                // return data
                arriveData[i] = arrive;
            }
            
        }
        
    }

}