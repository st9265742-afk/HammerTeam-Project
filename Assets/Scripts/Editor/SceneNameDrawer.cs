using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        List<string> names = new List<string>();

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                names.Add(Path.GetFileNameWithoutExtension(scene.path));
            }
        }

        if (names.Count == 0)
        {
            EditorGUI.LabelField(position, label.text, "No scenes in Build Profiles");
            return;
        }

        int index = Mathf.Max(0, names.IndexOf(property.stringValue));
        index = EditorGUI.Popup(position, label.text, index, names.ToArray());
        property.stringValue = names[index];
    }
}