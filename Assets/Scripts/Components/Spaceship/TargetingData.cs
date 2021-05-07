using Unity.Entities;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Components.Spaceship {
    
    /// <summary>
    /// Settings about a ships targeting system
    /// </summary>
    public struct TargetingData : IComponentData {
        public Entity Target;
        public float3 TargetPosition;
        public float AttackDistance;
        
    }

}