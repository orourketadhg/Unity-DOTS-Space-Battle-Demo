using Unity.Entities;

namespace Ie.TUDublin.GE2.Components.Spaceship {

    [GenerateAuthoringComponent]
    public struct DamageData : IComponentData {
        public int Value;
    }

}