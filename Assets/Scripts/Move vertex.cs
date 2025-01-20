using System;
using UnityEngine;

using static MeshToObj;

public class MoveVertex : MonoBehaviour
{
    [SerializeField] GameObject[] toGameObjects;
    [SerializeField] Transform fromPosition; 
    [SerializeField] float scale = 1;
    [SerializeField] bool orthographic = false;
    [SerializeField] float throughPlaneDistance = 1;
    [SerializeField] bool saveMesh = false;
    [SerializeField] String fileName = "mesh";

    private Mesh[] unmodifiedMeshes;
    private bool wasSaved;

    private void Start() 
   {
        wasSaved = saveMesh;

        unmodifiedMeshes = new Mesh[toGameObjects.Length];

        for (int i = 0; i < toGameObjects.Length; i++)
        {
            if (toGameObjects[i].GetComponent<MeshFilter>()== null)
                Debug.LogError($"no mesh filter at {i}");

            unmodifiedMeshes[i] = Instantiate(toGameObjects[i].GetComponent<MeshFilter>().mesh);
        }
   }

    private void Update() 
    {
        // Reset all meshes to unmodified state os updates can be in real time and things don't go flying off into the distance
        for (int i = 0; i < toGameObjects.Length; i++)
        {
            toGameObjects[i].GetComponent<MeshFilter>().mesh = Instantiate(unmodifiedMeshes[i]);
        }

        foreach (GameObject gameObject in toGameObjects)
        {
            if (orthographic)
            {
                MeshFromPerspectiveToOrtho1(gameObject);
            }
            ScaleFromView(gameObject);
        }

        if (saveMesh != wasSaved)
        {
            ObjectsToObj(toGameObjects);
        }
    }
    
    
    private void ScaleFromView(GameObject gameObject)
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        Ray[] rays = new Ray[vertices.Length];
        float[] vertexDistance = new float[vertices.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            // Initialization 

            // Ray from fromPosition to the vertex 
            rays[i] = new Ray(fromPosition.position, VertFromLocalToWorldSpace(gameObject, vertices[i]) - fromPosition.position);
            // Distance from fromPosition to the vertex
            vertexDistance[i] = Vector3.Distance(fromPosition.position, VertFromLocalToWorldSpace(gameObject, vertices[i]));

            // Edit
            vertices[i] = VertFromWorldToLocalSpace(gameObject, rays[i].direction * vertexDistance[i] * scale + fromPosition.position);

            // Rays to the unmodified objects vertices 
            Debug.DrawRay(rays[i].origin, rays[i].direction * vertexDistance[i], Color.red);
        }

        // Update the object's vertices to the modified ones
        mesh.vertices = vertices;
    }
    
    private void MeshFromPerspectiveToOrtho(GameObject gameObject)
    {
        Plane cameraPlane = new Plane(fromPosition.forward, fromPosition.position);

        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;
        Ray[] rays = new Ray[vertices.Length];

        Vector3[] throughViewPoints = new Vector3[vertices.Length];
        float[] vertexDistances = new float[vertices.Length];
        Vector3[] planeIntersectionPoint = new Vector3[vertices.Length];
        Ray[] perspectiveRays = new Ray[vertices.Length];
        

        for (int i = 0; i < vertices.Length; i++)
        {
            // Initialization 
            Vector3 worldSpaceVert = VertFromLocalToWorldSpace(gameObject, vertices[i]);
            rays[i] = new Ray(worldSpaceVert, - fromPosition.forward);

            planeIntersectionPoint[i] = RayPlaneIntersectionPoint(rays[i], cameraPlane);
            rays[i] = new Ray(planeIntersectionPoint[i], - rays[i].direction);

            vertexDistances[i] = Vector3.Distance(planeIntersectionPoint[i], worldSpaceVert);
            throughViewPoints[i] = rays[i].GetPoint(throughPlaneDistance);

            // Editing
            perspectiveRays[i] = new Ray(fromPosition.position, throughViewPoints[i]);
            vertices[i] =  VertFromWorldToLocalSpace(gameObject, perspectiveRays[i].direction * vertexDistances[i]);

            // Debugging
            Debug.DrawRay(planeIntersectionPoint[i], throughViewPoints[i] * vertexDistances[i], Color.blue);
        }

        mesh.vertices = vertices;
    }

    private void MeshFromPerspectiveToOrtho1(GameObject gameObject)
    {
        Plane cameraPlane = new Plane(fromPosition.forward, fromPosition.position);

        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;
        Ray[] rays = new Ray[vertices.Length];

        Vector3[] throughViewPoints = new Vector3[vertices.Length];
        float[] vertexDistances = new float[vertices.Length];
        Vector3[] planeIntersectionPoint = new Vector3[vertices.Length];
        Ray[] perspectiveRays = new Ray[vertices.Length];
        

        for (int i = 0; i < vertices.Length; i++)
        {
            // Initialization 
            Vector3 worldSpaceVert = VertFromLocalToWorldSpace(gameObject, vertices[i]);
            rays[i] = new Ray(worldSpaceVert, - fromPosition.forward);

            planeIntersectionPoint[i] = RayPlaneIntersectionPoint(rays[i], cameraPlane);
            rays[i] = new Ray(planeIntersectionPoint[i], - rays[i].direction);
            vertexDistances[i] = Vector3.Distance(planeIntersectionPoint[i], worldSpaceVert);

            // Editing 
            throughViewPoints[i] = rays[i].GetPoint(throughPlaneDistance);
            perspectiveRays[i] = new Ray(fromPosition.position, throughViewPoints[i]);

            vertices[i] = VertFromWorldToLocalSpace(gameObject, perspectiveRays[i].GetPoint(vertexDistances[i]));

            // Debugging
            Debug.DrawRay(perspectiveRays[i].origin, perspectiveRays[i].direction * vertexDistances[i], Color.blue);
        }

        mesh.vertices = vertices;
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
