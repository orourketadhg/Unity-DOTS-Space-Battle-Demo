using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Statemachine;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;

namespace ie.TUDublin.GE2.Systems.Statemachine {

    /// <summary>
    /// System to update the searching state of a ship
    /// </summary>
    public class SearchStateSystem : SystemBase {
        
        private EndSimulationEntityCommandBufferSystem _entityCommandBuffer;

        protected override void OnCreate() {
            _entityCommandBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {
            
            var ecb = _entityCommandBuffer.CreateCommandBuffer();

            Entities
                .WithName("SearchingState")
                .WithoutBurst()
                .WithNone<FleeState>()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, in TargetingData targetingData) => {

                    // check if the ship has a target, and update searching state accordingly 
                    if (targetingData.Target == Entity.Null) {
                        StatemachineUtil.TransitionToSearching(ecb, entityInQueryIndex, entity);
                    }
                    else if (targetingData.Target != Entity.Null && HasComponent<SearchState>(entity)) {
                        StatemachineUtil.TransitionFromSearching(ecb, entityInQueryIndex, entity);
                    }
                    
                }).Run();
            
            _entityCommandBuffer.AddJobHandleForProducer(Dependency);
            
        }
        
    }

}