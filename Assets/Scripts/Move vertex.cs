using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class MoveVertex : MonoBehaviour
{
    [SerializeField] GameObject[] toGameObjects;
    [SerializeField] Transform fromPosition; 
    [SerializeField] float scale = 1;
    [SerializeField] bool callScript = false;
    [SerializeField] float throughPlaneDistance = 1;
    [SerializeField] String fileName = "mesh";

    bool wasCalled;
   
   private void Start() 
   {
        wasCalled = callScript;
   }

    private void Update() 
    {
        if (callScript != wasCalled)
        {
            // ScaleFromView(toGameObject);
            // MeshToObj.ObjectToObj(toGameObject, $"C:\\Users\\happy\\Downloads\\{fileName}.obj");

            foreach (GameObject gameObject in toGameObjects)
            {
                MeshFromPerspectiveToOrtho(gameObject);
            }

            wasCalled = callScript;
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
            rays[i] = new Ray(fromPosition.position, (gameObject.transform.position + vertices[i]) - fromPosition.position);
            // Distance from fromPosition to the vertex
            vertexDistance[i] = Vector3.Distance(fromPosition.position, gameObject.transform.position + vertices[i]);


            // Edit mesh 

            // Takes the direction and the distance to give a point in world space that is then converted to 
            // object space of the gameObject. This can then all be scaled by the scale not changing the perceived 
            // largeness from the view of fromPosition 
            vertices[i] = rays[i].direction * vertexDistance[i] * scale - gameObject.transform.position;

            // Rays to the unmodified objects vertices 
            Debug.DrawRay(fromPosition.position, rays[i].direction * vertexDistance[i] , Color.red, 5.0f);
        }

        // Update the object's vertices to the modified ones
        mesh.vertices = vertices;
    }

    private void MeshFromPerspectiveToOrtho(GameObject gameObject)
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        Ray[] rays = new Ray[vertices.Length];
        Plane cameraPlane = new Plane(fromPosition.forward, fromPosition.position);

        Vector3[] throughViewPoints = new Vector3[vertices.Length];
        float[] vertDistance= new float[vertices.Length];

        Ray[] perspectiveRays = new Ray[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            // Initialization 
            rays[i] = new Ray(vertices[i] + gameObject.transform.position, - fromPosition.forward);

            Vector3 planeIntersectionPoint = RayPlaneIntersectionPoint(rays[i], cameraPlane);

            vertDistance[i] = Vector3.Distance(planeIntersectionPoint, vertices[i] + gameObject.transform.position);
            throughViewPoints[i] = rays[i].GetPoint(throughPlaneDistance);

            // Editing
            perspectiveRays[i] = new Ray(fromPosition.position, throughViewPoints[i]);

            //vertices[i] = perspectiveRays[i].GetPoint(vertDistance[i]) + fromPosition.position;
            vertices[i] =  perspectiveRays[i].direction * vertDistance[i] - gameObject.transform.position;

            // Debug.DrawRay(perspectiveRays[i].origin, perspectiveRays[i].direction * vertDistance[i], Color.blue, 10f);
        }

        mesh.vertices = vertices;


        for (int i = 0; i < vertices.Length; i++)
        {
            Debug.DrawRay(perspectiveRays[i].origin, perspectiveRays[i].direction * vertDistance[i], Color.blue, 10f);

        }
    }

    // Return world space point where they intersect 
    private Vector3 RayPlaneIntersectionPoint(Ray line, Plane plane)
    {
        float distance = 0;

        if (!plane.Raycast(line, out distance))
            Debug.LogError("A ray that was supposed to be hitting the camera plane did not some how. You should multiply by -1 some where");
        
        Vector3 intersectionPoint = line.GetPoint(distance);

        return intersectionPoint;
    }

}
