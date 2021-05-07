using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Spaceship {

    /// <summary>
    /// Details about a projectile entity
    /// </summary>
    [GenerateAuthoringComponent]
    public struct ProjectileSpawnData : IComponentData {
        public int DoDespawn;
        public float SpawnTime;
        public float ProjectileLifetime;
    }

}