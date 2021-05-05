using Unity.Entities;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Components.Spaceship {

    [GenerateAuthoringComponent]
    public struct TargetingData : IComponentData {
        public Entity Target;
        public float3 TargetPosition;
        public float AttackDistance;
    }

}