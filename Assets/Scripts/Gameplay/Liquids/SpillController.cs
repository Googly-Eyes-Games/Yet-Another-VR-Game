using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;

// SS: Modified code found on github

/**
    MIT License
   
   Copyright (c) 2019 Macoron
   
   Permission is hereby granted, free of charge, to any person obtaining a copy
   of this software and associated documentation files (the "Software"), to deal
   in the Software without restriction, including without limitation the rights
   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
   copies of the Software, and to permit persons to whom the Software is
   furnished to do so, subject to the following conditions:
   
   The above copyright notice and this permission notice shall be included in all
   copies or substantial portions of the Software.
   
   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
   SOFTWARE.
   
 */

/// <summary>
/// Calculates liquids splitting effect and transfer to other liquid containers
/// https://github.com/Macoron/Unity-Simple-Liquid
/// </summary>
public class SpillController : MonoBehaviour
{
    public LiquidHandler liquidHandler;

    [SerializeField]
    private float bottleneckRadius = 0.1f;

    [SerializeField]
    private Transform bottleneckSocket;

    [Tooltip("How fast liquid split from container")]
    public float splitSpeed = 2f;

    [Tooltip("Number number of objects the liquid will hit off and continue flowing")]
    public int maxEdgeDrops = 4;

    private int currentDrop;

    #region Particles

    [SerializeField]
    private ParticleSystem particles;

    [SerializeField]
    private Vector3 localSpillOffset;

    private void StartEffect(Vector3 splitPos, float scale)
    {
        if (!particles)
            return;

        particles.transform.localScale = Vector3.one * (bottleneckRadius * scale);
        particles.transform.position = splitPos + transform.TransformDirection(localSpillOffset);
        particles.Play();
    }

    #endregion

    #region Bottleneck

    public Plane bottleneckPlane { get; private set; }
    public Plane surfacePlane { get; private set; }
    public Vector3 BottleneckPos { get; private set; }
    public Vector3 OverflowPoint { get; private set; }

    private Plane CalculateBottleneckPlane()
    {
        return new Plane(bottleneckSocket.up, bottleneckSocket.position);
    }

    private Vector3 CalculateBottleneckPos()
    {
        return bottleneckSocket.position;
    }

    private Vector3 GenerateBottleneckLowesPoint()
    {
        if (!liquidHandler)
            return Vector3.zero;

        // Calculate the direction vector of the bottlenecks slope

        Vector3 bottleneckSlope = GetSlopeDirection(Vector3.up, bottleneckPlane.normal);

        // Find a position along the slope the side of the bottleneck radius
        Vector3 min = BottleneckPos + bottleneckSlope * bottleneckRadius;

        return min;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        // Draws bottleneck direction and radius
        Plane bottleneckPlaneLocal = CalculateBottleneckPlane();

        Gizmos.color = Color.red;
        GizmoUtils.DrawPlaneGizmos(CalculateBottleneckPos(), bottleneckPlaneLocal);

        // And bottleneck position
        GizmoUtils.DrawSphereOnPlane(CalculateBottleneckPos(), bottleneckPlaneLocal, bottleneckRadius);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(OverflowPoint, 0.01f);
        
        // Draw a yellow sphere at the transform's position
        if (raycasthit == Vector3.zero)
            return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(raycasthit, 0.01f);
        Gizmos.DrawSphere(raycastStart, 0.01f);
    }

    #endregion

    #region Split Logic

    private const float splashSize = 0.025f;

    public bool IsSpilling { get; private set; }

