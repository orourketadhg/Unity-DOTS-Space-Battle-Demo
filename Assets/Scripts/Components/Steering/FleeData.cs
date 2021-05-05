using Unity.Entities;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Components.Steering {

    [GenerateAuthoringComponent]
    public struct FleeData : IComponentData {
        public float3 Force;
        public float Weight;
    }

}