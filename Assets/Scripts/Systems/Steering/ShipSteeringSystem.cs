using Ie.TUDublin.GE2.Components.Steering;
using Ie.TUDublin.GE2.Systems.Util;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Ie.TUDublin.GE2.Systems.Steering {

    public class ShipSteeringSystem : SystemBase {

        protected override void OnUpdate() {

            var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;
            float dt = Time.DeltaTime;
            
        }
        
    }

}