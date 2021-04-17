using Unity.Entities;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Components.Spaceship {

    [GenerateAuthoringComponent]
    public struct HealthData : IComponentData {
        private int _value;

        public void TakeDamage(int damage) {
            _value = math.clamp(_value - damage, 0, _value);
        }
    }

}