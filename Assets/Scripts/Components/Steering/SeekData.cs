using Unity.Entities;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Components.Steering {

    /// <summary>
    /// Seek steering behaviour details 
    /// </summary>
    [GenerateAuthoringComponent]
    public struct SeekData : IComponentData {
        public float3 Force;
        public float Weight;
    }

}