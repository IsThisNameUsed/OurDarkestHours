using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Generator : Interactable
{
    [Space]
    public bool Activated;
    [ReadOnly]
    public float Percentage;

    [Space]
    [Header("Percentage paramaters")]
    public float IncreasePerMash;
    public float DecreasePerSecond;

    [Space]
    [Header("Disactivation parameters")]
    [Tooltip("Time to disable the generator, keep at 0 for no disable")]
    public float ActivateLength;
    private float _disableTimeStamp;
    public bool ResetPercentageWhenDisable;

    [Space][Header("Callbacks")]
    public UnityEvent OnActivated;
    public UnityEvent OnDisactivated;

    public UnityEvent OnPlayer1StartMashing;
    public UnityEvent OnPlayer2StartMashing;

    private string _currentPlayer;

    private bool _imageDisplayed;
    public Image _image;

    private void Start()
    {
        if (interactFeedbackMash == null)
        {
            Debug.LogError("No canvas, disabling generator");
            gameObject.SetActive(false);
            return;
        }
        _imageDisplayed = false;
    }

    public void Log(string txt)
    {
        Debug.Log(txt);
    }

	// Update is called once per frame
	void Update () {
		if (Percentage > 0 && !Activated)
        {
            Percentage -= DecreasePerSecond * Time.deltaTime;
            if (Percentage < 0)
                Percentage = 0;

            _image.fillAmount = Percentage / 100;
        }

        if (Activated && ActivateLength > 0.1f && Time.time > _disableTimeStamp)
        {
            Activated = false;
            OnDisactivated.Invoke();
            if (ResetPercentageWhenDisable)
                Percentage = 0;
        }
    }

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        if (!_imageDisplayed)
        {
            _imageDisplayed = true;
            displayImage(true);
            actualState = state.mash;

            if (playerInteraction.name == "Player1")
            {
                OnPlayer1StartMashing.Invoke();
                _currentPlayer = "Player1";
            }
                
            else if (playerInteraction.name == "Player2")
            {
                _currentPlayer = "Player2";
                OnPlayer2StartMashing.Invoke();
            }
                
            else
                Debug.LogWarning("Can't decide if it's Vicky or Johnson", gameObject);
        }

        // TODO optimize input mashing
        if (!Activated)
        {
            Percentage += IncreasePerMash;
            if (Percentage >= 100)
            {
                Percentage = 100;
                Activated = true;
                _disableTimeStamp = Time.time + ActivateLength;
                AkSoundEngine.PostEvent("play_generator_on", this.gameObject);
                OnActivated.Invoke();
                displayImage(false);
                DestroyInteractFeedback();
                this.gameObject.layer = 0;
            }

            _image.fillAmount = Percentage / 100;
        }
        
    }

    public override void InteractHold(PlayerInteraction playerInteraction)
    {
    }

    public override void InteractStop(PlayerInteraction playerInteraction)
    {
    }

    public void displayImage(bool display)
    {
        if (display) _image.enabled = true;
        else _image.enabled = false;
    }
}
