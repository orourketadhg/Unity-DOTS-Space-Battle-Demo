using Unity.Entities;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Components.Spaceship {
    
    public struct TargetingData : IComponentData {
        public Entity Target;
        public float3 TargetPosition;
        public float AttackDistance;
        
    }

}