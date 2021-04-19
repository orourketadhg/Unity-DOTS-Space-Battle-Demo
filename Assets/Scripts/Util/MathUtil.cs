using Unity.Burst;
using Unity.Mathematics;

namespace Ie.TUDublin.GE2.Util {

    [BurstCompile]
    public static class MathUtil {

        public  static float3 InsideUnitSphere(ref Random random) {
            float t = math.PI * random.NextFloat();
            float v = random.NextFloat();
            float phi = math.acos(( 2 * v ) - 1);

            float r = math.pow(random.NextFloat(), 0.333f);
            
            float x = r * math.sin(phi) * math.cos(t);
            float y = r * math.sin(phi) * math.sin(t);
            float z = r * math.cos(phi);
            
            return new float3(x, y, z);
        }
        
        public static float3 OnUnitSphere(ref Random random) {

            float a = 2 * math.PI * random.NextFloat();
            float b = math.acos(1 - 2 * random.NextFloat());
            
            float x = math.sin(b) * math.cos(a);
            float y = math.sin(b) * math.sin(a);
            float z = math.cos(b);
            
            return new float3(x, y, z);
        }
        
    }

}