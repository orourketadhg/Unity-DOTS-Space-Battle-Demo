using Unity.Entities;

namespace Ie.TUDublin.GE2.Components.Spaceship {

    [GenerateAuthoringComponent]
    public struct HealthData : IComponentData {
        public int Value;
    }

}