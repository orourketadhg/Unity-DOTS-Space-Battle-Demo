using ie.TUDublin.GE2.Components.Spawning;
using ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Spawning {
    
    /// <summary>
    /// System to spawn ships at the start of a fight
    /// </summary>
    public class ShipSpawningSystem : SystemBase {
        
        private BeginSimulationEntityCommandBufferSystem _entityCommandBuffer;

        public JobHandle OutDependency => Dependency;
        
        protected override void OnCreate() {
            _entityCommandBuffer = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {

            var ecb = _entityCommandBuffer.CreateCommandBuffer();
            var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;

            Entities
                .WithName("ShipSpawning")
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, in ShipSpawningSettingsData spawningSettings, in DynamicBuffer<ShipSpawnElement> shipBuffer) => {
                    var random = randomArray[nativeThreadIndex];
                    
                    foreach (var shipElement in shipBuffer) {

                        // calculate number of ships to spawn of this ship type to spawn 
                        int numShips = (int) math.ceil(random.NextInt(shipElement.numberOfShips.x, shipElement.numberOfShips.y) * spawningSettings.shipCountMultiplier);

                        for (int i = 0; i < numShips; i++) {

                            // calculate ship positioning and rotation
                            var spawnArea = spawningSettings.spawnArea * spawningSettings.shipCountMultiplier;
                            var instancePosition = spawningSettings.position + random.NextFloat3(-spawnArea, spawnArea);
                            var instanceRotation = quaternion.Euler(spawningSettings.rotation);
                            
                            // create ship instance and assign calculated component data
                            var instance = ecb.Instantiate(shipElement.prefab);
                            ecb.SetComponent(instance, new Translation() {Value = instancePosition});
                            ecb.SetComponent(instance, new Rotation() {Value = instanceRotation});
                        }
                    }
                    
                    ecb.DestroyEntity(entity);
                }).Schedule();
            
            _entityCommandBuffer.AddJobHandleForProducer(Dependency);
        }
        
    }

}