using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Spaceship {
    
    /// <summary>
    /// Damage an entity can cause 
    /// </summary>
    [GenerateAuthoringComponent]
    public struct DamageData : IComponentData {
        public int Value;
    }

}