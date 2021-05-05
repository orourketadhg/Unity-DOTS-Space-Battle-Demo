﻿using Unity.Entities;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Components.Steering {
    
    [GenerateAuthoringComponent]
    public struct ArriveData : IComponentData {
        public float3 Force;
        public float Weight;
        public float SlowingDistance;
    }

}