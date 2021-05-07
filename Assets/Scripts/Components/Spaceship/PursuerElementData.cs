using Unity.Entities;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Components.Spaceship {

    /// <summary>
    /// Buffer element to store details about a pursing ship
    /// </summary>
    public struct PursuerElementData : IBufferElementData {
        public Entity PursuerEntity;
        public float3 PursuerPosition;
    }

}