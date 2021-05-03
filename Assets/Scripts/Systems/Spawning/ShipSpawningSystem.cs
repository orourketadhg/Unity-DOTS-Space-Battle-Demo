using Ie.TUDublin.GE2.Components.Spawning;
using Ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Spawning {
    
    public class ShipSpawningSystem : SystemBase {
        
        private BeginSimulationEntityCommandBufferSystem _entityCommandBuffer;
        
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

                        int numShips = (int) math.ceil(random.NextInt(shipElement.numberOfShips.x, shipElement.numberOfShips.y) * spawningSettings.shipCountMultiplier);

                        for (int i = 0; i < numShips; i++) {

                            var spawnArea = spawningSettings.spawnArea * spawningSettings.shipCountMultiplier;
                            var instancePosition = spawningSettings.position + random.NextFloat3(-spawnArea, spawnArea);
                            var instanceRotation = quaternion.EulerXYZ(spawningSettings.rotation);
                            
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