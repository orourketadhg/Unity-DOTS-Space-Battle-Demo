using Ie.TUDublin.GE2.Components.Spaceship;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace Ie.TUDublin.GE2.Systems.Physics {

    [BurstCompile]
    public struct CollisionDamageJob : ICollisionEventsJob {

        public ComponentDataFromEntity<DamageData> damageData;
        public ComponentDataFromEntity<HealthData> healthData;
        
        public void Execute(CollisionEvent collisionEvent) {

            var entityA = collisionEvent.EntityA;
            var entityB = collisionEvent.EntityB;

            if (damageData.HasComponent(entityA) && healthData.HasComponent(entityB)) {
                healthData[entityA].TakeDamage(damageData[entityB].Value);
            }
            
            if (damageData.HasComponent(entityA) && healthData.HasComponent(entityA)) {
                healthData[entityB].TakeDamage(damageData[entityA].Value);
            }
        }
    }

}