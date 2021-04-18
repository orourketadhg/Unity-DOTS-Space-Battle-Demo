using Ie.TUDublin.GE2.Components.Spaceship;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace Ie.TUDublin.GE2.Systems.Physics {

    [BurstCompile]
    public struct DamageTriggerJob : ITriggerEventsJob {

        public ComponentDataFromEntity<DamageData> damageData;
        public ComponentDataFromEntity<HealthData> healthData;
        
        public void Execute(TriggerEvent triggerEvent) {
            var entityA = triggerEvent.EntityA;
            var entityB = triggerEvent.EntityB;
            
            Debug.Log(entityA + " -> " + entityB);
        }
    }

}