using Ie.TUDublin.GE2.Components.Steering;
using Ie.TUDublin.GE2.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Steering {

    [BurstCompile]
    public struct SteeringJob : IJobEntityBatch {

        [NativeSetThreadIndex] private int _nativeThreadIndex;
        
        public NativeArray<Random> randomArray;
        public float deltaTime;
        
        // Transform and physics components
        public ComponentTypeHandle<Translation> TranslationHandle;
        public ComponentTypeHandle<Rotation> RotationHandle;
        public ComponentTypeHandle<PhysicsVelocity> VelocityHandle;
        public ComponentTypeHandle<LocalToWorld> LocalToWorldHandle;
        
        // Steering components
        public ComponentTypeHandle<SpaceshipData> SpaceshipHandle;
        public ComponentTypeHandle<JitterWanderData> wanderHandle;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {

            var translations = batchInChunk.GetNativeArray(TranslationHandle);
            var rotations = batchInChunk.GetNativeArray(RotationHandle);
            var velocities = batchInChunk.GetNativeArray(VelocityHandle);
            var localToWorlds = batchInChunk.GetNativeArray(LocalToWorldHandle);

            var spaceships = batchInChunk.GetNativeArray(SpaceshipHandle);
            var wanderArray = batchInChunk.GetNativeArray(wanderHandle);

            var random = randomArray[_nativeThreadIndex];
            
            for (int i = 0; i < batchInChunk.Count; i++) {

                var translation = translations[i];
                var rotation = rotations[i];
                var velocity = velocities[i];
                var ltw = localToWorlds[i];

                var ship = spaceships[i];
                var wander = wanderArray[i];
                
                // wander
                
                // pursue

                // flee 
                
                // obstacle avoidance
                
                // constrain

            }

            randomArray[_nativeThreadIndex] = random;

        }
        
        private float3 CalculateSeekForce(in Translation position, in PhysicsVelocity velocity, in SpaceshipData spaceship, float3 target) {
            var desired = target - position.Value;
            desired = math.normalizesafe(desired);
            desired *= spaceship.MaxSpeed;
            return desired - velocity.Linear;
        }

        private float3 CalculateWanderForce(ref Random random, ref JitterWanderData wander, Translation translation, Rotation rotation) {
            var displacement = wander.Jitter * MathUtil.InsideUnitSphere(ref random) * deltaTime;
            wander.Target += displacement;
            
            wander.Target = math.normalizesafe(wander.Target);
            wander.Target *= wander.Radius;

            var localTarget = ( new float3(0, 0, 1) * wander.Distance ) + wander.Target;

            var pos = translation.Value;
            var rot = rotation.Value;
            var worldTarget = math.mul(rot, localTarget) + pos;

            return ( worldTarget - pos ) * wander.Weight;
        }

        private float3 CalculatePursueForce(ref PursueData pursue, in Translation translation, in PhysicsVelocity velocity, in SpaceshipData spaceship) {
            float dist = MathUtil.Float3Distance(spaceship.TargetPosition, translation.Value);
            float time = dist / spaceship.MaxSpeed;

            var target = spaceship.TargetPosition + ( velocity.Linear * time );

            return CalculateSeekForce(in translation, in velocity, in spaceship, target);
        }
        
    }

}