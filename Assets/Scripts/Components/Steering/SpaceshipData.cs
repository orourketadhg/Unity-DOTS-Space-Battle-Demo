﻿using Unity.Entities;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Components.Steering {

    [GenerateAuthoringComponent]
    public struct SpaceshipData : IComponentData {
        public Entity Target;
        public float3 TargetPosition;
        
        public float MaxSpeed; 
    }

}