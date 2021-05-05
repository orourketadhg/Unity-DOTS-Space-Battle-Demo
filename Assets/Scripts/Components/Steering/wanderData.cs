using Unity.Entities;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Components.Steering {

    [GenerateAuthoringComponent]
    public struct wanderData : IComponentData {
        public float3 Force;
        public float Weight;

        public float Distance;
        public float Radius;
        public float Jitter;

        public float3 Target;
        public float3 LocalTarget;
        public float3 WorldTarget;

    }

}