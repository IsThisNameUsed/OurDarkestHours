using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class OnOffLight : MonoBehaviour
{
    [Header("This script is used to control the toggling of the light attached to it")]
    [ReadOnly]
    public bool Toggled;

    // Timers to toggle on and off light
    public float TimeToOn = 1;
    public float TimeToOff = 1;

    // Animation curve for on animations
    [Tooltip("If true, >0.5 = 1 and <0.5 = 0")]
    public bool BooleanValues = true;
    public AnimationCurve OnAnimation = AnimationCurve.Constant(0, 1, 1);
    public AnimationCurve OffAnimation = AnimationCurve.Constant(0, 1, 0);

    // Flickering management
    public FlickerType FlickeringType;
    //public bool Flickering;
    public float FlickeringTime = 0.4f;
    //public bool RandomFlickering;
    [Range(0.1f, 1)] public float FlickeringTimeRange = 0.4f;
    [Range(0, 1)] public float FlickeringMinimumTime = 0.8f;
    public float StroboscopeRate = 10f;
    private float _nextFlick;
    public AnimationCurve FlickeringAnimation = AnimationCurve.Constant(0, 1, 0.5f);

    // Feedback sounds
    public AudioClip FlickerOnSound;
    public AudioClip FlickerOffSound;

    // Sounds components
    private AudioSource _flickerAudio;
    
    public float LightIntensityMultiplier;
    public Light[] Lights;

    [Space]
    [Header("For developpers")]
    public int CoroutinesRate = 20;

    private FieldOfView _fov;
    private float _oldRadius;

    private float _currentLightLevel;

    private Coroutine _toggleCoroutine;

    // Public event for other scripts to detect state changing
    public delegate void OnToggleDelegate(bool toggled);
    public event OnToggleDelegate OnToggle;

    private void Awake()
    {
        _fov = GetComponent<FieldOfView>();
        Toggled = _fov.ViewRadius > 0.1f;
        _oldRadius = _fov.ViewRadius;

        /*if (Light == null)
            Light = GetComponent<Light>();*/
        _flickerAudio = gameObject.AddComponent<AudioSource>();
        _flickerAudio.spatialBlend = 1;
        _flickerAudio.rolloffMode = AudioRolloffMode.Linear;
        _flickerAudio.maxDistance = 50f;
        _flickerAudio.spread = 60f;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateCoroutine());
    }

    public IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            if (FlickeringType != FlickerType.Off && Toggled)
            {
                float time = Time.time;
                if (FlickeringType == FlickerType.RandomFlicker && time >= _nextFlick)
                {
                    SetLight(_currentLightLevel < 0.1f, _currentLightLevel > 0.1f ? 0 : 1);
                    if (_currentLightLevel < 0.1f)
                    {
                        _nextFlick = time + Random.value * FlickeringTimeRange;
                        PlayFlickerSound(true);
                    }
                    else
                    {
                        _nextFlick = time + Random.value * FlickeringTimeRange + FlickeringMinimumTime;
                        PlayFlickerSound(false);
                    }
                }
                else if (FlickeringType == FlickerType.ScriptedFlicker)
                {
                    float progress = Time.time % FlickeringTime;
                    progress = Mathf.InverseLerp(0, FlickeringTime, progress);
                    float value = GetValueAt(FlickeringAnimation, progress);
                    SetLight(value > 0.1f, value);
                    PlayFlickerSound(value > 0.1f);
                }
                else if (FlickeringType == FlickerType.STROBOSCOPE && time >= _nextFlick)
                {
                    SetLight(_currentLightLevel < 0.1f, _currentLightLevel > 0.1f ? 0 : 1);
                    _nextFlick = time + (1f / StroboscopeRate);
                    PlayFlickerSound(_currentLightLevel > 0.1f);
                }
            }

            yield return new WaitForSeconds(1f / CoroutinesRate);
        }
    }

    /// <summary>
    /// Main function to toggle light
    /// </summary>
    public void Toggle()
    {
        if (Toggled)
            ToggleOff();
        else
        {
            AkSoundEngine.PostEvent("play_env_lamp_on", this.gameObject);
            ToggleOn();
        }
            
    }

    /// <summary>
    /// Main function to toggle on the light
    /// </summary>
    public void ToggleOn()
    {
        if (Toggled)
            return;
        _fov.ViewRadius = _oldRadius;
        //Debug.Log("Setting view radius to " + _oldRadius);
        Toggled = true;

        if (_toggleCoroutine != null)
            StopCoroutine(_toggleCoroutine);
        _toggleCoroutine = StartCoroutine(ToggleCoroutine(OnAnimation, TimeToOn));

        if (OnToggle != null)
            OnToggle(true);
    }

    /// <summary>
    /// Main function to toggle off the light
    /// </summary>
    /// <param name="sound"></param>
    public void ToggleOff(bool sound = true)
    {
        if (!Toggled)
            return;
        //_oldAngle = _fov.ViewAngle;
        //_oldRadius = _fov.ViewRadius;

        _fov.ViewRadius = 0;
        //Debug.Log("Setting view radius to " + 0);
        Toggled = false;
        AkSoundEngine.PostEvent("play_env_lamp_off", this.gameObject);

        if (_toggleCoroutine != null)
            StopCoroutine(_toggleCoroutine);
        _toggleCoroutine = StartCoroutine(ToggleCoroutine(OffAnimation, TimeToOff));

        if (OnToggle != null)
            OnToggle(false);
    }
    
    public void Bug(float duration)
    {
        StartCoroutine(IBug(duration));
    }

    private IEnumerator IBug(float duration)
    {
        FlickerType before = FlickeringType;
        FlickeringType = FlickerType.RandomFlicker;
        yield return new WaitForSeconds(duration);
        FlickeringType = before;
    }

    private IEnumerator ToggleCoroutine(AnimationCurve curve, float time)
    {
        float wait = 1f / CoroutinesRate;
        float startTime = Time.time;
        float endTime = startTime + time;
        float currentTime = startTime;

        while (currentTime < endTime)
        {
            float percentage = Mathf.InverseLerp(startTime, endTime, currentTime);
            float value = GetValueAt(curve, percentage);
            SetLight(value > .1f, value);

            yield return new WaitForSeconds(wait);
            currentTime += wait;
        }
    }

    private float GetValueAt(AnimationCurve curve, float time)
    {
        float value = curve.Evaluate(time);
        if (BooleanValues)
            value = value > 0.5f ? 1 : 0;

        return value;
    }
    
    /// <summary>
    /// Manually change light level, careful, does not logically change light state
    /// </summary>
    /// <param name="enlighten"></param>
    /// <param name="value"></param>
    public void SetLight(bool enlighten, float value)
    {
        _currentLightLevel = value;

        foreach(Light light in Lights)
            light.intensity = _currentLightLevel * LightIntensityMultiplier;

        /*if (Light != null)
            Light.intensity = _currentLightLevel * LightIntensityMultiplier;*/
        
        if (enlighten && _fov.ViewRadius != _oldRadius)
        {
            _fov.ViewRadius = _oldRadius;
            //Debug.Log("Setting view radius to " + _oldRadius);
        }
        else if (!enlighten && _fov.ViewRadius != 0)
        {
            _fov.ViewRadius = 0;
            //Debug.Log("Setting view radius to " + 0);
        }
    }

    private void PlayFlickerSound(bool on)
    {
        _flickerAudio.clip = (on ? FlickerOnSound : FlickerOffSound);
        _flickerAudio.Play();
    }

    public enum FlickerType
    {
        Off,
        ScriptedFlicker,
        RandomFlicker,
        STROBOSCOPE,
        IntensityVariation
    }
}
