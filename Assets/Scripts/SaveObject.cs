using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveObject : MonoBehaviour
{
    [SerializeField] String path = "C:\\Users\\happy\\Downloads";
    [SerializeField] String fileName = "mesh";
   [SerializeField] GameObject[] gameObjects;

    public void SaveObjects()
    {
        MeshToObj.ObjectsToObj(gameObjects, path, fileName); 
    }
    public void SaveThis()
    {
        MeshToObj.ObjectToObj(gameObject, path, fileName);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(SaveObject))]
class EditorSaveObject : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SaveObject so = target as SaveObject;

        if (GUILayout.Button("Save Objects"))
        {
            so.SaveObjects();
        }
        if (GUILayout.Button("Save This Object "))
        {
            so.SaveThis();
        }

    }
}
#endif