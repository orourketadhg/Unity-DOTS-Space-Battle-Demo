using ie.TUDublin.GE2.Components.Steering;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering {

    [UpdateAfter(typeof(SteeringForcesSystem))]
    public class BoidSystem : SystemBase {

        private EntityQuery _boidQuery;

        protected override void OnCreate() {

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
                    
                    var force = float3.zero;
                    
                    if (steering.Seek == 1) {
                        var seek = GetComponent<SeekData>(entity);
                        force += seek.Force * seek.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
            
                    if (steering.Arrive == 1) {
                        var arrive = GetComponent<ArriveData>(entity);
                        force += arrive.Force * arrive.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
            
                    if (steering.Pursue == 1) {
                        var pursue = GetComponent<PursueData>(entity);
                        force += pursue.Force * pursue.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
            
                    if (steering.Flee == 1) {
                        var flee = GetComponent<FleeData>(entity);
                        force += flee.Force * flee.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
            
                    if (steering.Constrain == 1) {
                        var constrain = GetComponent<ConstrainData>(entity);
                        force += constrain.Force * constrain.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
            
                    if (steering.Wander == 1) {
                        var wander = GetComponent<WanderData>(entity);
                        force += wander.Force * wander.Weight;
                        force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    }
                    
                    boid.Force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                    boid.Acceleration = boid.Force / boid.Mass;
                    boid.Velocity += boid.Acceleration * dt;

                    boid.Velocity = MathUtil.ClampMagnitude(boid.Velocity, boid.MaxSpeed);

                    boid.Speed = math.length(boid.Velocity);
                    if (boid.Speed > 0) {
                    
                        var tempUp = math.lerp(boid.Up, math.up() + (boid.Acceleration * boid.Banking), dt * 3.0f);
                        rotation.Value = quaternion.LookRotation(boid.Velocity, tempUp);
                        boid.Up = tempUp * math.up();

                        translation.Value += boid.Velocity * dt;
                        boid.Velocity *= ( 1.0f - ( boid.Damping * dt ) );

                    }
                    
                }).ScheduleParallel();
            
        }

    }

}