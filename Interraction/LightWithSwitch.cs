using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWithSwitch : Interactable {

    [Header("light options")]
    public bool hasIntensityVariation;
    public bool isFlashing;
    [Tooltip("The probability that the light is flashing, in %")]
    [Range(0,100)] public float flashingFrequency;
    public bool turnOnAtStart;
    public float intensity;

    [Header("Switch")]
    [Tooltip("put a GameObject with a Switch script")]
    public GameObject linkedSwitch;

    private Switch switchScript;
    private Light light;
    
    private float _velocity = 0.0f;
    private bool _turnedOn;
    private bool _turnedOff;
    private bool intensityVariationIsRunning = false;
    private bool flashingIsRunning = false;

    IEnumerator intensityVariation()
    {
        intensityVariationIsRunning = true;
        int time = 0;
        while (time<10)
        {
            float intensityTargeted = Random.Range(intensity/2, intensity);
            bool higherIntensity = light.intensity > intensityTargeted ? false : true;
            float pas = (light.intensity - intensityTargeted) / 100;
            float speed = Random.Range(1,5);
            pas = pas * speed;
            while (light.intensity != intensityTargeted)
            {
                if (higherIntensity)
                    light.intensity += 0.01f;
                else light.intensity -= 0.01f;
                yield return null;
                if (higherIntensity && light.intensity > intensityTargeted - pas*3)
                    light.intensity = intensityTargeted;
                else if(!higherIntensity && light.intensity < intensityTargeted + pas*3)
                    light.intensity = intensityTargeted;
            }
            time += 1; 
        }
        intensityVariationIsRunning = false;
        _turnedOn = false;
    }

    IEnumerator Flashing()
    {
        flashingIsRunning = true;
        int numberOffFlash = Random.Range(1, 5);
        int i = 0;
        while(i < numberOffFlash)
        {
            float timeBetweenFlashing = Random.Range(0.05f, 1f);
            float lastIntensity = light.intensity;
            light.intensity = 0;
            yield return new WaitForSeconds(timeBetweenFlashing);
            timeBetweenFlashing = Random.Range(0.05f, 1f);
            light.intensity = lastIntensity;
            yield return new WaitForSeconds(timeBetweenFlashing);
            i++;
        }
        _turnedOn = false;
        flashingIsRunning = false;
    }
    IEnumerator turnOffLight()
    {
        while (light.intensity > 0)
        {
            yield return new WaitForSeconds(0.1f);
            light.intensity -= 0.1f;
        }
    }

    void Start () {
        if (linkedSwitch != null)
            switchScript = linkedSwitch.GetComponent<Switch>();

        light = GetComponent<Light>();
        if (turnOnAtStart)
        {
            switchScript.turnedOn = true;
            _turnedOn = false;
            _turnedOff = false;
            light.intensity = intensity;
        }
        else
        {
            switchScript.turnedOn = false;
            _turnedOff = false;
            light.intensity = 0;
        }
        
	}
	
    void TurnOn()
    {
        float newIntensity = Mathf.SmoothDamp(light.intensity, intensity, ref _velocity, 1f);
        light.intensity = newIntensity;
    }

    void TurnOff()
    {
        float newIntensity = Mathf.SmoothDamp(light.intensity, 0, ref _velocity, 1f);
        light.intensity = newIntensity;
    }

	void Update () {

        if (switchScript.turnedOn && !_turnedOn)
        {
            _turnedOff = false;
            TurnOn();
            if (light.intensity >= intensity - 0.05f)
            {
                light.intensity = intensity;
                _turnedOn = true;
            }
        }
            
        if (!switchScript.turnedOn && !_turnedOff)
        {
            _turnedOn = false;
            TurnOff();
            if (light.intensity <= 0 + 0.05f)
            {
                light.intensity = 0;
                _turnedOff = true;
            }
        }

        if (_turnedOn && hasIntensityVariation)
        {
            if (!intensityVariationIsRunning)
            {
                int rand = Random.Range(0, 1000);
                if(rand <= 5)
                {
                    StartCoroutine(intensityVariation());
                }
            }
        }

        if(isFlashing && !_turnedOff && !flashingIsRunning)
        {
            int rand = Random.Range(0, 100);
            if (rand < flashingFrequency)
                StartCoroutine(Flashing());
        }
    }

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        throw new System.NotImplementedException();
    }

    public override void InteractHold(PlayerInteraction playerInteraction)
    {
        throw new System.NotImplementedException();
    }

    public override void InteractStop(PlayerInteraction playerInteraction)
    {
        throw new System.NotImplementedException();
    }
}
