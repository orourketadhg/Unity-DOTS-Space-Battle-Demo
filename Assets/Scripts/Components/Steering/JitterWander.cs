using Unity.Entities;

namespace Ie.TUDublin.GE2.Components.Steering {

    [GenerateAuthoringComponent]
    public struct JitterWander : IComponentData {
        public float Distance;
        public float Radius;
        public float Jitter; 

    }

}