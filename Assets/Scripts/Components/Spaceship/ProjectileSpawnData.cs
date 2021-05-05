using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Spaceship {

    [GenerateAuthoringComponent]
    public struct ProjectileSpawnData : IComponentData {
        public int DoDespawn;
        public float SpawnTime;
        public float ProjectileLifetime;
    }

}