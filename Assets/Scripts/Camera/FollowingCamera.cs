using ie.TUDublin.GE2.Components.Camera;
using ie.TUDublin.GE2.Components.Steering;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ie.TUDublin.GE2.Camera {

    public class FollowingCamera : MonoBehaviour {

        private EntityManager _manager;
        private EntityQuery _shipQuery;

        private Entity _target;
        
        private void Awake() {
            _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _shipQuery = _manager.CreateEntityQuery(new EntityQueryDesc() {
                Any = new ComponentType[] {
                    typeof(CameraData),
                    typeof(BoidData)
                },
                None = new ComponentType[] {
                    typeof(Prefab)
                }
            });
        }

        private void OnEnable() {
            var alliedShips = _shipQuery.ToEntityArray(Allocator.Temp);
            _target = alliedShips[Random.Range(0, alliedShips.Length)];
            alliedShips.Dispose();
        }

        private void Update() {

            if (_target == Entity.Null || !_manager.Exists(_target) || Input.GetKeyDown(KeyCode.Mouse0)) {
                var alliedShips = _shipQuery.ToEntityArray(Allocator.Temp);
                _target = alliedShips[Random.Range(0, alliedShips.Length)];
                alliedShips.Dispose();
            }

            var targetTranslation = _manager.GetComponentData<Translation>(_target);
            var cameraOffset = _manager.GetComponentData<CameraData>(_target);

            transform.position = Vector3.Lerp(transform.position, targetTranslation.Value + cameraOffset.Offset, 0.1f);
            transform.LookAt(targetTranslation.Value);

        }

    }

}