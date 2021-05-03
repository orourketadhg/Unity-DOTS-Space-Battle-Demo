using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Ie.TUDublin.GE2.Components.Spawning {

    public class ShipSpawningSettingsAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs {

        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 spawnArea;
        [SerializeField] private int spawnAttempts; 
        
        [SerializeField] private List<ShipSettings> ships;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

            dstManager.AddComponentData(entity, new ShipSpawningSettingsData() {
                position = position,
                spawnArea = spawnArea,
                spawnAttempts = spawnAttempts
            });
            
            var shipBuffer = dstManager.AddBuffer<ShipSpawningPrefabElement>(entity);
            foreach (var ship in ships) {
                shipBuffer.Add(new ShipSpawningPrefabElement() {
                    prefab = conversionSystem.GetPrimaryEntity(ship.prefab),
                    numberOfShips = ship.numberOfShips,
                    spawnCheckRadius = ship.spawnCheckRadius
                });
            }

        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) => referencedPrefabs.AddRange(ships.Select(ship => ship.prefab));
    }

    [System.Serializable]
    public class ShipSettings {
        public string name;
        public GameObject prefab;
        [Min(0)] public Vector2 numberOfShips;
        [Min(0)] public float spawnCheckRadius;
    }

    public struct ShipSpawningSettingsData : IComponentData {
        public float3 position;
        public float3 spawnArea;
        public int spawnAttempts;
    }

    public struct ShipSpawningPrefabElement : IBufferElementData {
        public Entity prefab;
        public float2 numberOfShips;
        public float spawnCheckRadius;
    }

}