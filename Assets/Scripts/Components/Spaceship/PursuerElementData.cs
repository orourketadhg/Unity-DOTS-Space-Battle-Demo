using Unity.Entities;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Components.Spaceship {

    public struct PursuerElementData : IBufferElementData {
        public Entity PursuerEntity;
        public float3 PursuerPosition;
    }

}