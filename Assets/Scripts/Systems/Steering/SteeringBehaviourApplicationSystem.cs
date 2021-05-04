using Ie.TUDublin.GE2.Components.Steering;
using Ie.TUDublin.GE2.Util;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;

namespace Ie.TUDublin.GE2.Systems.Steering {

    [UpdateAfter(typeof(SteeringBehaviourCalculationSystem))]
    public class SteeringBehaviourApplicationSystem : SystemBase {

        protected override void OnUpdate() {

            Entities
                .WithName("SteeringBehaviourForceAccumulationJob")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref PhysicsVelocity velocity, in PhysicsMass mass, in SpaceshipBoidData boidData) => {

                    var force = float3.zero;

                    if (HasComponent<JitterWanderData>(entity)) {
                        var wanderData = GetComponent<JitterWanderData>(entity);
                        force += wanderData.Force * wanderData.Weight;
                        force = MathUtil.ClampMagnitude(force, boidData.MaxSpeed);
                    }

                    force = MathUtil.ClampMagnitude(force, boidData.MaxSpeed);

                    

                }).ScheduleParallel();

        }

    }

}