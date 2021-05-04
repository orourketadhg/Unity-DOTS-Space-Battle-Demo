using Ie.TUDublin.GE2.Components.Statemachine;
using Ie.TUDublin.GE2.Components.Steering;
using Ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Statemachine {

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
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, in SpaceshipBoidData boidData, in Translation position) => {

                    var targetPosition = GetComponent<Translation>(boidData.Target).Value;
                    float distanceToTarget = math.distance(position.Value, targetPosition);

                    if (boidData.Target == Entity.Null || distanceToTarget > boidData.AttackDistance) {
                        StatemachineUtil.TransitionFromAttacking(ecb, entityInQueryIndex, entity);
                    }
                    if (distanceToTarget <= boidData.AttackDistance && !HasComponent<AttackingState>(entity)) {
                        StatemachineUtil.TransitionToAttacking(ecb, entityInQueryIndex, entity);
                    }
                    
                }).ScheduleParallel();
            
            _entityCommandBuffer.AddJobHandleForProducer(Dependency);

        }
    }

}