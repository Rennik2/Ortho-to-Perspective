using System;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class SlideVerts : MonoBehaviour
{
    [SerializeField] Transform fromPosition;
    [SerializeField] float fullObjectScale = 1;
    [SerializeField][Range(.3f, 3)] float[] uniqueVertMultiplier;
    [SerializeField] bool updateContinuously;

    private Vector3[] uniqueVert;
    private Mesh unmodifiedMesh;

    // Start is called before the first frame update
    void Start()
    {
        unmodifiedMesh = Instantiate(gameObject.GetComponent<MeshFilter>().mesh);
        uniqueVert = GetNumberUniqueVerts(gameObject);
        uniqueVertMultiplier = new float[uniqueVert.Length];

        for (int i = 0; i < uniqueVertMultiplier.Length; i++)
        {
            uniqueVertMultiplier[i] = 1;
        }
        if (fromPosition == null)
        {
            fromPosition = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
         if (updateContinuously)
        {
            UpdateMesh();
        }
    }

    public void UpdateMesh()
    {
        if (!gameObject.GetComponent<ToOrthoV2>())
        {
            gameObject.GetComponent<MeshFilter>().mesh = Instantiate(unmodifiedMesh);
        }
        
        ScaleFromViewVert(gameObject);
    }

    private void ScaleFromViewVert(GameObject gameObject)
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;

        Ray[] rays = new Ray[verts.Length];
        float[] vertexDistance = new float[verts.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float distanceMultiplier = 1;

            for (int j = 0; j < uniqueVert.Length; j++)
            {
                if (verts[i] == uniqueVert[j])
                {
                    distanceMultiplier = Mathf.Sqrt(uniqueVertMultiplier[j]);
                }
            }
            // Ray from fromPosition to the vertex 
            rays[i] = new Ray(fromPosition.position, ToOrtho.VertFromLocalToWorldSpace(gameObject, verts[i]) - fromPosition.position);
            // Distance from fromPosition to the vertex
            vertexDistance[i] = Vector3.Distance(fromPosition.position, ToOrtho.VertFromLocalToWorldSpace(gameObject, verts[i]));

            verts[i] = ToOrtho.VertFromWorldToLocalSpace(gameObject, fullObjectScale * distanceMultiplier * vertexDistance[i] * rays[i].direction + fromPosition.position);

        }
        mesh.vertices = verts;
    }

    // private void ScaleFromView(GameObject gameObject)
    // {
    //     Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
    //     Vector3[] vertices = mesh.vertices;

    //     Ray[] rays = new Ray[vertices.Length];
    //     float[] vertexDistance = new float[vertices.Length];

    //     for (int i = 0; i < mesh.vertices.Length; i++)
    //     {
    //         // Initialization 

    //         // Ray from fromPosition to the vertex 
    //         rays[i] = new Ray(fromPosition.position, ToOrtho.VertFromLocalToWorldSpace(gameObject, vertices[i]) - fromPosition.position);
    //         // Distance from fromPosition to the vertex
    //         vertexDistance[i] = Vector3.Distance(fromPosition.position, ToOrtho.VertFromLocalToWorldSpace(gameObject, vertices[i]));

    //         // Edit
    //         vertices[i] = ToOrtho.VertFromWorldToLocalSpace(gameObject, fullObjectScale * vertexDistance[i] * rays[i].direction + fromPosition.position);

    //         // Rays to the unmodified objects vertices 
    //         //Debug.DrawRay(rays[i].origin, rays[i].direction * vertexDistance[i], Color.red);
    //     }

    //     // Update the object's vertices to the modified ones
    //     mesh.vertices = vertices;
    // }

    private Vector3[] GetNumberUniqueVerts(GameObject gameObject)
    {
        Vector3[] verts = gameObject.GetComponent<MeshFilter>().mesh.vertices;
        List<Vector3> uniqueVerts = new();

        for (int i = 0; i < verts.Length; ++i)
        {
            if (!uniqueVerts.Contains(verts[i]))
            {
                uniqueVerts.Add(verts[i]);
            }
        }
        return uniqueVerts.ToArray();
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(SlideVerts))]
class EditorSlideVerts : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SlideVerts sv = target as SlideVerts;

        if (GUILayout.Button("Update Mesh"))
        {
            sv.UpdateMesh();
        }
    }
}
#endif