using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(TriggerZone))]
public class TriggerZoneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TriggerZone trigger = target as TriggerZone;

        if (trigger.Collider == null)
            trigger.Collider = trigger.GetComponent<Collider>();

        if (trigger.Collider != null)
        {
            trigger.Collider.isTrigger = true;

            base.OnInspectorGUI();
            return;
        }

        if (GUILayout.Button("Box Collider"))
            trigger.gameObject.AddComponent<BoxCollider>();
        else if (GUILayout.Button("Sphere Collider"))
            trigger.gameObject.AddComponent<SphereCollider>();
    }
}
