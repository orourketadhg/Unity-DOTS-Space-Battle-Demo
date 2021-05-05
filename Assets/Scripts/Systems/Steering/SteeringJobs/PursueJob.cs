using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering.SteeringJobs {

    public struct PursueJob : IJobEntityBatch {
        
        [ReadOnly] public ComponentTypeHandle<Translation> TranslationHandle;
        [ReadOnly] public ComponentTypeHandle<TargetingData> TargetHandle;
        [ReadOnly] public ComponentTypeHandle<BoidData> BoidHandle;

        public ComponentTypeHandle<PursueData> PursueHandle;
        
        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            
            var pursueData = batchInChunk.GetNativeArray(PursueHandle);
            var targetData = batchInChunk.GetNativeArray(TargetHandle);
            var boidData = batchInChunk.GetNativeArray(BoidHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);

            for (int i = 0; i < batchInChunk.Count; i++) {
                var pursue = pursueData[i];
                var target = targetData[i];
                var boid = boidData[i];
                var position = translationData[i].Value;

                if (target.Target == Entity.Null) {
                    continue;
                }
                
                float distanceToTarget = math.length(target.TargetPosition - position);
                float time = distanceToTarget / boid.MaxSpeed;

                var targetPosition = target.TargetPosition + pursue.TargetVelocity * time;
                
                var desired = targetPosition - position;
                desired = math.normalize(desired);
                desired *= boid.MaxSpeed;

                pursue.Force = desired - boid.Velocity;
                
                pursueData[i] = pursue;
            }
        }
        
    }

}