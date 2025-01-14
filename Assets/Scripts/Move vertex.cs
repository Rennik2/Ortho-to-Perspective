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
            MoveVerities();
            wasCalled = callScript;
        }
    }
    
    private void MoveVerities()
    {
        Mesh mesh = toGameObject.GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;

        Ray[] rays = new Ray[vertices.Length];
        float[] vertexDistance = new float[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            
            // Ray from fromPosition to the vertex 
            rays[i] = new Ray(fromPosition.position, (toGameObject.transform.position + vertices[i]) - fromPosition.position);
            // Distance from fromPosition to the vertex
            vertexDistance[i] = Vector3.Distance(fromPosition.position, toGameObject.transform.position + vertices[i]);


            vertices[i] = rays[i].direction * vertexDistance[i] * 1.2f;


            Debug.DrawRay(rays[i].origin, rays[i].direction * vertexDistance[i], Color.red);
        }

        mesh.vertices = vertices;
    }
}
