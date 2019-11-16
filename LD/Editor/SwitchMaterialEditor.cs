using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SwitchMaterial))][CanEditMultipleObjects]
public class SwitchMaterialEditor : Editor
{
    private SerializedProperty _mat1;
    private SerializedProperty _mat2;

    void OnEnable()
    {
        _mat1 = serializedObject.FindProperty("Material1");
        _mat2 = serializedObject.FindProperty("Material2");
    }

    public override void OnInspectorGUI()
    {
        SwitchMaterial sm = serializedObject.targetObject as SwitchMaterial;

        EditorGUILayout.PropertyField(_mat1, new GUIContent("Material 1"));
        if (sm.Material1 == null && GUILayout.Button("Set material 1 as current material"))
        {
            foreach (Object targetObject in serializedObject.targetObjects)
            {
                sm = targetObject as SwitchMaterial;
                if (sm.Material1 == null)
                    sm.Material1 = sm.Renderer.sharedMaterial;
            }
        }

        EditorGUILayout.PropertyField(_mat2, new GUIContent("Material 2"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Set material :", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Material 1"))
        {
            foreach (Object targetObject in serializedObject.targetObjects)
            {
                sm = targetObject as SwitchMaterial;
                sm.SetMaterial(SwitchMaterial.Type.First);
            }
        }

        if (GUILayout.Button("Material 2"))
        {
            foreach (Object targetObject in serializedObject.targetObjects)
            {
                sm = targetObject as SwitchMaterial;
                sm.SetMaterial(SwitchMaterial.Type.Second);
            }
        }

        GUILayout.EndHorizontal();


        EditorGUILayout.LabelField("Change all materials with this script :", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();

        sm = serializedObject.targetObject as SwitchMaterial;
        if (GUILayout.Button("Material 1"))
            sm.SetAll(SwitchMaterial.Type.First);
        if (GUILayout.Button("Material 2"))
            sm.SetAll(SwitchMaterial.Type.Second);

        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
