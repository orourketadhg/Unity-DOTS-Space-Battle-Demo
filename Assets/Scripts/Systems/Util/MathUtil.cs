using Unity.Burst;
using Unity.Mathematics;

namespace ie.TUDublin.GE2.Systems.Util {

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

        public static float Float3Distance(float3 v, float3 w) {
            float x = math.pow(( v.x - w.x ), 2);
            float y = math.pow(( v.y - w.y ), 2);
            float z = math.pow(( v.z - w.z ), 2);

            float a = math.sqrt(x + y + z);
            
            return math.abs(a);
        }

        public static float3 ClampMagnitude(float3 vector, float maxLength) {
            float sqrLength = math.lengthsq(vector);

            if (sqrLength <= maxLength * maxLength) return vector;

            float length = (float) math.sqrt((double) sqrLength);
            float x = vector.x / length;
            float y = vector.y / length;
            float z = vector.z / length;

            return new float3(x * maxLength, y * maxLength, z * maxLength);
        }
        
    }

}