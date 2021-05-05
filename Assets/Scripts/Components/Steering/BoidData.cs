using Unity.Entities;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Components.Steering {

    [GenerateAuthoringComponent]
    public struct BoidData : IComponentData {
        public float3 Velocity;
        public float3 Acceleration;
        public float3 Up;
        
        public float3 Force;
        public float MaxForce;

        public float Speed;
        public float MaxSpeed;
        
        public float Mass;
        public float Banking;
        public float Damping;
    }

}