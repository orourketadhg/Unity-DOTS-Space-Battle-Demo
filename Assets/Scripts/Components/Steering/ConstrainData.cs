using Unity.Entities;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Components.Steering {

    /// <summary>
    /// Constrain steering behaviour details 
    /// </summary>
    [GenerateAuthoringComponent]
    public struct ConstrainData : IComponentData {
        public float3 Force;
        public float Weight;

        public float Radius;
        public float3 Origin;
    }

}