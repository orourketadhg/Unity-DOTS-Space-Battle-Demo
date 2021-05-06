using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;

namespace ie.TUDublin.GE2.Systems.Statemachine {

    public class PursueStateSystem : SystemBase {

        private EndSimulationEntityCommandBufferSystem _entityCommandBuffer;

        protected override void OnCreate() {
            _entityCommandBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {

            var ecb = _entityCommandBuffer.CreateCommandBuffer().AsParallelWriter();

            Entities
                .WithName("PursuingSystem")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, in TargetingData targetingData) => {

                    if (targetingData.Target == Entity.Null) {
                        StatemachineUtil.TransitionFromPursuing(ecb, entityInQueryIndex, entity);
                    }
                    else {
                        StatemachineUtil.TransitionToPursuing(ecb, entityInQueryIndex, entity);
                    }
                    
                }).Schedule();
            
            _entityCommandBuffer.AddJobHandleForProducer(Dependency);

        }
    }

}