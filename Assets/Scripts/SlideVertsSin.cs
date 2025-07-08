using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Compression;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class SlideVertsSin : MonoBehaviour
{
    [SerializeField] Transform fromPosition;
    [SerializeField] float fullObjectScale = 1;
    [SerializeField] bool updateContinuously;

    private Vector3[] uniqueVert;
    private Mesh unmodifiedMesh;

    // Start is called before the first frame update
    void Start()
    {
        unmodifiedMesh = Instantiate(gameObject.GetComponent<MeshFilter>().mesh);
       
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
        
        ScaleFromViewSin(gameObject);
    }
    private void ScaleFromViewSin(GameObject gameObject)
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        Ray[] rays = new Ray[vertices.Length];
        float[] vertexDistance = new float[vertices.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            // Initialization 

            // Ray from fromPosition to the vertex 
            rays[i] = new Ray(fromPosition.position, ToOrtho.VertFromLocalToWorldSpace(gameObject, vertices[i]) - fromPosition.position);
            // Distance from fromPosition to the vertex
            vertexDistance[i] = Vector3.Distance(fromPosition.position, ToOrtho.VertFromLocalToWorldSpace(gameObject, vertices[i]));

            // Edit
            vertices[i] = ToOrtho.VertFromWorldToLocalSpace(gameObject, fullObjectScale * (vertexDistance[i] + (Mathf.Sin(vertexDistance[i])))* rays[i].direction + fromPosition.position );

            // Rays to the unmodified objects vertices 
            //Debug.DrawRay(rays[i].origin, rays[i].direction * vertexDistance[i], Color.red);
        }

        // Update the object's vertices to the modified ones
        mesh.vertices = vertices;
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(SlideVertsSin))]
class EditorSlideVertsSin : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SlideVertsSin sv = target as SlideVertsSin;

        if (GUILayout.Button("Update Mesh"))
        {
            sv.UpdateMesh();
        }
    }
}
#endif