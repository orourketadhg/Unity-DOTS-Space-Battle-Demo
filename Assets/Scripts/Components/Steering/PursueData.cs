using Unity.Entities;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Components.Steering {

    /// <summary>
    /// Pursue steering behaviour details 
    /// </summary>
    [GenerateAuthoringComponent]
    public struct PursueData : IComponentData {
        public float3 Force;
        public float Weight;

        public float3 TargetVelocity;
    }

}