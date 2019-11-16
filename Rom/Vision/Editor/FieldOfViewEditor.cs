using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine.Rendering;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        FieldOfView fov = target as FieldOfView;
    }

    void OnSceneGUI()
    {
        FieldOfView fow = target as FieldOfView;

        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.ViewRadius);

        Vector3 viewAngleMin = fow.DirFromAngle(-fow.ViewAngle / 2, false);
        Vector3 viewAngleMax = fow.DirFromAngle(+fow.ViewAngle / 2, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleMin * fow.ViewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleMax * fow.ViewRadius);

        foreach (Lightable visibleTarget in fow.VisibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.transform.position);
        }
    }
}
