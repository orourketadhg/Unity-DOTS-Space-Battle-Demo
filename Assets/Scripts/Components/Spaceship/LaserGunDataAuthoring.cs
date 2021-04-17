using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Ie.TUDublin.GE2.Components.Spaceship {

    public class LaserGunDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs {

        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private float3 laserRotation;
        [SerializeField] private float laserSpeed;
        [SerializeField] [Min(0)] private float firingRate;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
            dstManager.AddComponentData(entity, new LaserGunSettingsData() {
                LaserPrefab = conversionSystem.GetPrimaryEntity(laserPrefab),
                FiringRate = firingRate,
                LaserRotation = laserRotation,
                ProjectileSpeed = laserSpeed,
                
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
        public float3 LaserRotation;
        public float ProjectileSpeed;
    }

    public struct LaserGunInternalSettingsData : IComponentData {
        public int IsFiringEnabled;
        public float TimeOfLastFire;
    }

}