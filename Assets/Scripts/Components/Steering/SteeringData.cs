using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Steering {
    
    public struct SteeringData : IComponentData {
        public int Seek;
        public int Arrive;
        public int Pursue;
        public int Flee;
        public int Wander;
        public int Constrain;
    }

}