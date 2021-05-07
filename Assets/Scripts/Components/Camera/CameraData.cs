using Unity.Entities;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Components.Camera {

    /// <summary>
    /// Camera positioning details about a ship
    /// </summary>
    [GenerateAuthoringComponent]
    public struct CameraData : IComponentData {
        public float3 Offset;
    }

}