using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

// #if UNITY_EDITOR
// using UnityEditor;
// #endif

public class ToOrthoV2 : MonoBehaviour
{
    [SerializeField] Transform fromPosition; 
    [SerializeField] float objScale = 1;
    [SerializeField] float throughPlaneDistance = 10;
    private Mesh unmodifiedMesh;
    private float lastObjScale = -1;
    private float lastThroughPlaneDistance = -1;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 lastScale;

    private void Start()
    {
        unmodifiedMesh = gameObject.GetComponent<MeshFilter>().mesh;

        if (fromPosition == null)
        {
            fromPosition = Camera.main.transform;
        }
    }

    private void Update() 
    {
        if (objScale != lastObjScale || throughPlaneDistance != lastThroughPlaneDistance || !SamePositionRotateScale())
        {
            gameObject.GetComponent<MeshFilter>().mesh = Instantiate(unmodifiedMesh);

            MeshFromPerspectiveToOrtho(gameObject);

            if (objScale != 1 && !gameObject.GetComponent<SlideVerts>())
            {
                ScaleFromView(gameObject);
            }

            lastThroughPlaneDistance = throughPlaneDistance;
            lastObjScale = objScale;
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            lastScale = transform.localScale;
        }
    }
    
    private void MeshFromPerspectiveToOrtho(GameObject gameObject)
    {
        Plane cameraPlane = new Plane(fromPosition.forward, fromPosition.position);

        Vector3[] vertices = gameObject.GetComponent<MeshFilter>().mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            // Initialization 
            Vector3 worldSpaceVert = VertFromLocalToWorldSpace(gameObject, vertices[i]);
            Ray ray = new Ray(worldSpaceVert, -fromPosition.forward);

            Vector3 planeIntersectionPoint = RayPlaneIntersectionPoint(ray, cameraPlane);
            ray = new Ray(planeIntersectionPoint, -ray.direction);
            float vertexDistances = Vector3.Distance(planeIntersectionPoint, worldSpaceVert);

            // Editing 
            Vector3 throughViewPoints = ray.GetPoint(throughPlaneDistance);
            Ray perspectiveRay = new Ray(fromPosition.position, throughViewPoints);

            vertices[i] = VertFromWorldToLocalSpace(gameObject, perspectiveRay.GetPoint(vertexDistances));

            // Debugging
            //Debug.DrawRay(perspectiveRays[i].origin, perspectiveRays[i].direction * vertexDistances[i], Color.blue);
        }

        gameObject.GetComponent<MeshFilter>().mesh.vertices = vertices;
    }

    // Return world space point where they intersect 
    public static Vector3 RayPlaneIntersectionPoint(Ray line, Plane plane)
    {
        float distance = 0;

        if (!plane.Raycast(line, out distance))
            Debug.LogError("A ray that was supposed to be hitting the camera plane did not some how. You should multiply by -1 some where");
        
        Vector3 intersectionPoint = line.GetPoint(distance);

        return intersectionPoint;
    }

    private void ScaleFromView(GameObject gameObject)
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;


        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            // Ray from fromPosition to the vertex 
            Ray ray = new Ray(fromPosition.position, ToOrtho.VertFromLocalToWorldSpace(gameObject, vertices[i]) - fromPosition.position);
            // Distance from fromPosition to the vertex
            float vertexDistance = Vector3.Distance(fromPosition.position, ToOrtho.VertFromLocalToWorldSpace(gameObject, vertices[i]));

            // Edit
            vertices[i] = ToOrtho.VertFromWorldToLocalSpace(gameObject, objScale * vertexDistance * ray.direction + fromPosition.position);

            // Rays to the unmodified objects vertices 
            //Debug.DrawRay(rays[i].origin, rays[i].direction * vertexDistance[i], Color.red);
        }
        mesh.vertices = vertices;
    }

    private bool SamePositionRotateScale()
    {
        if (transform.position != lastPosition)
            return false;
        if (transform.rotation != lastRotation)
            return false;
        if (transform.localScale != lastScale)
            return false;
        return true;
    }
    
    public static Vector3 VertFromLocalToWorldSpace(GameObject gameObject, Vector3 vert)
    {
        Matrix4x4 transformationMatrix = gameObject.transform.localToWorldMatrix;
        // Might want to see if MultiplyPoint3x4 works (it would be faster)
        Vector3 vert_WS = transformationMatrix.MultiplyPoint(vert);
        return vert_WS;
    }
    public static Vector3[] VerticesFromLocalToWorldSpace(GameObject gameObject, Vector3[] vertices)
    {
        Vector3[] vertices_WS = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices_WS[i] = VertFromLocalToWorldSpace(gameObject, vertices[i]);
        }
        return vertices_WS;
    }
    public static Vector3 VertFromWorldToLocalSpace(GameObject gameObject, Vector3 vert)
    {
        Matrix4x4 transformationMatrix = gameObject.transform.worldToLocalMatrix;
        Vector3 vert_LS = transformationMatrix.MultiplyPoint(vert);
        return vert_LS;
    }
    public static Vector3[] VerticesFromWorldToLocalSpace(GameObject gameObject, Vector3[] vertices)
    {
        Vector3[] vertices_LS = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices_LS[i] = VertFromWorldToLocalSpace(gameObject, vertices[i]);
        }
        return vertices_LS;
    }
}
