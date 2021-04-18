using Unity.Entities;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Components.Spaceship {

    [GenerateAuthoringComponent]
    public struct HealthData : IComponentData {
        public int Value;
    }

}