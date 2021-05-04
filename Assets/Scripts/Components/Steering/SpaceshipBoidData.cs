using Unity.Entities;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Components.Steering {

    [GenerateAuthoringComponent]
    public struct SpaceshipBoidData : IComponentData {
        public Entity Target;

        public float Speed;
        public float MaxSpeed;

        public float attackDistance;
    }

}