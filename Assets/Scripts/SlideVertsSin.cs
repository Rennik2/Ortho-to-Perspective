using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SlideVertsSin : MonoBehaviour
{
    [SerializeField] Transform fromPosition;
    [SerializeField] float fullObjectScale = 1;
    [SerializeField] bool updateContinuously;
    [SerializeField] float number;

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

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            // Ray from fromPosition to the vertex 
            Ray ray = new Ray(fromPosition.position, ToOrtho.VertFromLocalToWorldSpace(gameObject, vertices[i]) - fromPosition.position);
            // Distance from fromPosition to the vertex
            float vertexDistance = Vector3.Distance(fromPosition.position, ToOrtho.VertFromLocalToWorldSpace(gameObject, vertices[i]));

            //vertexDistance -= (Mathf.Sin(ToOrtho.VertFromLocalToWorldSpace(gameObject, vertices[i]).y) + 1) * (Mathf.Sin(ToOrtho.VertFromLocalToWorldSpace(gameObject, vertices[i]).z) + 1);

            vertexDistance += Mathf.Sin(ToOrtho.VertFromLocalToWorldSpace(gameObject, vertices[i]).z * 2)/number;
            vertexDistance *= fullObjectScale;

            // Edit
            //vertices[i] = ToOrtho.VertFromWorldToLocalSpace(gameObject, fullObjectScale * (vertexDistance[i] + Mathf.Sin(vertexDistance[i])) * ray.direction + fromPosition.position);
            vertices[i] = ToOrtho.VertFromWorldToLocalSpace(gameObject, vertexDistance * ray.direction + fromPosition.position);

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