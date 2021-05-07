using ie.TUDublin.GE2.Components.Spaceship;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace ie.TUDublin.GE2.Systems.Physics {

    /// <summary>
    /// Trigger Job to inflict damage on an entities health
    /// </summary>
    [BurstCompile]
    public struct DamageTriggerJob : ITriggerEventsJob {

        public EntityCommandBuffer ecb;
        
        public ComponentDataFromEntity<HealthData> healthData;
        public ComponentDataFromEntity<DamageData> damageData;

        public void Execute(TriggerEvent triggerEvent) {
            var entityA = triggerEvent.EntityA;
            var entityB = triggerEvent.EntityB;

            if (healthData.HasComponent(entityA) && damageData.HasComponent(entityB)) {
                int health = healthData[entityA].Value;
                int damage = damageData[entityB].Value;
                int newHealth = math.clamp(health - damage, 0, health);
                
                ecb.SetComponent(entityA, new HealthData() { Value = newHealth});
            }
            
            if (healthData.HasComponent(entityB) && damageData.HasComponent(entityA)) {
                int health = healthData[entityB].Value;
                int damage = damageData[entityA].Value;
                int newHealth = math.clamp(health - damage, 0, health);
                
                ecb.SetComponent(entityB, new HealthData() { Value = newHealth});
            }
        }
    }

}