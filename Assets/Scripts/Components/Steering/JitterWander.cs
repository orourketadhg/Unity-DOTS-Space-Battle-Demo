﻿using Unity.Entities;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Components.Steering {

    public struct JitterWander : IComponentData {
        public float3 Force;
        public float Weight;

        public float Distance;
        public float Radius;
        public float Jitter;

        public float3 target;

    }

}