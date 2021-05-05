using ie.TUDublin.GE2.Components.Steering;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering {

    [BurstCompile]
    public struct BoidJob : IJobEntityBatch {

        public float DeltaTime;

        [ReadOnly] public ComponentTypeHandle<SeekData> SeekHandle; 
        [ReadOnly] public ComponentTypeHandle<ArriveData> ArriveHandle; 
        [ReadOnly] public ComponentTypeHandle<PursueData> PursueHandle; 
        [ReadOnly] public ComponentTypeHandle<FleeData> FleeHandle;
        [ReadOnly] public ComponentTypeHandle<ConstrainData> ConstrainHandle;
        [ReadOnly] public ComponentTypeHandle<WanderData> WanderHandle;
        [ReadOnly] public ComponentTypeHandle<ActiveSteeringData> SteeringHandle;

        public ComponentTypeHandle<Translation> TranslationHandle;
        public ComponentTypeHandle<Rotation> RotationHandle;
        public ComponentTypeHandle<BoidData> BoidData;
        
        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {

            var seekData = batchInChunk.GetNativeArray(SeekHandle);
            var arriveData = batchInChunk.GetNativeArray(ArriveHandle);
            var pursueData = batchInChunk.GetNativeArray(PursueHandle);
            var fleeData = batchInChunk.GetNativeArray(FleeHandle);
            var constrainData = batchInChunk.GetNativeArray(ConstrainHandle);
            var wanderData = batchInChunk.GetNativeArray(WanderHandle);
            var steeringData = batchInChunk.GetNativeArray(SteeringHandle);
            var translationData = batchInChunk.GetNativeArray(TranslationHandle);
            var rotationData = batchInChunk.GetNativeArray(RotationHandle);
            var boidData = batchInChunk.GetNativeArray(BoidData);

            for (int i = 0; i < batchInChunk.Count; i++) {
                var seek = seekData[i];
                var arrive = arriveData[i];
                var pursue = pursueData[i];
                var flee = fleeData[i];
                var constrain = constrainData[i];
                var wander = wanderData[i];
                var steering = steeringData[i];
                var translation = translationData[i];
                var rotation = rotationData[i];
                var boid = boidData[i];
                
                var force = CalculateForces(in seek, in arrive, in pursue, in flee, in constrain, in wander, in boid, in steering);

                boid.Force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                boid.Acceleration = boid.Force / boid.Mass;
                boid.Velocity += boid.Acceleration * DeltaTime;

                boid.Velocity = MathUtil.ClampMagnitude(boid.Velocity, boid.MaxSpeed);

                boid.Speed = math.length(boid.Velocity);
                if (boid.Speed > 0) {
                    
                    var tempUp = math.lerp(boid.Up, math.up() + (boid.Acceleration * boid.Banking), DeltaTime * 3.0f);
                    rotation.Value = quaternion.LookRotation(boid.Velocity, tempUp);
                    boid.Up = tempUp * math.up();

                    translation.Value += boid.Velocity * DeltaTime;
                    boid.Velocity *= ( 1.0f - ( boid.Damping * DeltaTime ) );

                }
                    
                translationData[i] = translation;
                rotationData[i] = rotation;
                boidData[i] = boid;

            }

        }

        private static float3 CalculateForces(in SeekData seek, in ArriveData arrive, in PursueData pursue, in FleeData flee, in ConstrainData constrain, in WanderData wander, in BoidData boid, in ActiveSteeringData steering) {
            var force = float3.zero;

            if (steering.Seek == 1) {
                force += seek.Force * seek.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            if (steering.Arrive == 1) {
                force += arrive.Force * arrive.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            if (steering.Pursue == 1) {
                force += pursue.Force * pursue.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            if (steering.Flee == 1) {
                force += flee.Force * flee.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            if (steering.Constrain == 1) {
                force += constrain.Force * constrain.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            if (steering.Wander == 1) {
                force += wander.Force * wander.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            return force;
        }
    }

}