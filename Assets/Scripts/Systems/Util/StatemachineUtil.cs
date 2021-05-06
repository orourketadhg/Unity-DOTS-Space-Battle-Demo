using ie.TUDublin.GE2.Components.Statemachine;
using Unity.Burst;
using Unity.Entities;

namespace ie.TUDublin.GE2.Systems.Util {

    [BurstCompile]
    public static class StatemachineUtil {

        public static void TransitionToAttacking(EntityCommandBuffer.ParallelWriter ecb, int index, Entity entity) {
            ecb.AddComponent<AttackingState>(index, entity);
        }
        
        public static void TransitionFromAttacking(EntityCommandBuffer.ParallelWriter ecb, int index, Entity entity) {
            ecb.RemoveComponent<AttackingState>(index, entity);
        }
        
        public static void TransitionToSearching(EntityCommandBuffer.ParallelWriter ecb, int index, Entity entity) {
            ecb.AddComponent<SearchState>(index, entity);
        }
        
        public static void TransitionFromSearching(EntityCommandBuffer.ParallelWriter ecb, int index, Entity entity) {
            ecb.RemoveComponent<SearchState>(index, entity);
        }
        
        public static void TransitionToFleeing(EntityCommandBuffer.ParallelWriter ecb, int index, Entity entity) {
            ecb.AddComponent<FleeState>(index, entity);
        }
        
        public static void TransitionFromFleeing(EntityCommandBuffer.ParallelWriter ecb, int index, Entity entity) {
            ecb.RemoveComponent<FleeState>(index, entity);
        }
        
        public static void TransitionToPursuing(EntityCommandBuffer.ParallelWriter ecb, int index, Entity entity) {
            ecb.AddComponent<PursueState>(index, entity);
        }
        
        public static void TransitionFromPursuing(EntityCommandBuffer.ParallelWriter ecb, int index, Entity entity) {
            ecb.RemoveComponent<PursueState>(index, entity);
        }
        
        
    }

}