    private void CheckSpilling()
    {
        IsSpilling = false;

        if (!liquidHandler)
            return;

        // Do we have something to split?
        if (liquidHandler.FillAmount <= 0f)
            return;

        // Check if liquid is overflows
        bool overflows = LinearAlgebra.PlanePlaneIntersection(
            out Vector3 tempOverflowPoint, out Vector3 _,
             bottleneckPlane.normal,BottleneckPos,
             liquidHandler.SurfaceNormal, liquidHandler.SurfacePosition); 
        
        Debug.DrawRay(liquidHandler.SurfacePosition, liquidHandler.SurfaceNormal, Color.red);
        Debug.DrawRay(BottleneckPos, bottleneckPlane.normal, Color.blue);
        // Debug.DrawRay(overflowPoint, bottleneckPlane.normal, Color.green);
        
        // Translate to contrainers world position
        // overflowsPoint += liquidHandler.transform.position;

        if (overflows)
        {
            OverflowPoint = tempOverflowPoint;
            
            // Let's check if overflow point is inside bottleneck radius
            bool insideBottleneck = Vector3.Distance(OverflowPoint, BottleneckPos) < bottleneckRadius;

            if (insideBottleneck)
            {
                // We are inside bottleneck - just start spliting from lowest bottleneck point
                Vector3 minPoint = GenerateBottleneckLowesPoint();
                SplitLogic(minPoint);
                return;
            }
        }

        if (BottleneckPos.y < OverflowPoint.y)
        {
            // Oh, looks like container is upside down - let's check it
            float dot = Vector3.Dot(bottleneckPlane.normal, surfacePlane.normal);
            if (dot < 0f)
            {
                // Yep, let's split from the bottleneck center
                SplitLogic(BottleneckPos);
            }
            else
            {
                // Well, this weird, let's check if spliting point is even inside our liquid
                float dist = liquidHandler.MeshRenderer.bounds.SqrDistance(OverflowPoint);
                bool inBounding = dist < 0.0001f;

                if (inBounding)
                {
                    // Yeah, we are inside liquid container
                    var minPoint = GenerateBottleneckLowesPoint();
                    SplitLogic(minPoint);
                }
            }
        }
    }

    private void SplitLogic(Vector3 splitPos)
    {
        IsSpilling = true;

        // Check rotation of liquid container
        // It conttolls how many liquid we lost and particles size
        float howLow = Vector3.Dot(Vector3.up, liquidHandler.transform.up);
        float flowScale = 1f - (howLow + 1) * 0.5f + 0.2f;

        float liquidStep = bottleneckRadius * splitSpeed * Time.deltaTime * flowScale;
        float newLiquidAmount = liquidHandler.FillAmount - liquidStep;

        // Check if amount is negative and change it to zero
        if (newLiquidAmount < 0f)
        {
            liquidStep = liquidHandler.FillAmount;
            newLiquidAmount = 0f;
        }

        // Transfer liquid to other container (if possible)
        liquidHandler.FillAmount = newLiquidAmount;

        RaycastHit containerHit = FindLiquidContainer(splitPos, this.gameObject);

        //RaycastHit is a struct which gives us everything we need
        if (containerHit.collider)
        {
            TransferLiquid(containerHit, liquidStep, flowScale);
        }

        // Start particles effect
        StartEffect(splitPos, flowScale);
    }


    //Used for Gizmo only
    private Vector3 raycasthit;
    private Vector3 raycastStart;

    private void TransferLiquid(RaycastHit hit, float lostPercentAmount, float scale)
    {
        SpillController other = hit.collider.GetComponent<SpillController>();

        Vector3 otherBottleneck = other.CalculateBottleneckPos();
        float radius = other.bottleneckRadius;

        Vector3 hitPoint = hit.point;

        // Do we touched bottleneck?
        bool insideRadius = Vector3.Distance(hitPoint, otherBottleneck) < radius + splashSize * scale;
        if (insideRadius)
        {
            other.liquidHandler.LiquidVolume += lostPercentAmount * liquidHandler.LiquidVolume;

            //color change in capacity
            SendLiquidContainer(other.liquidHandler);
        }
    }

