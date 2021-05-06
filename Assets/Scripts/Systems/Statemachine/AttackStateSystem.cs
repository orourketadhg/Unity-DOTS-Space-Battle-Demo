using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Statemachine;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Statemachine {

    [UpdateBefore(typeof(SceneCleanupSystem))]
    public class AttackStateSystem : SystemBase {

        private EndSimulationEntityCommandBufferSystem _entityCommandBuffer;

        protected override void OnCreate() {
            _entityCommandBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {

            var ecb = _entityCommandBuffer.CreateCommandBuffer();

            Entities
                .WithName("AttackingState")
                .WithoutBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, in Translation position, in TargetingData targetingData, in DynamicBuffer<Child> children) => {

                    if (!EntityManager.Exists(targetingData.Target) || targetingData.Target == Entity.Null) {
                        if (HasComponent<AttackingState>(entity)) {
                            StatemachineUtil.TransitionFromAttacking(ecb, entityInQueryIndex, entity);
                            
                            for (int i = 0; i < children.Length; i++) {
                                ecb.RemoveComponent<AttackingState>(children[i].Value);
                            }
                            
                        }
                        return;
                    }
                    
                    var targetPosition = GetComponent<Translation>(targetingData.Target).Value;
                    float distanceToTarget = math.distance(position.Value, targetPosition);

                    if (distanceToTarget > targetingData.AttackDistance) {
                        StatemachineUtil.TransitionFromAttacking(ecb, entityInQueryIndex, entity);

                        for (int i = 0; i < children.Length; i++) {
                            ecb.RemoveComponent<AttackingState>(children[i].Value);
                        }
                        
                    }
                    if (distanceToTarget <= targetingData.AttackDistance && !HasComponent<AttackingState>(entity)) {
                        StatemachineUtil.TransitionToAttacking(ecb, entityInQueryIndex, entity);
                        
                        for (int i = 0; i < children.Length; i++) {
                            ecb.AddComponent<AttackingState>(children[i].Value);
                        }
                    }
                    
                }).Run();
            
            _entityCommandBuffer.AddJobHandleForProducer(Dependency);

        }
    }

}