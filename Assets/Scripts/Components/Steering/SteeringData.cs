using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Steering {
    
    /// <summary>
    /// Controller data for the enabled steering behaviours a ship has
    /// </summary>
    public struct SteeringData : IComponentData {
        public int Seek;
        public int Arrive;
        public int Pursue;
        public int Flee;
        public int Wander;
        public int Constrain;
    }

}