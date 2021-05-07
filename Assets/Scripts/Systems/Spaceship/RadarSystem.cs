using System.Linq;
using ie.TUDublin.GE2.Components.Spaceship;
using ie.TUDublin.GE2.Components.Statemachine;
using ie.TUDublin.GE2.Components.Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace ie.TUDublin.GE2.Systems.Spaceship {

    /// <summary>
    /// system to detect targets
    /// </summary>
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

                    // check entity has a target
                    if (targetingData.Target != Entity.Null) {
                        return;
                    }

                    // calculate overlap parameters
                    var radarPositionA = ltw.Position;
                    var radarPositionB = ltw.Position + ( ltw.Forward * radar.Distance );
                    var radarHits = new NativeList<DistanceHit>(Allocator.Temp);
                    var radarFilter = new CollisionFilter() {
                        BelongsTo = ~0u,
                        CollidesWith = ( 1u << 5 ),
                        GroupIndex = 0
                    };

                    // cast overlap
                    if (physicsWorld.OverlapCapsule(radarPositionA, radarPositionB, radar.Radius, ref radarHits, radarFilter)) {
                        for (int i = 0; i < radarHits.Length; i++) {
                            
                            // check found targets are on the opposing team
                            var entityHit = radarHits[i].Entity;
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
                    else {
                        Debug.Log("Failure");
                    }
                }).Schedule();
        }
        
    }

}