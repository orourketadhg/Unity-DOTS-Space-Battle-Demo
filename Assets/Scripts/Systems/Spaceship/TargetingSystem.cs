using Ie.TUDublin.GE2.Components.Spaceship;
using Ie.TUDublin.GE2.Components.Steering;
using Unity.Entities;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Spaceship {

    public class TargetingSystem : SystemBase {

        protected override void OnUpdate() {

            Entities
                .WithName("TargetingSystem")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref TargetingData targetData) => {
                    if (targetData.Target == Entity.Null) {
                        return;
                    }
                    
                    targetData.TargetPosition = GetComponent<Translation>(targetData.Target).Value;
                }).ScheduleParallel();

            Entities
                .WithName("PursuingSystem")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref PursueData pursueData, in TargetingData targetData) => {
                    if (targetData.Target == Entity.Null) {
                        return;
                    }

                    pursueData.TargetVelocity = GetComponent<BoidData>(targetData.Target).Velocity;
                }).ScheduleParallel();
        }
    }

}