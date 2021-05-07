using Unity.Entities;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Components.Camera {

    [GenerateAuthoringComponent]
    public struct CameraData : IComponentData {
        public float3 Offset;
    }

}