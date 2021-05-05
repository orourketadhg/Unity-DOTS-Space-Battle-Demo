using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Statemachine;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Statemachine {

    public class AttackStateSystem : SystemBase {

        private EndSimulationEntityCommandBufferSystem _entityCommandBuffer;

        protected override void OnCreate() {
            _entityCommandBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {

            var ecb = _entityCommandBuffer.CreateCommandBuffer().AsParallelWriter();

            Entities
                .WithName("AttackingState")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, in Translation position, in TargetingData targetingData) => {

                    if (targetingData.Target == Entity.Null) {
                        if (HasComponent<AttackingState>(entity)) {
                            StatemachineUtil.TransitionFromAttacking(ecb, entityInQueryIndex, entity);
                        }
                        return;
                    }
                    
                    var targetPosition = GetComponent<Translation>(targetingData.Target).Value;
                    float distanceToTarget = math.distance(position.Value, targetPosition);

                    if (distanceToTarget > targetingData.AttackDistance) {
                        StatemachineUtil.TransitionFromAttacking(ecb, entityInQueryIndex, entity);
                    }
                    if (distanceToTarget <= targetingData.AttackDistance && !HasComponent<AttackingState>(entity)) {
                        StatemachineUtil.TransitionToAttacking(ecb, entityInQueryIndex, entity);
                    }
                    
                }).ScheduleParallel();
            
            _entityCommandBuffer.AddJobHandleForProducer(Dependency);

        }
    }

}