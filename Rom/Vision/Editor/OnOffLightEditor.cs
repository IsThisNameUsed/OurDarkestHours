using System.Collections;
using System.Collections.Generic;
using ProBuilder2.EditorCommon;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OnOffLight))]
public class OnOffLightEditor : Editor
{
    private bool _onOffCollapse;
    private bool _flickeringCollapse;
    private bool _soundsCollapse;
    private bool _devCollapse;

    private float _lightIntensity;

    public override void OnInspectorGUI()
    {
        OnOffLight ool = target as OnOffLight;
        if (ool == null)
            return;

        base.OnInspectorGUI();
        EditorGUILayout.Space();
        return;
        
        /*EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Toggle("Is Toggled", ool.IsToggled);
        EditorGUI.EndDisabledGroup();
        
        if (Application.isPlaying && GUILayout.Button("Switch " + (ool.Toggled ? "Off" : "On")))
            ool.Toggle();

        _onOffCollapse = EditorGUILayout.Foldout(_onOffCollapse, "On and Off options");
        if (_onOffCollapse)
        {
            ool.TimeToOn = EditorGUILayout.FloatField("Time to switch on", ool.TimeToOn);
            ool.TimeToOff = EditorGUILayout.FloatField("Time to switch off", ool.TimeToOff);
            ool.BooleanValues = EditorGUILayout.Toggle("Boolean values", ool.BooleanValues);

            ool.OnAnimation = EditorGUILayout.CurveField("Switch On Animation", ool.OnAnimation);
            ool.OffAnimation = EditorGUILayout.CurveField("Switch Off Animation", ool.OffAnimation);
        }

        _flickeringCollapse = EditorGUILayout.Foldout(_flickeringCollapse, "Flickering options");
        if (_flickeringCollapse)
        {
            ool.Flickering = EditorGUILayout.Toggle("Flickering", ool.Flickering);
            ool.RandomFlickering = EditorGUILayout.Toggle("Random flickering", ool.RandomFlickering);

            if (ool.RandomFlickering)
            {
                ool.FlickeringTimeRange = EditorGUILayout.Slider("Flickering Time Range", ool.FlickeringTimeRange, 0.1f, 1);
                ool.FlickeringMinimumTime = EditorGUILayout.Slider("Flickering Minimum Time", ool.FlickeringMinimumTime, 0, 1);
            }
            else
            {
                ool.FlickeringTime = EditorGUILayout.FloatField("Flickering curve time", ool.FlickeringTime);
                ool.FlickeringAnimation = EditorGUILayout.CurveField("Flickering animation", ool.FlickeringAnimation);
            }
        }

        _soundsCollapse = EditorGUILayout.Foldout(_soundsCollapse, "Sounds");
        if (_soundsCollapse)
        {
            ool.ToggleOnSound = EditorGUILayout.ObjectField("Toggle On Sound", ool.ToggleOnSound, typeof(AudioClip), false) as AudioClip;
            ool.ToggleOffSound = EditorGUILayout.ObjectField("Toggle Off Sound", ool.ToggleOffSound, typeof(AudioClip), false) as AudioClip;
            EditorGUILayout.Space();
            ool.FlickerOnSound = EditorGUILayout.ObjectField("Flicker On Sound", ool.FlickerOnSound, typeof(AudioClip), false) as AudioClip;
            ool.FlickerOffSound = EditorGUILayout.ObjectField("Flicker Off Sound", ool.FlickerOffSound, typeof(AudioClip), false) as AudioClip;
        }

        ool.Light = EditorGUILayout.ObjectField("Light Object", ool.Light, typeof(Light), true) as Light;
        
        ool.LightIntensityMultiplier = EditorGUILayout.FloatField("Light Intensity Multiplier", ool.LightIntensityMultiplier);
        EditorGUILayout.Space();

        _devCollapse = EditorGUILayout.Foldout(_devCollapse, "Developers zone");
        if (_devCollapse)
        {
            ool.CoroutinesRate = EditorGUILayout.IntField("Coroutines Rate", ool.CoroutinesRate);

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target as OnOffLight), typeof(OnOffLight), false);
            GUI.enabled = true;
        }*/
    }

    public void OnSelectionChange()
    {
        Debug.Log("hey");
    }
}
