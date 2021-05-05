using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Steering {

    [GenerateAuthoringComponent]
    public struct ActiveSteeringData : IComponentData {
        public int Seek;
        public int Arrive;
        public int Pursue;
        public int Flee;
        public int Wander;
        public int Constrain;
    }

}