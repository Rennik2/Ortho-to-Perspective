using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MoveVertex : MonoBehaviour
{
    [SerializeField] GameObject toGameObject;
    [SerializeField] Transform fromPosition; 
    [SerializeField] float scale = 1;
    [SerializeField] bool callScript = false;
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
            MoveVerities(toGameObject);
            MeshToObj.ObjectToObj(toGameObject, $"C:\\Users\\happy\\Downloads\\{fileName}.obj");
            wasCalled = callScript;
        }
        //MoveVerities(toGameObject);
    }
    
    private void MoveVerities(GameObject gameObject)
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

    // private void cameraRays()
    // {
    //     Plane plane = new Plane(fromPosition.position, );
    // }


}
