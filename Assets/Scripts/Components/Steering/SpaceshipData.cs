using Unity.Entities;

namespace Ie.TUDublin.GE2.Components.Steering {

    [GenerateAuthoringComponent]
    public struct SpaceshipData : IComponentData {
        public Entity Target;
        public float MaxSpeed; 
    }

}