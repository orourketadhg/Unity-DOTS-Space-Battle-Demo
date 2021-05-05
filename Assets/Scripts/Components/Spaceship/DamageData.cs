using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Spaceship {

    [GenerateAuthoringComponent]
    public struct DamageData : IComponentData {
        public int Value;
    }

}