using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(FlashlightBattery))]
public class FlashLightController : MonoBehaviour
{
    public float AngleIncrease;
    public float Speed;
    public float TimeToSwitch;
    [ReadOnly]
    public bool isActiv = true;
    private float _baseAngle;
    private float _baseLightAngle;
    private FieldOfView _fov;
    private Lightable _playerStatus;

    [SerializeField][ReadOnly]
    private bool _modifying;

    private FlashlightBattery _battery;
    private OnOffLight _ool;

    public GameObject Player;

    private InputManager inputManager;
    private Rewired.Player RInput;

    // Use this for initialization
    void Start ()
    {
        if (Player == null)
            Player = gameObject;

        _playerStatus = GetComponent<Lightable>();
	    _battery = GetComponent<FlashlightBattery>();
	    _ool = GetComponent<OnOffLight>();

        //le joueur démarre la lampe allumé
        _playerStatus.LightSources = 1;

        _ool.OnToggle += (on) =>
        {
            if (on == true) _playerStatus.LightSources += 1;
            else _playerStatus.LightSources -= 1;
        };

	    _fov = GetComponent<FieldOfView>();

	    _baseAngle = _fov.ViewAngle;

        if (_baseAngle + AngleIncrease > 360)
        {
            AngleIncrease = 360 - _baseAngle;
        }

        _baseLightAngle = _ool.Lights[0].spotAngle;

        // To determine joypad or keyboard
        inputManager = GameObject.Find("GameManager").GetComponent<InputManager>();
        if (Player.name == "Player2")
        {
            RInput = Rewired.ReInput.players.GetPlayer(inputManager.Player2Index);
        }
        else
        {
            RInput = Rewired.ReInput.players.GetPlayer(inputManager.Player1Index);
        }

        _ool.ToggleOff();
    }

    // Update is called once per frame
    void Update ()
    {
        if (RInput.GetButtonDown("Flashlight") && !_modifying)
        {
            if (_ool.Toggled && isActiv)
            {
                AkSoundEngine.PostEvent("play_flashlight_off", this.gameObject);
                _ool.ToggleOff();
            }
            else if (!_ool.Toggled && _battery.CurrentBattery > _battery.ToggleOnCost && isActiv)
            {
                AkSoundEngine.PostEvent("play_flashlight_on", this.gameObject);
                Debug.Log("SOUND PLAYER");
                _ool.ToggleOn();
            }
        }
    }
    
    public void DesactivFlashLight()
    {
        _ool.ToggleOff();
        isActiv = false;
        AkSoundEngine.PostEvent("play_flashlight_shut_off", this.gameObject);
    }

    public void ActivFlashLight()
    {
        _ool.ToggleOn();
        isActiv = true;
        AkSoundEngine.PostEvent("play_flashlight_on", this.gameObject);
    }
    private IEnumerator Increase()
    {
        // Prevent light radius issue if flashlight spamming
        foreach (Light light in _ool.Lights)
        {
            if (light.type == LightType.Spot)
                light.spotAngle = _baseLightAngle;
        }

        /*// Prevent light radius issue if flashlight spamming
        _ool.Light.spotAngle = _baseLightAngle;*/
        
        _ool.ToggleOff();
        
        yield return new WaitForSeconds(TimeToSwitch);

        _ool.ToggleOn();
        _fov.ViewAngle = _baseAngle + AngleIncrease;

        /*_ool.Light.spotAngle = _baseLightAngle + AngleIncrease;*/
        foreach (Light light in _ool.Lights)
        {
            if (light.type == LightType.Spot)
                light.spotAngle = _baseLightAngle + AngleIncrease;
        }

        _modifying = false;
        _battery.Boosted = true;
    }

    private IEnumerator Decrease()
    {
        // Prevent light radius issue if flashlight spamming
        foreach (Light light in _ool.Lights)
        {
            if (light.type == LightType.Spot)
                light.spotAngle = _baseLightAngle + AngleIncrease;
        }
        /*_ool.Light.spotAngle = _baseLightAngle + AngleIncrease;*/

        _ool.ToggleOff();

        yield return new WaitForSeconds(TimeToSwitch);

        _ool.ToggleOn();
        _fov.ViewAngle = _baseAngle;

        /*_ool.Light.spotAngle = _baseLightAngle;*/
        foreach (Light light in _ool.Lights)
        {
            if (light.type == LightType.Spot)
                light.spotAngle = _baseLightAngle;
        }

        _modifying = false;
        _battery.Boosted = false;
    }
}
