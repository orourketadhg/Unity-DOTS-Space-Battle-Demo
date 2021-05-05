using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Spaceship {

    [GenerateAuthoringComponent]
    public struct HealthData : IComponentData {
        public int Value;
    }

}