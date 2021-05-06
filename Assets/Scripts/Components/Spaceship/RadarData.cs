using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Spaceship {

    public struct RadarData : IComponentData {
        public float Distance;
        public float Radius;
    }

}