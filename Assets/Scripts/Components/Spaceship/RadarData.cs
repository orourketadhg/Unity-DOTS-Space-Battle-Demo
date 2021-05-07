using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Spaceship {

    /// <summary>
    /// Settings for a ships radar
    /// </summary>
    public struct RadarData : IComponentData {
        public float Distance;
        public float Radius;
    }

}