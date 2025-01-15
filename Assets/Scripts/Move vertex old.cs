using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MoveVertexOld : MonoBehaviour
{
    [SerializeField] GameObject toGameObject;
    [SerializeField] Transform fromPosition; 
    [SerializeField] float scale;
    [SerializeField] bool callScript;

    bool wasCalled;

    private Mesh mesh;
    private Vector3[] vertices;
    private Ray[] rays;
    private float[] vertexDistance;

    private void Start() 
    {
        prepareMeshData();
    }    

    private void Update() 
    {
        // Debug.Log(vertices.Length + " " + rays.Length + " " + vertexDistance.Length);
        // if (callScript != wasCalled)
        // {
        //     MoveVerities();
        //     wasCalled = callScript;
        // }
        MoveVerities();
    }
    
    private void MoveVerities()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float distance = Vector3.Distance(fromPosition.position, toGameObject.transform.position + vertices[i]);

            vertices[i] = rays[i].direction * vertexDistance[i];

            Debug.DrawRay(rays[i].origin, rays[i].direction * vertexDistance[i] * distance, Color.red);
        }

        mesh.vertices = vertices;
    }

    private void prepareMeshData()
    {
        mesh = toGameObject.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        rays = new Ray[vertices.Length];
        vertexDistance = new float[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            // Ray from fromPosition to the vertex 
            rays[i] = new Ray(fromPosition.position, (toGameObject.transform.position + vertices[i]) - fromPosition.position);
            // Distance from fromPosition to the vertex
            vertexDistance[i] = Vector3.Distance(fromPosition.position, toGameObject.transform.position + vertices[i]);
        }
    }
}
