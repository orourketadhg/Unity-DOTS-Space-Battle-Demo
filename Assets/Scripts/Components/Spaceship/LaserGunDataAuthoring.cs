using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Ie.TUDublin.GE2.Components.Spaceship {

    public class LaserGunDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs {

        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private float laserSpeed;
        [SerializeField] private float projectileLifetime;
        [SerializeField] [Min(0)] private float firingRate;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
            dstManager.AddComponentData(entity, new LaserGunSettingsData() {
                LaserPrefab = conversionSystem.GetPrimaryEntity(laserPrefab),
                FiringRate = firingRate,
                ProjectileSpeed = laserSpeed,
                ProjectileLifetime = projectileLifetime
            });

            dstManager.AddComponentData(entity, new LaserGunInternalSettingsData() {
                IsFiringEnabled = 0,
                TimeOfLastFire = 0
            });
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) => referencedPrefabs.Add(laserPrefab);
    }

    public struct LaserGunSettingsData : IComponentData {
        public float FiringRate;
        public Entity LaserPrefab;
        public float ProjectileSpeed;
        public float ProjectileLifetime;
    }

    public struct LaserGunInternalSettingsData : IComponentData {
        public int IsFiringEnabled;
        public float TimeOfLastFire;
    }

}