    private RaycastHit FindLiquidContainer(Vector3 splitPos, GameObject ignoreCollision)
    {
        var ray = new Ray(splitPos, Vector3.down);

        // Check all colliders under ours
        var hits = Physics.SphereCastAll(ray, splashSize);
        hits = hits.OrderBy((h) => h.distance).ToArray();

        foreach (var hit in hits)
        {
            //Ignore ourself
            if (!GameObject.ReferenceEquals(hit.collider.gameObject, ignoreCollision) && !hit.collider.isTrigger)
            {
                // does it even a split controller
                var liquid = hit.collider.GetComponent<SpillController>();
                if (liquid && liquid != this)
                {
                    return hit;
                }


                //Something other than a liquid splitter is in the way
                if (!liquid)
                {
                    //If we have already dropped down off too many objects, break

                    if (currentDrop >= maxEdgeDrops)
                    {
                        //if we have rolled down too many objects, return empty hit
                        //This assumes at this point the liquid has "dried up" rather than pouring from the last valid point
                        return new RaycastHit();
                    }
                    //Simulate the liquid running off an object it hits and continuing down from the edge of the liquid
                    //Does not take velocity into account

                    //First get the slope direction
                    Vector3 slope = GetSlopeDirection(Vector3.up, hit.normal);

                    //Next we try to find the edge of the object the liquid would roll off
                    //This really only works for primitive objects, it would look weird on other stuff
                    Vector3 edgePosition = TryGetSlopeEdge(slope, hit);
                    if (edgePosition != Vector3.zero)
                    {
                        //edge position found, surface must be tilted
                        //Now we can try to transfer the liquid from this position
                        currentDrop++;
                        return FindLiquidContainer(edgePosition, hit.collider.gameObject);
                    }

                    return new RaycastHit();
                }
            }
        }

        return new RaycastHit();
    }

    #endregion

    #region ChangeColor

    //playerMultiply for faster color change
    [Range(0, 2)]
    [Tooltip("Mixing speed ratio of different colors")]
    public float mixingSpeed = 1;

    private void SendLiquidContainer(LiquidHandler other)
    {
        //find the color and split speed
        Color newColor = this.liquidHandler.LiquidColor;

        //we find the coefficient of the volume of the tank and the volume of the incoming fluid
        float volume = other.LiquidVolume;
        float koof = splitSpeed / (volume * 1000);
        other.LiquidColor = Color.Lerp(other.LiquidColor, newColor, koof * mixingSpeed);
    }

    #endregion

    #region Slope Logic

    private float GetIdealRayCastDist(Bounds boundBox, Vector3 point, Vector3 slope)
    {
        Vector3 final = boundBox.min;

        // X axis	
        if (slope.x > 0)
            final.x = boundBox.max.x;
        // Y axis	
        if (slope.y > 0)
            final.y = boundBox.max.y;
        // Z axis
        if (slope.z > 0)
            final.z = boundBox.max.z;

        return Vector3.Distance(point, final);
    }

    private Vector3 GetSlopeDirection(Vector3 up, Vector3 normal)
    {
        //https://forum.unity.com/threads/making-a-player-slide-down-a-slope.469988/#post-3062204			
        return Vector3.Cross(Vector3.Cross(up, normal), normal).normalized;
    }

    private Vector3 TryGetSlopeEdge(Vector3 slope, RaycastHit hit)
    {
        Vector3 edgePosition = Vector3.zero;

        // We need to pick a position outside of the object to raycast back towards it to find an edge.
        // We need a position slightly down so it will hit the edge of the object
        Vector3 moveDown = new Vector3(0f, -0.0001f, 0f);
        // We also need to move the position outside of the objects bounding box, so we actually hit it
        float dist = GetIdealRayCastDist(hit.collider.bounds, hit.point, slope);

        Vector3 reverseRayPos = hit.point + moveDown + (slope * dist);
        raycastStart = reverseRayPos;
        Ray backwardsRay = new Ray(reverseRayPos, -slope);
        RaycastHit[] revHits = Physics.RaycastAll(backwardsRay);

        foreach (var revHit in revHits)
        {
            // https://answers.unity.com/questions/752382/how-to-compare-if-two-gameobjects-are-the-same-1.html
            //We only want to get this position on the original object we hit off of
            if (GameObject.ReferenceEquals(revHit.collider.gameObject, hit.collider.gameObject))
            {
                //We hit the object the liquid is running down!
                raycasthit = edgePosition = revHit.point;
                break;
            }
        }

        return edgePosition;
    }

    #endregion

    private void Update()
    {
        // Update bottleneck and surface from last update
        bottleneckPlane = CalculateBottleneckPlane();
        BottleneckPos = CalculateBottleneckPos();
        surfacePlane = new Plane(liquidHandler.SurfacePosition, liquidHandler.SurfaceNormal);

        // Now check spliting, starting from the top
        currentDrop = 0;
        CheckSpilling();
    }
    
}