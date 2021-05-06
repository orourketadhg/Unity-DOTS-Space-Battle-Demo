using ie.TUDublin.GE2.Components.Statemachine;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Entities;

namespace ie.TUDublin.GE2.Systems.Statemachine {

    public class StatemachineSystem : SystemBase {

        protected override void OnUpdate() {

            Entities
                .WithName("StatemachineSystem")
                .WithBurst()
                .WithAny<SearchState, AttackingState>()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref SteeringData steeringData) => {
                    steeringData.Constrain = 1;

                    // wander in search of new opponents
                    steeringData.Wander = HasComponent<SearchState>(entity) ? 1 : 0;
                    
                }).ScheduleParallel();
            
        }
        
    }

}