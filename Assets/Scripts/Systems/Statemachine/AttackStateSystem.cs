using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Statemachine;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Statemachine {

    /// <summary>
    /// System to update a ships attacking state
    /// </summary>
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

                    // check if the target exists 
                    if (!EntityManager.Exists(targetingData.Target) || targetingData.Target == Entity.Null) {
                        // stop attacking
                        if (HasComponent<AttackingState>(entity)) {
                            StatemachineUtil.TransitionFromAttacking(ecb, entityInQueryIndex, entity);
                            
                            // disable guns
                            for (int i = 0; i < children.Length; i++) {
                                ecb.RemoveComponent<AttackingState>(children[i].Value);
                            }
                            
                        }
                        return;
                    }
                    
                    // get positioning of target
                    var targetPosition = GetComponent<Translation>(targetingData.Target).Value;
                    float distanceToTarget = math.distance(position.Value, targetPosition);

                    // update attacking state based on distance to target
                    if (distanceToTarget > targetingData.AttackDistance) {
                        StatemachineUtil.TransitionFromAttacking(ecb, entityInQueryIndex, entity);

                        // disable guns
                        for (int i = 0; i < children.Length; i++) {
                            ecb.RemoveComponent<AttackingState>(children[i].Value);
                        }
                    }
                    else if (distanceToTarget <= targetingData.AttackDistance && !HasComponent<AttackingState>(entity)) {
                        StatemachineUtil.TransitionToAttacking(ecb, entityInQueryIndex, entity);
                        
                        // enable guns
                        for (int i = 0; i < children.Length; i++) {
                            ecb.AddComponent<AttackingState>(children[i].Value);
                        }
                    }
                    
                }).Run();
            
            _entityCommandBuffer.AddJobHandleForProducer(Dependency);

        }
    }

}