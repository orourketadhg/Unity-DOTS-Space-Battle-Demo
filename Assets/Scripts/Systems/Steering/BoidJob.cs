using ie.TUDublin.GE2.Components.Steering;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Steering {

    /// <summary>
    /// Job to calculate and apply forces from steering behaviours
    /// </summary>
    [BurstCompile]
    public struct BoidJob : IJobEntityBatch {

        public float DeltaTime;
        
        // Component Handlers
        [ReadOnly] public ComponentTypeHandle<SeekData> SeekHandle; 
        [ReadOnly] public ComponentTypeHandle<ArriveData> ArriveHandle; 
        [ReadOnly] public ComponentTypeHandle<PursueData> PursueHandle; 
        [ReadOnly] public ComponentTypeHandle<FleeData> FleeHandle;
        [ReadOnly] public ComponentTypeHandle<ConstrainData> ConstrainHandle;
        [ReadOnly] public ComponentTypeHandle<WanderData> WanderHandle;
        [ReadOnly] public ComponentTypeHandle<SteeringData> SteeringHandle;

        public ComponentTypeHandle<Translation> TranslationHandle;
        public ComponentTypeHandle<Rotation> RotationHandle;
        public ComponentTypeHandle<BoidData> BoidData;
        
        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {
            // Get component arrays from batch
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
                // get entities components and data
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
                
                // accumulate forces
                var force = CalculateForces(in seek, in arrive, in pursue, in flee, in constrain, in wander, in boid, in steering);

                // calculate velocity based on forces 
                boid.Force = MathUtil.ClampMagnitude(force, boid.MaxForce);
                boid.Acceleration = boid.Force / boid.Mass;
                boid.Velocity += boid.Acceleration * DeltaTime;
                boid.Velocity = MathUtil.ClampMagnitude(boid.Velocity, boid.MaxSpeed);

                // apply velocity to boid
                boid.Speed = math.length(boid.Velocity);
                if (boid.Speed > 0) {
                    
                    var tempUp = math.lerp(boid.Up, math.up() + (boid.Acceleration * boid.Banking), DeltaTime * 3.0f);
                    rotation.Value = quaternion.LookRotation(boid.Velocity, tempUp);
                    boid.Up = tempUp * math.up();

                    translation.Value += boid.Velocity * DeltaTime;
                    boid.Velocity *= ( 1.0f - ( boid.Damping * DeltaTime ) );

                }
                
                // return data
                translationData[i] = translation;
                rotationData[i] = rotation;
                boidData[i] = boid;

            }

        }

        /// <summary>
        /// Accumulate forces from steering behaviours
        /// </summary>
        /// <param name="seek">Seek Component Data</param>
        /// <param name="arrive">Arrive Component Data</param>
        /// <param name="pursue">Pursue Component Data</param>
        /// <param name="flee">Flee Component Data</param>
        /// <param name="constrain">Constrain Component Data</param>
        /// <param name="wander">Wander Component Data</param>
        /// <param name="boid">Boid data</param>
        /// <param name="steering">enabled steering behaviours</param>
        /// <returns></returns>
        private static float3 CalculateForces(in SeekData seek, in ArriveData arrive, in PursueData pursue, in FleeData flee, in ConstrainData constrain, in WanderData wander, in BoidData boid, in SteeringData steering) {
            var force = float3.zero;

            // apply seek forces
            if (steering.Seek == 1) {
                force += seek.Force * seek.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            // apply arrive forces
            if (steering.Arrive == 1) {
                force += arrive.Force * arrive.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            // apply pursue forces
            if (steering.Pursue == 1) {
                force += pursue.Force * pursue.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            // apply flee forces
            if (steering.Flee == 1) {
                force += flee.Force * flee.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            // apply constrain forces
            if (steering.Constrain == 1) {
                force += constrain.Force * constrain.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            // apply wander forces
            if (steering.Wander == 1) {
                force += wander.Force * wander.Weight;
                force = MathUtil.ClampMagnitude(force, boid.MaxForce);
            }
            
            return force;
        }
    }

}