using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(ScriptableProfileData))]
public class MyDataScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ScriptableProfileData myDataScriptableObject = (ScriptableProfileData)target;

        if (GUILayout.Button("Load Data from JSON"))
        {
            string path = EditorUtility.OpenFilePanel("Select JSON file", "", "json");
            if (path.Length != 0)
            {
                string json = File.ReadAllText(path);
                myDataScriptableObject.data = JsonUtility.FromJson<NewDataManager.ProfileData>(json);
                EditorUtility.SetDirty(myDataScriptableObject); // Mark the object as dirty to save the changes
            }
        }
    }
}
