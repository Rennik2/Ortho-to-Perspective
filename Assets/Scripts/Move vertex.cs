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

            vertices[i] = scale * rays[i].direction * vertexDistance[i] - gameObject.transform.position ;

            Debug.DrawRay(fromPosition.position, rays[i].direction * vertexDistance[i] , Color.red);
        }

        mesh.vertices = vertices;
    }


}
