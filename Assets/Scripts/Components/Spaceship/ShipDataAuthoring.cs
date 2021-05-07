using ie.TUDublin.GE2.Components.Steering;
using Unity.Entities;
using UnityEngine;

namespace ie.TUDublin.GE2.Components.Spaceship {

    /// <summary>
    /// Authoring class for components on a ship
    /// </summary>
    public class ShipDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity {

        [Header("Ship Properties")]
        [SerializeField] private int shipHealth;
        [SerializeField] private int collisionDamage;

        [Header("Ship Targeting")]
        [SerializeField] private float attackRange;

        [Header("Ship Radar")] 
        [SerializeField] private float distance;
        [SerializeField] private float radius;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

            dstManager.AddComponentData(entity, new HealthData() {Value = shipHealth});
            dstManager.AddComponentData(entity, new DamageData() {Value = collisionDamage});
            dstManager.AddComponentData(entity, new TargetingData() {AttackDistance = attackRange});
            dstManager.AddComponentData(entity, new RadarData() {Distance = distance, Radius = radius});
            dstManager.AddComponentData(entity, new SteeringData());
            dstManager.AddBuffer<PursuerElementData>(entity);
        }
    }

}