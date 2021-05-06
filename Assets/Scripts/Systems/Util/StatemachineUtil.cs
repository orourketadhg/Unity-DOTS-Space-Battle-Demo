using ie.TUDublin.GE2.Components.Statemachine;
using Unity.Burst;
using Unity.Entities;

namespace ie.TUDublin.GE2.Systems.Util {

    [BurstCompile]
    public static class StatemachineUtil {

        public static void TransitionToAttacking(EntityCommandBuffer ecb, int index, Entity entity) {
            ecb.AddComponent<AttackingState>(entity);
        }
        
        public static void TransitionFromAttacking(EntityCommandBuffer ecb, int index, Entity entity) {
            ecb.RemoveComponent<AttackingState>(entity);
        }
        
        public static void TransitionToSearching(EntityCommandBuffer ecb, int index, Entity entity) {
            ecb.AddComponent<SearchState>(entity);
        }
        
        public static void TransitionFromSearching(EntityCommandBuffer ecb, int index, Entity entity) {
            ecb.RemoveComponent<SearchState>(entity);
        }
        
        public static void TransitionToFleeing(EntityCommandBuffer ecb, int index, Entity entity) {
            ecb.AddComponent<FleeState>(entity);
        }
        
        public static void TransitionFromFleeing(EntityCommandBuffer ecb, int index, Entity entity) {
            ecb.RemoveComponent<FleeState>(entity);
        }
        
        public static void TransitionToPursuing(EntityCommandBuffer ecb, int index, Entity entity) {
            ecb.AddComponent<PursueState>(entity);
        }
        
        public static void TransitionFromPursuing(EntityCommandBuffer ecb, int index, Entity entity) {
            ecb.RemoveComponent<PursueState>(entity);
        }
        
        
    }

}