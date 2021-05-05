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
        
        
    }

}