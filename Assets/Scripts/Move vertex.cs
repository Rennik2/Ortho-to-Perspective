using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MoveVertex : MonoBehaviour
{
    [SerializeField] GameObject toGameObject;
    [SerializeField] Transform fromPosition; 
    [SerializeField] float scale;
    [SerializeField] bool callScript;

    bool wasCalled;
   

    private void Update() 
    {
        if (callScript != wasCalled)
        {
            MoveVerities(toGameObject);
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
            // Initialisation 

            // Ray from fromPosition to the vertex 
            rays[i] = new Ray(fromPosition.position, (gameObject.transform.position + vertices[i]) - fromPosition.position);
            // Distance from fromPosition to the vertex
            vertexDistance[i] = Vector3.Distance(fromPosition.position, gameObject.transform.position + vertices[i]);


            // Edit mesh 

            // Takes the direction and the distance to give a point in world space that is then conveted to 
            // object space of the gameObject. This can then all be scaled by the scale not changeing the pecived 
            // largness from the view of fromPosition 
            vertices[i] = scale * rays[i].direction * vertexDistance[i] - gameObject.transform.position ;

            // Rays to the unmodivied objects vertecies 
            Debug.DrawRay(fromPosition.position, rays[i].direction * vertexDistance[i] , Color.red, 5.0f);
        }

        // Update the object's vertices to the modified ones
        mesh.vertices = vertices;
    }


}
