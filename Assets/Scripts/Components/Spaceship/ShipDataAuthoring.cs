﻿using Unity.Entities;
using UnityEngine;

namespace Ie.TUDublin.GE2.Components.Spaceship {

    public class ShipDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity {

        [Header("Ship Properties")]
        [SerializeField] private int shipHealth;
        [SerializeField] private int collisionDamage;

        [Header("Ship Targeting")] 
        [SerializeField] private float attackRange;
        

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

            dstManager.AddComponentData(entity, new HealthData() {Value = shipHealth});
            dstManager.AddComponentData(entity, new DamageData() {Value = collisionDamage});
            dstManager.AddComponentData(entity, new TargetingData() {AttackDistance = attackRange});
        }
    }

}