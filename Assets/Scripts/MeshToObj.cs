using UnityEngine;
using System.IO;
using System;
using static MoveVertex;

public static class MeshToObj 
{

    public static void ObjectToObj(GameObject gameObject, string path, string name)
    {
        if (gameObject == null || gameObject.GetComponent<MeshFilter>() == null || gameObject.GetComponent<MeshFilter>().mesh == null)
        {
            Debug.LogError("GameObject is null or doesn't have MeshFilter component or doesn't have a mesh");
            return; 
        }


        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;

        mesh.Optimize();

        using (StreamWriter writer = new StreamWriter($"{path}\\{name}.obj"))
        {
            // Write object name
            writer.WriteLine($"o " + gameObject.name);

            // Write vertices 
            foreach (Vector3 vert in mesh.vertices)
            {
                Vector3 vertWorldPos = VertFromLocalToWorldSpace(gameObject, vert);
                writer.WriteLine($"v { - vertWorldPos.x} {vertWorldPos.y} {vertWorldPos.z}");
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
        ObjectToObj(gameObject, path, "mesh");
    }


    public static void ObjectsToObj(GameObject[] gameObjects, String path, String name)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            ObjectToObj(gameObjects[i], path, $"{name}{i}");
        }
    }
    public static void ObjectsToObj(GameObject[] gameObject)
    {
        string path = "C:\\Users\\happy\\Downloads";
        ObjectsToObj(gameObject, path, "mesh");
    }
}
