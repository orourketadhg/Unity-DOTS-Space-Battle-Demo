using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Entities;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Spaceship {
    
    public class TargetingSystem : SystemBase {

        protected override void OnUpdate() {
            
            Entities
                .WithName("TargetingSystem")
                .WithoutBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref TargetingData targetData) => {

                    if (!EntityManager.Exists(targetData.Target) || targetData.Target == Entity.Null) {
                        return;
                    }
                    
                    targetData.TargetPosition = GetComponent<Translation>(targetData.Target).Value;
                }).Run();

            Entities
                .WithName("PursuingSystem")
                .WithoutBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref PursueData pursueData, in TargetingData targetData) => {
                    if (!EntityManager.Exists(targetData.Target) || targetData.Target == Entity.Null) {
                        return;
                    }

                    pursueData.TargetVelocity = GetComponent<BoidData>(targetData.Target).Velocity;
                }).Run();
            
            // Entities
            //     .WithName("PursuersSystem")
            //     .WithBurst()
            //     .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref DynamicBuffer<PursuerElementData> pursuersBuffer) => {
            //         var copy = pursuersBuffer.AsNativeArray();
            //         for (int i = 0; i < copy.Length; i++) {
            //             var pursuer = copy[i];
            //             pursuer.PursuerPosition = GetComponent<Translation>(pursuer.PursuerEntity).Value;
            //             copy[i] = pursuer;
            //         }
            //         pursuersBuffer.Clear();
            //         pursuersBuffer.CopyFrom(copy);
            //     }).ScheduleParallel();
            
        }
    }

}