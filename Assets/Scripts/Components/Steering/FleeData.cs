using Unity.Entities;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Components.Steering {

    /// <summary>
    /// Flee steering behaviour details 
    /// </summary>
    [GenerateAuthoringComponent]
    public struct FleeData : IComponentData {
        public float3 Force;
        public float Weight;
    }

}