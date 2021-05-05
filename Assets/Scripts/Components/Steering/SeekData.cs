using Unity.Entities;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Components.Steering {

    [GenerateAuthoringComponent]
    public struct SeekData : IComponentData {
        public float3 Force;
        public float Weight;
    }

}