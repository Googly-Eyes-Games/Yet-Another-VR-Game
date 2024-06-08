
using UnityEngine;

public static class MathUtils
{
    public static Vector3 NearestPointOnSegment(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 direction = (end - start).normalized;
        Vector3 v = point - start;
        float d = Vector3.Dot(v, direction);
        return start + (direction * d);
    }

    public static float Remap(float aMin, float aMax, float bMin, float bMax, float value)
    {
        float t = Mathf.InverseLerp(aMin, aMax, value);
        return Mathf.Lerp(bMin, bMax, t);
    }
    
    //https://forum.unity.com/threads/manually-calculate-angular-velocity-of-gameobject.289462/#post-4302796
    public static Vector3 CalculateAngularVelocity(Quaternion lastFrameRotation, Quaternion currentRotation)
    {
        Quaternion q = currentRotation * Quaternion.Inverse(lastFrameRotation);
        // no rotation?
        // You may want to increase this closer to 1 if you want to handle very small rotations.
        // Beware, if it is too close to one your answer will be Nan
        if (Mathf.Abs(q.w) > 1023.5f / 1024.0f)
            return Vector3.zero;
        
        float gain;
        // handle negatives, we could just flip it but this is faster
        if (q.w < 0.0f)
        {
            float angle = Mathf.Acos(-q.w);
            gain = -2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        else
        {
            float angle = Mathf.Acos(q.w);
            gain = 2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }

        Vector3 angularVelocity = new Vector3(q.x * gain, q.y * gain, q.z * gain);
        
        if (float.IsNaN(angularVelocity.z))
            angularVelocity = Vector3.zero;

        return angularVelocity;
    }

    public static Vector2 ToXZPlane(this Vector3 x)
    {
        return new Vector2(x.x, x.z);
    }
}