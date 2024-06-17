using UnityEngine;

public static class LinearAlgebra
{
    public static Vector3 NearestPointOnSegment(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 direction = (end - start).normalized;
        Vector3 v = point - start;
        float d = Vector3.Dot(v, direction);
        d = Mathf.Clamp(d, 0, (end - start).magnitude);
        return start + (direction * d);
    }

    // https://forum.unity.com/threads/how-to-find-line-of-intersecting-planes.109458/#post-725977
    public static bool PlanePlaneIntersection(
        out Vector3 linePoint, out Vector3 lineVec,
        Vector3 plane1Normal, Vector3 plane1Position,
        Vector3 plane2Normal, Vector3 plane2Position)
    {
        linePoint = Vector3.zero;
        lineVec = Vector3.zero;
        
        //We can get the direction of the line of intersection of the two planes by calculating the 
        //cross product of the normals of the two planes. Note that this is just a direction and the line
        //is not fixed in space yet. We need a point for that to go with the line vector.
        lineVec = Vector3.Cross(plane1Normal, plane2Normal);
        
        //Next is to calculate a point on the line to fix it's position in space. This is done by finding a vector from
        //the plane2 location, moving parallel to it's plane, and intersecting plane1. To prevent rounding
        //errors, this vector also has to be perpendicular to lineDirection. To get this vector, calculate
        //the cross product of the normal of plane2 and the lineDirection.      
        Vector3 ldir = Vector3.Cross(plane2Normal, lineVec);        
        
        float denominator = Vector3.Dot(plane1Normal, ldir);
        
        //Prevent divide by zero and rounding errors by requiring about 5 degrees angle between the planes.
        if(Mathf.Abs(denominator) > 0.006f){
            
            Vector3 plane1ToPlane2 = plane1Position - plane2Position;
            float t = Vector3.Dot(plane1Normal, plane1ToPlane2) / denominator;
            linePoint = plane2Position + t * ldir;
            
            return true;
        }
        
        //output not valid
        else{
            return false;
        }
    }
}