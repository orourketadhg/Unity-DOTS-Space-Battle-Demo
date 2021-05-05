using Ie.TUDublin.GE2.Components.Spaceship;
using Unity.Entities;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Spaceship {

    public class TargetingSystem : SystemBase {

        protected override void OnUpdate() {

            Entities
                .WithName("TargetingSystem")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref TargetingData targetData) => {
                    if (targetData.Target != Entity.Null) {
                        targetData.TargetPosition = GetComponent<Translation>(targetData.Target).Value;
                    }
                }).ScheduleParallel();
        }
    }

}