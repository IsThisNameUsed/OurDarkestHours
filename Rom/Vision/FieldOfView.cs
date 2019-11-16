using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public int EnlightementLevel = 1;
    public float ViewRadius = 20f;                          // View distance
    [Range(0, 365)] public float ViewAngle;                 // View angle

    public LayerMask TargetMask;                            // Enemies mask
    public LayerMask ObstacleMask;                          // Obstacles mask
    public LayerMask ObstacleMaskRender;                    // Visibles obstacles mask
    public Vector3 RayOffset = Vector3.up;

    public float UpdateRate = 10;                           // Updates per seconds for enemy detection
    [Range(0, 0.5f)] public float MeshResolution = 0.2f;    // 0.5 = best precision, 1 ray per 2 degrees
    public int EdgeResolveIterations = 5;                   // Number of iterations when trying to find an edge
    public float EdgeDistanceThreshold = 1;                 // Maximum distance to try to find edge
    
    public List<Lightable> VisibleTargets = new List<Lightable>();  // Visible targets, recalculation time set by UpdateRate

    public bool drawGizmo;

    private GameObject gameManager;
    private EnemyTargeted enemyTargeted;

    void Start()
    {
        // Update targets
        StartCoroutine(FindTargetsCoroutine());

        gameManager = GameObject.Find("GameManager");
        enemyTargeted = gameManager.GetComponent<EnemyTargeted>();
    }

    void LateUpdate()
    {
        // Generate mesh and update visualization
        if(ViewAngle > 0)
            DrawFieldOfView();
    }

    /// <summary>
    /// Update visible targets list every {delay} seconds
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator FindTargetsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / UpdateRate);
            
            foreach (Lightable lightable in VisibleTargets)
            {
                if (lightable != null)
                {
                    lightable.LightSources -= EnlightementLevel;
                }
            }
            
            FindVisibleTargets();

            foreach (Lightable lightable in VisibleTargets)
            {
                if (lightable != null)
                {
                    lightable.LightSources += EnlightementLevel;
                }
            }
            enemyTargeted.majVisibleTargeted(VisibleTargets,name);
        }
    }
    /// <summary>
    /// Update targets list
    /// </summary>
    void FindVisibleTargets()
    {
        VisibleTargets.Clear();

        // Get all targets in radius
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, TargetMask);
        
        for (int i = 0; i < targetsInViewRadius.Length; ++i)
        {
            //Transform target = targetsInViewRadius[i].transform;
            Lightable lightable = targetsInViewRadius[i].GetComponent<Lightable>();
            if (lightable == null || targetsInViewRadius[i].transform.name == transform.name)
                continue;

            Vector3 rayOrigin = transform.position + RayOffset;
            Vector3 dirToTarget = (lightable.RayTargetPoint - rayOrigin).normalized;

            // For each target, check if it's in angle
            float angle = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(dirToTarget.x, dirToTarget.z));
            if (angle < ViewAngle / 2)
            {
                // +1 to avoid weird ray inconsistency across frames
                float distToTarget = Vector3.Distance(rayOrigin, lightable.RayTargetPoint) + 1;
                if (lightable.name.ToLower().Contains("angel") && drawGizmo)
                    Debug.DrawRay(rayOrigin, dirToTarget * distToTarget, Color.red, 1f / UpdateRate);

                // Check if there isn't any obstacle between source and target 
                RaycastHit[] hits = Physics.RaycastAll(rayOrigin, dirToTarget, distToTarget, ObstacleMask);
                RaycastHit nearestHit = new RaycastHit { distance = float.MaxValue };
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform == transform)
                        continue;

                    if (hit.distance < nearestHit.distance)
                        nearestHit = hit;
                }
                if (nearestHit.transform == lightable.transform)
                    VisibleTargets.Add(lightable);
            }
        }
    }

    /// <summary>
    /// Draw visualization cone
    /// </summary>
    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(ViewAngle * MeshResolution);   // Number of rays
        float stepAngleSize = ViewAngle / stepCount;                    // Degrees per ray
        List<Vector3> viewPoints = new List<Vector3>();                 // Vertexes points used to draw mesh
        ViewCastInfo oldViewCast = new ViewCastInfo();                  // Previous cast, used to check if there is a collide difference to find edge

        // Cast rays
        for (int i = 0; i < stepCount; ++i)
        {
            float angle = transform.eulerAngles.y - ViewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            // Check if edge between old cast and new one
            if (i > 0)
            {
                //if (oldViewCast.Hit != newViewCast.Hit)
                bool tooFar = Mathf.Abs(oldViewCast.Distance - newViewCast.Distance) > EdgeDistanceThreshold;
                if (oldViewCast.Hit != newViewCast.Hit || (oldViewCast.Hit && newViewCast.Hit && tooFar))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                        viewPoints.Add(edge.pointA);
                    if (edge.pointB != Vector3.zero)
                        viewPoints.Add(edge.pointB);

                    if(drawGizmo)
                        Debug.DrawLine(transform.position, viewPoints.Last(), Color.yellow);
                }
            }

            viewPoints.Add(newViewCast.Point);
            oldViewCast = newViewCast;
            if(drawGizmo) Debug.DrawLine(transform.position, newViewCast.Point);
        }

        // Initialize mesh
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        // Create mesh data
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; ++i)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
    }

    /// <summary>
    /// Get a global direction vector from an angle in degrees
    /// </summary>
    /// <param name="angleInDegrees"></param>
    /// <param name="angleIsGlobal"></param>
    /// <returns></returns>
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(
            Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),
            0,
            Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)
        );
    }

    /// <summary>
    /// Find an approximation of the edge position between two different ray casts
    /// </summary>
    /// <param name="minViewCast"></param>
    /// <param name="maxViewCast"></param>
    /// <returns></returns>
    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.Angle;
        float maxAngle = maxViewCast.Angle;

        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        
        // At each iteration, sharp the position precision by 2
        for (int i = 0; i < EdgeResolveIterations; ++i)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool tooFar = Mathf.Abs(minViewCast.Distance - newViewCast.Distance) > EdgeDistanceThreshold;
            if (newViewCast.Hit == minViewCast.Hit && !tooFar)
            {
                minAngle = angle;
                minPoint = newViewCast.Point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.Point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    /// <summary>
    /// Create a structure with information about a ray cast
    /// </summary>
    /// <param name="globalAngle"></param>
    /// <returns></returns>
    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, ViewRadius, ObstacleMaskRender))
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);

        return new ViewCastInfo(false, transform.position + direction * ViewRadius, ViewRadius, globalAngle);
    }

    /// <summary>
    /// Used to give information about a ray cast
    /// </summary>
    public struct ViewCastInfo
    {
        public bool Hit;
        public Vector3 Point;
        public float Distance;
        public float Angle;

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            this.Hit = hit;
            this.Point = point;
            this.Distance = distance;
            this.Angle = angle;
        }
    }

    /// <summary>
    /// Used to give approximate position of an edge
    /// </summary>
    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }
}
