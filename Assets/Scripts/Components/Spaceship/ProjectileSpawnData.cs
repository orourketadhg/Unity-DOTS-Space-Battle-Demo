using Unity.Entities;

namespace Ie.TUDublin.GE2.Components.Spaceship {

    [GenerateAuthoringComponent]
    public struct ProjectileSpawnData : IComponentData {
        public int DoDespawn;
        public float SpawnTime;
        public float ProjectileLifetime;
    }

}