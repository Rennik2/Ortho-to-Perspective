using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTransforms : MonoBehaviour
{
    [SerializeField] GameObject testObject; 
 
    // Update is called once per frame
    void Update()
    {
        Mesh mesh = testObject.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices_WS = VerticesFromLocalToWorldSpace(testObject, mesh.vertices);
        Debug.Log(vertices_WS[0]);
        mesh.vertices = VerticesFromWorldToLocalSpace(testObject, vertices_WS);
    }

    private Vector3 VertFromLocalToWorldSpace(GameObject gameObject, Vector3 vert)
    {
        Matrix4x4 transformationMatrix = gameObject.transform.localToWorldMatrix;
        // Might want to see if MultiplyPoint3x4 works (it would be faster)
        Vector3 vert_WS = transformationMatrix.MultiplyPoint(vert);
        return vert_WS;
    }
    private Vector3[] VerticesFromLocalToWorldSpace(GameObject gameObject, Vector3[] vertices)
    {
        Vector3[] vertices_WS = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices_WS[i] = VertFromLocalToWorldSpace(gameObject, vertices[i]);
        }
        return vertices_WS;
    }
    private Vector3 VertFromWorldToLocalSpace(GameObject gameObject, Vector3 vert)
    {
        Matrix4x4 transformationMatrix = gameObject.transform.worldToLocalMatrix;
        Vector3 vert_LS = transformationMatrix.MultiplyPoint(vert);
        return vert_LS;
    }
    private Vector3[] VerticesFromWorldToLocalSpace(GameObject gameObject, Vector3[] vertices)
    {
        Vector3[] vertices_LS = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices_LS[i] = VertFromWorldToLocalSpace(gameObject, vertices[i]);
        }
        return vertices_LS;
    }

}
