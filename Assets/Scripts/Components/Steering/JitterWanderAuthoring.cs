using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ie.TUDublin.GE2.Components.Steering {

    public class JitterWanderAuthoring : MonoBehaviour, IConvertGameObjectToEntity {

        [SerializeField] private float distance;
        [SerializeField] private float radius;
        [SerializeField] private float jitter;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
            dstManager.AddComponentData(entity, new JitterWanderData() {
                Distance = distance,
                Radius = radius,
                Jitter = jitter,
                Target = Random.insideUnitSphere * radius
            });
        }
    }
    
    public struct JitterWanderData : IComponentData {
        public float Distance;
        public float Radius;
        public float Jitter;
        public float3 Target; 
    }

}