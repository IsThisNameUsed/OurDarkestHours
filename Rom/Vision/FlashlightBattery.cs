using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(OnOffLight))]
public class FlashlightBattery : MonoBehaviour
{
    [Tooltip("Max battery value")] public float MaxBattery = 90;
    internal float CurrentBattery { get; private set; }

    [Tooltip("Time to regen after toggle off")] public float DelayBeforeRegen = 1;
    [Tooltip("Regeneration per second")] public float RegenSpeed = 1.2f;

    [Tooltip("Battery price to toggle on")] public float ToggleOnCost = 5;
    [Tooltip("Consumption when flashlight is lit")] public float NormalConsumptionPerSecond = 1;
    [Tooltip("Consumption when flashlight is boosted")] public float BoostedConsumptionPerSecond = 2;
    
    private float _regenStart;      // Timestamp when the battery will regenerate
    
    [Tooltip("On when flashlight is boosted")] [ReadOnly] public bool Boosted;
    internal bool IsLoading { get; private set; }
    
    public Color FullBatteryColor;
    public Color MinBatteryColor;

    public AudioClip NoBatterySound;

    [Tooltip("Current battery charge")] [ReadOnly] public float Percentage;
    [Tooltip("Current battery charge")] [ReadOnly] public Color CurrentBatteryColor;

    private Light _light;

    private FieldOfView _fov;       // Local FieldOfView reference, used to change radius and angle
    private float _lastRadius;      // Last value of radius to make it back after toggle off
    private float _lastAngle;       // Last value of angle to make it back after toggle off
    private Player _playerStatus;    // Component used to light the player // TODO : directly register in player status ? Once uml is implemented
    private OnOffLight _ool;        // Master component for light handling

    void Start ()
    {
        _playerStatus = GetComponent<Player>();
        _fov = GetComponent<FieldOfView>();

        _ool = GetComponent<OnOffLight>();
        _ool.OnToggle += (toggled) =>
        {
            if (toggled)
                ToggleOn();
            else
                ToggleOff();
        };

        CurrentBattery = MaxBattery;
        
        _lastRadius = _fov.ViewRadius;
        _lastAngle = _fov.ViewAngle;

        _light = GetComponentInChildren<Light>();
    }
	
	void Update ()
	{
	    Percentage = CurrentBattery / MaxBattery;
	    CurrentBatteryColor = Color.Lerp(MinBatteryColor, FullBatteryColor, Percentage);
	    if (_light != null)
	        _light.color = CurrentBatteryColor;

        // If flashlight is off, waited the regen cooldown and hasn't max battery
	    if (!_ool.Toggled && Time.time > _regenStart && CurrentBattery < MaxBattery)
	    {
	        IsLoading = true;
	        CurrentBattery += RegenSpeed * Time.deltaTime;
	        if (CurrentBattery > MaxBattery)
	            CurrentBattery = MaxBattery;
	    }

        // If the flashlight is lit, consume battery and test if it has to be shutdown
	    if (_ool.Toggled)
	    {
	        CurrentBattery -= (Boosted ? BoostedConsumptionPerSecond : NormalConsumptionPerSecond) * Time.deltaTime;
	        if (CurrentBattery <= 0)
	        {
	            CurrentBattery = 0;
                AkSoundEngine.PostEvent("play_flashlight_shut_off", this.gameObject);
                if (NoBatterySound != null)
	            {
	                _ool.ToggleOff(false);
	                AudioSource source = gameObject.AddComponent<AudioSource>();
	                source.clip = NoBatterySound;
                    source.Play();
                    Destroy(source, NoBatterySound.length);
	            }
                else
                    _ool.ToggleOff();
	        }
	    }
	}

    /// <summary>
    /// Toggle on the flashlight if it has enough battery
    /// </summary>
    public void ToggleOn()
    {
        if (CurrentBattery < ToggleOnCost)
            return;

        CurrentBattery -= ToggleOnCost;

        _fov.ViewRadius = _lastRadius;
        _fov.ViewAngle = _lastAngle;

        Boosted = false;
        IsLoading = false;

        //_playerStatus.LightSources ++; Redondance avec FlashlightController
    }

    /// <summary>
    /// Toggle off the flashlight
    /// </summary>
    public void ToggleOff()
    {
        _lastRadius = _fov.ViewRadius;
        _fov.ViewRadius = 0;

        _regenStart = Time.time + DelayBeforeRegen;
        //Lit = false;

        //_playerStatus.LightSources --; Redondance avec FlashlightController
    }
}
