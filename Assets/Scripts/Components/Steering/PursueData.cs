using Unity.Entities;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Components.Steering {

    public struct PursueData : IComponentData {
        public float Weight;
        public float3 Force;
        
        public float3 Target;
        
    }

}