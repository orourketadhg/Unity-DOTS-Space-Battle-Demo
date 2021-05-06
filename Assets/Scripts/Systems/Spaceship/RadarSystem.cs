using System.Linq;
using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Statemachine;
using ie.TUDublin.GE2.Components.Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace ie.TUDublin.GE2.Systems.Spaceship {

    public class RadarSystem : SystemBase {

        private BuildPhysicsWorld _buildPhysicsWorld;
        
        protected override void OnCreate() {
            _buildPhysicsWorld = World.GetExistingSystem<BuildPhysicsWorld>();
        }

        protected override void OnUpdate() {

            var physicsWorld = _buildPhysicsWorld.PhysicsWorld;

            Entities
                .WithName("RadarSystem")
                .WithBurst()
                .WithAll<SearchState>()
                .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref TargetingData targetingData, in RadarData radar, in LocalToWorld ltw) => {

                    if (targetingData.Target != Entity.Null) {
                        return;
                    }

                    var radarPositionA = ltw.Position;
                    var radarPositionB = ltw.Position + ( ltw.Forward * radar.Distance );
                    var radarHits = new NativeList<DistanceHit>(Allocator.TempJob);
                    var radarFilter = new CollisionFilter() {
                        BelongsTo = ~0u,
                        CollidesWith = ( 1u << 5 ),
                        GroupIndex = 0
                    };

                    if (physicsWorld.OverlapCapsule(radarPositionA, radarPositionB, radar.Radius, ref radarHits, radarFilter)) {
                        foreach (var entityHit in radarHits.Select(hit => hit.Entity)) {
                            if (HasComponent<AlliedTag>(entity) && HasComponent<EnemyTag>(entityHit)) {
                                GetBuffer<PursuerElementData>(entityHit).Add(new PursuerElementData() {PursuerEntity = entityHit});
                                targetingData.Target = entityHit;
                                break;
                            }
                            
                            if (HasComponent<AlliedTag>(entityHit) && HasComponent<EnemyTag>(entity)) {
                                
                                GetBuffer<PursuerElementData>(entityHit).Add(new PursuerElementData() {PursuerEntity = entityHit});
                                targetingData.Target = entityHit;
                                break;
                            }
                        }
                    }
                }).ScheduleParallel();
            
            // Entities
            //     .WithName("PursuersCleanupSystem")
            //     .WithBurst()
            //     .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref DynamicBuffer<PursuerElementData> pursuers) => {
            //         for (int index = pursuers.Length - 1; index >= 0; index--) {
            //             var pursuer = pursuers[index];
            //             if (pursuer.PursuerEntity == Entity.Null) {
            //                 pursuers.RemoveAt(index);
            //             }
            //         }
            //     }).ScheduleParallel();
                
        }
        
    }

}