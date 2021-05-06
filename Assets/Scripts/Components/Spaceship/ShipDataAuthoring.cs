using ie.TUDublin.GE2.Components.Steering;
using Unity.Entities;
using UnityEngine;

namespace ie.TUDublin.GE2.Components.Spaceship {

    public class ShipDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity {

        [Header("Ship Properties")]
        [SerializeField] private int shipHealth;
        [SerializeField] private int collisionDamage;

        [Header("Ship Targeting")]
        [SerializeField] private float attackRange;
        [SerializeField] private GameObject target;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

            dstManager.AddComponentData(entity, new HealthData() {Value = shipHealth});
            dstManager.AddComponentData(entity, new DamageData() {Value = collisionDamage});
            dstManager.AddComponentData(entity, new TargetingData() {Target = conversionSystem.GetPrimaryEntity(target), AttackDistance = attackRange});
            dstManager.AddComponentData(entity, new SteeringData());
            dstManager.AddBuffer<PursuerElementData>(entity);
        }
    }

}