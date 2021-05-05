using Ie.TUDublin.GE2.Components.Spaceship;
using Ie.TUDublin.GE2.Components.Statemachine;
using Ie.TUDublin.GE2.Systems.Spaceship;
using Ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;

namespace Ie.TUDublin.GE2.Systems.Statemachine {

    public class SearchStateSystem : SystemBase {
        
        private EndSimulationEntityCommandBufferSystem _entityCommandBuffer;

        protected override void OnCreate() {
            _entityCommandBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {
            
            var ecb = _entityCommandBuffer.CreateCommandBuffer().AsParallelWriter();

            Entities
                .WithName("SearchingState")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, in TargetingData targetingData) => {

                    if (targetingData.Target == Entity.Null) {
                        StatemachineUtil.TransitionToSearching(ecb, entityInQueryIndex, entity);
                    }
                    else if (targetingData.Target != Entity.Null && HasComponent<SearchState>(entity)) {
                        StatemachineUtil.TransitionFromSearching(ecb, entityInQueryIndex, entity);
                    }
                    
                }).ScheduleParallel();
            
            _entityCommandBuffer.AddJobHandleForProducer(Dependency);
            
        }
        
    }

}