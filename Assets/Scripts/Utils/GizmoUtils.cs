using UnityEngine;

public class GizmoUtils
{
    public static void DrawPlaneGizmos(Vector3 position, Plane plane)
    {
        Gizmos.DrawLine(position, position + plane.normal * 0.1f);
    }

    public static void DrawSphereOnPlane(Vector3 position, Plane plane, float radius)
    {
        Gizmos.DrawWireSphere(position, radius);
    }
}