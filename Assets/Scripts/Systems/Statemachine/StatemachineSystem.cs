using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Statemachine;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Entities;

namespace ie.TUDublin.GE2.Systems.Statemachine {

    public class StatemachineSystem : SystemBase {
        
        protected override void OnUpdate() {

            Entities
                .WithName("StatemachineSystem")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref SteeringData steeringData, in TargetingData targetingData) => {
                    steeringData.Constrain = 1;

                    
                    
                }).ScheduleParallel();
            
        }
    }

}