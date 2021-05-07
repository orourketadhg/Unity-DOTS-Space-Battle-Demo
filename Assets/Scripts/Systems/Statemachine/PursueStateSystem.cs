using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Statemachine {
    
    /// <summary>
    /// System to update a ships pursuing state
    /// </summary>
    public class PursueStateSystem : SystemBase {

        private EndSimulationEntityCommandBufferSystem _entityCommandBuffer;

        protected override void OnCreate() {
            _entityCommandBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {

            var ecb = _entityCommandBuffer.CreateCommandBuffer();

            Entities
                .WithName("PursuingSystem")
                .WithoutBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, in TargetingData targetingData) => {

                    // depending on if a target exists, update the pursuing state 
                    if (!EntityManager.Exists(targetingData.Target) || targetingData.Target == Entity.Null) {
                        StatemachineUtil.TransitionFromPursuing(ecb, entityInQueryIndex, entity);
                    }
                    else {
                        StatemachineUtil.TransitionToPursuing(ecb, entityInQueryIndex, entity);
                    }
                    
                }).Run();
            
            _entityCommandBuffer.AddJobHandleForProducer(Dependency);

        }
    }

}