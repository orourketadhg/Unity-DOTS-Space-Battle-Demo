using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Spaceship {

    /// <summary>
    /// Health an entity can have
    /// </summary>
    [GenerateAuthoringComponent]
    public struct HealthData : IComponentData {
        public int Value;
    }

}