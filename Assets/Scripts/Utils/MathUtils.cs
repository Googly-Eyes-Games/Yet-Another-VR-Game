
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
}