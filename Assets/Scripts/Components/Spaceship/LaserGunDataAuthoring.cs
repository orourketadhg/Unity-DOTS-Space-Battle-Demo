using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace ie.TUDublin.GE2.Components.Spaceship {

    /// <summary>
    /// Authoring Class for Laser Gun
    /// </summary>
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
                TimeOfLastFire = 0
            });
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) => referencedPrefabs.Add(laserPrefab);
    }

    /// <summary>
    /// Static Settings for a laser gun 
    /// </summary>
    public struct LaserGunSettingsData : IComponentData {
        public float FiringRate;
        public Entity LaserPrefab;
        public float ProjectileSpeed;
        public float ProjectileLifetime;
    }

    /// <summary>
    /// Dynamic settings for a laser gun
    /// </summary>
    public struct LaserGunInternalSettingsData : IComponentData {
        public float TimeOfLastFire;
    }

}