using UnityEngine;
using System.IO;
using System;
using TMPro;

public static class MeshToObj 
{

    public static void ObjectToObj(GameObject gameObject, string pathAndName)
    {
        if (gameObject == null || gameObject.GetComponent<MeshFilter>() == null || gameObject.GetComponent<MeshFilter>().mesh == null)
        {
            Debug.LogError("Gameobject is null or doesn't have MeshFilter componet or doesn't have a mesh");
            return; 
        }


        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;

        using (StreamWriter writer = new StreamWriter(pathAndName))
        {
            // Write object name
            writer.WriteLine($"o " + gameObject.name);

            // Write vertices 
            foreach (Vector3 vert in mesh.vertices)
            {
                writer.WriteLine($"v {vert.x} {vert.y} {vert.z}");
            }
            // Write normals 
            foreach (Vector3 vertNormal in mesh.normals)
            {
                writer.WriteLine($"vn {vertNormal.x} {vertNormal.y} {vertNormal.z}");
            }
            // Write uvs
            foreach (Vector2 uv in mesh.uv)
            {
                writer.WriteLine($"vt {uv.x} {uv.y}");
            }
            // Write
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                writer.WriteLine($"f {mesh.triangles[i] + 1} {mesh.triangles[i + 1] + 1} {mesh.triangles[i + 2] + 1}");
            }
        }

    }
    
    public static void ObjectToObj(GameObject gameObject)
    {
        string path = "C:\\Users\\happy\\Downloads";
        ObjectToObj(gameObject, path + "\\mesh.obj");
    }
}
