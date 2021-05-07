using ie.TUDublin.GE2.Components.Steering;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering {

    /// <summary>
    /// System to calculate and apply forces from steering behaviours
    /// </summary>
    [UpdateAfter(typeof(SteeringForcesSystem))]
    public class BoidSystem : SystemBase {

        private EntityQuery _boidQuery;

        protected override void OnCreate() {

            // Query to get components for Boid Job
            var boidQueryDesc = new EntityQueryDesc() {
                Any = new[] {
                    typeof(Translation),
                    typeof(Rotation),
                    typeof(BoidData),
                    ComponentType.ReadOnly<SteeringData>(),
                    ComponentType.ReadOnly<SeekData>(),
                    ComponentType.ReadOnly<ArriveData>(),
                    ComponentType.ReadOnly<PursueData>(),
                    ComponentType.ReadOnly<FleeData>(),
                    ComponentType.ReadOnly<WanderData>(),
                    ComponentType.ReadOnly<ConstrainData>(),

                }
            };

            _boidQuery = GetEntityQuery(boidQueryDesc);
        }

        protected override void OnUpdate() {

            float dt = Time.DeltaTime;
            
            // var seekHandle = GetComponentTypeHandle<SeekData>(true); 
            // var arriveHandle = GetComponentTypeHandle<ArriveData>(true);
            // var pursueHandle = GetComponentTypeHandle<PursueData>(true);
            // var fleeHandle = GetComponentTypeHandle<FleeData>(true);
            // var wanderHandle = GetComponentTypeHandle<WanderData>(true);
            // var constrainHandle = GetComponentTypeHandle<ConstrainData>(true);
            // var steeringHandle = GetComponentTypeHandle<SteeringData>(true);
            //
            // var translationHandle = GetComponentTypeHandle<Translation>();
            // var rotationHandle = GetComponentTypeHandle<Rotation>();
            // var boidHandle = GetComponentTypeHandle<BoidData>();
            //
            // var boidJob = new BoidJob() {
            //     DeltaTime = dt,
            //
            //     SeekHandle = seekHandle,
            //     ArriveHandle = arriveHandle,
            //     PursueHandle = pursueHandle,
            //     FleeHandle = fleeHandle,
            //     WanderHandle = wanderHandle,
            //     ConstrainHandle = constrainHandle,
            //     SteeringHandle = steeringHandle,
            //     TranslationHandle = translationHandle,
            //     RotationHandle = rotationHandle,
            //     BoidData = boidHandle
            // };
            //
            // Dependency = boidJob.ScheduleParallel(_boidQuery, 1, Dependency);

            Entities
                .WithName("BoidSystem")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref Translation translation, ref Rotation rotation, ref BoidData boidData, in SteeringData steering) => {
            
                    var boid = boidData;
                    
                    // accumulate forces
                    
                    var force = float3.zero;
                    
                    // calculate seek forces
                    if (steering.Seek == 1) {
                        var seek = GetComponent<SeekData>(entity);
                        force += seek.Force * seek.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
            
                    // calculate arrive forces
                    if (steering.Arrive == 1) {
                        var arrive = GetComponent<ArriveData>(entity);
                        force += arrive.Force * arrive.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
            
                    // calculate pursue forces
                    if (steering.Pursue == 1) {
                        var pursue = GetComponent<PursueData>(entity);
                        force += pursue.Force * pursue.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
            
                    // calculate flee forces
                    if (steering.Flee == 1) {
                        var flee = GetComponent<FleeData>(entity);
                        force += flee.Force * flee.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
            
                    // calculate constrain forces
                    if (steering.Constrain == 1) {
                        var constrain = GetComponent<ConstrainData>(entity);
                        force += constrain.Force * constrain.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
            
                    // calculate wander forces
                    if (steering.Wander == 1) {
                        var wander = GetComponent<WanderData>(entity);
                        force += wander.Force * wander.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
            
                    // apply boid weight
                    force *= boid.Weight;
                    boid.Force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    
                    // calculate velocity based on forces 
                    var tempAcceleration = (boid.Force * boid.Weight) * (1.0f / boid.Mass);
                    tempAcceleration.y *= boidData.VerticalLimiter;
                    boid.Acceleration = math.lerp(boid.Acceleration, tempAcceleration, dt);
                    
                    // limit vertical spinning
                    boid.Velocity += boid.Acceleration * dt;
                    boid.Velocity = MathUtil.ClampMagnitude(boid.Velocity, boid.MaxSpeed);
                    
                    // apply velocity to boid
                    boid.Speed = math.length(boid.Velocity);
                    if (boid.Speed > 0) {
                    
                        var tempUp = math.lerp(boid.Up, math.up() + (boid.Acceleration * boid.Banking), dt * 3.0f);
                        rotation.Value = quaternion.LookRotation(boid.Velocity, tempUp);
                        boid.Up = math.mul(rotation.Value, math.up());
            
                        translation.Value += boid.Velocity * dt;
                        boid.Velocity *= ( 1.0f - ( boid.Damping * dt ) );
                    }
            
                    // update boid data
                    boidData = boid;
            
                }).ScheduleParallel();
            
        }

    }

}