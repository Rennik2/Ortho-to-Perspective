using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
* Use to save the distorted meshes in play mode to your computer as obj files.
*
* instructions:
* 
* - add to the object you wish to save or add all the object you wish to save to the GameObject array
* - enter play mode
* - distort the mesh
* - press on of the save buttons

**/

public class SaveObject : MonoBehaviour
{
    // The path where the meshes will be saved on your computer
    [SerializeField] String path = "";

    // The name of the saved files
    [SerializeField] String fileName = "mesh";
    [SerializeField] GameObject[] gameObjects;

    // saves all the objects in gameObjects
    public void SaveObjects()
    {
        MeshToObj.ObjectsToObj(gameObjects, path, fileName);
    }
    
    // only saves the game object that this script is attached to.
    public void SaveThis()
    {
        MeshToObj.ObjectToObj(gameObject, path, fileName);
    }
}

// buttons in the inspector
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
        if (GUILayout.Button("Save THIS Object "))
        {
            so.SaveThis();
        }

    }
}
#endif