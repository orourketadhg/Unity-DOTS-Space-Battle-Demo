using Ie.TUDublin.GE2.Components.Statemachine;
using Unity.Entities;

namespace Ie.TUDublin.GE2.Systems.Util {

    
    public class StatemachineUtil {

        public static void TransitionToAttacking(EntityCommandBuffer.ParallelWriter ecb, int index, Entity entity) {
            ecb.AddComponent<AttackingState>(index, entity);
        }
        
        public static void TransitionFromAttacking(EntityCommandBuffer.ParallelWriter ecb, int index, Entity entity) {
            ecb.RemoveComponent<AttackingState>(index, entity);
        }
        
    }

}