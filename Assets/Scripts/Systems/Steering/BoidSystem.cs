using Ie.TUDublin.GE2.Components.Steering;
using Ie.TUDublin.GE2.Util;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Steering {

    [UpdateAfter(typeof(SteeringForcesSystem))]
    public class BoidSystem : SystemBase {
        
        protected override void OnUpdate() {

            float dt = Time.DeltaTime;

            Entities
                .WithName("SteeringBehaviourForceAccumulationJob")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref BoidData spaceshipBoidData, ref Translation translation, ref Rotation rotation) => {
                    
                    var boidData = spaceshipBoidData;
                    
                    var force = float3.zero;
                    
                    if (HasComponent<SeekData>(entity)) {
                        var wanderData = GetComponent<SeekData>(entity);
                        force += wanderData.Force * wanderData.Weight;
                        force = MathUtil.ClampMagnitude(force, boidData.MaxForce);
                    }
                    
                    if (HasComponent<ArriveData>(entity)) {
                        var wanderData = GetComponent<ArriveData>(entity);
                        force += wanderData.Force * wanderData.Weight;
                        force = MathUtil.ClampMagnitude(force, boidData.MaxForce);
                    }
                    
                    if (HasComponent<PursueData>(entity)) {
                        var wanderData = GetComponent<PursueData>(entity);
                        force += wanderData.Force * wanderData.Weight;
                        force = MathUtil.ClampMagnitude(force, boidData.MaxForce);
                    }
                    
                    if (HasComponent<FleeData>(entity)) {
                        var wanderData = GetComponent<FleeData>(entity);
                        force += wanderData.Force * wanderData.Weight;
                        force = MathUtil.ClampMagnitude(force, boidData.MaxForce);
                    }

                    if (HasComponent<WanderData>(entity)) {
                        var wanderData = GetComponent<WanderData>(entity);
                        force += wanderData.Force * wanderData.Weight;
                        force = MathUtil.ClampMagnitude(force, boidData.MaxForce);
                    }
                    
                    if (HasComponent<ConstrainData>(entity)) {
                        var wanderData = GetComponent<ConstrainData>(entity);
                        force += wanderData.Force * wanderData.Weight;
                        force = MathUtil.ClampMagnitude(force, boidData.MaxForce);
                    }

                    boidData.Force = MathUtil.ClampMagnitude(force, boidData.MaxForce);
                    boidData.Acceleration = boidData.Force / boidData.Mass;
                    boidData.Velocity += boidData.Acceleration * dt;

                    boidData.Velocity = MathUtil.ClampMagnitude(boidData.Velocity, boidData.MaxSpeed);

                    boidData.Speed = math.length(boidData.Velocity);
                    if (boidData.Speed > 0) {
                        
                        var tempUp = math.lerp(boidData.Up, math.up() + (boidData.Acceleration * boidData.Banking), dt * 3.0f);
                        rotation.Value = quaternion.LookRotation(boidData.Velocity, tempUp);
                        boidData.Up = tempUp * math.up();

                        translation.Value += boidData.Velocity * dt;
                        boidData.Velocity *= ( 1.0f - ( boidData.Damping * dt ) );

                    }
                    
                    spaceshipBoidData = boidData;

                }).ScheduleParallel();

        }

    }

}