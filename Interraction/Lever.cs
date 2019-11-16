using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Lever : Interactable
{
    [Space][Header("Main parameters")][Tooltip("Keep at 0 for instant interaction")]
    public float TimeToTrigger;
    private float _triggerStartTime;
    private float _triggerEndTime;
   

    [Tooltip("If enabled, stopping interaction will not reset lever position")]
    public bool KeepPulled;

    [Tooltip("Limit the lever to only one interaction")]
    public bool SingleEvent;
    private bool _alreadyTriggered;

    [Space][Header("Events")]
    public UnityEvent OnStartPulling;
    public UnityEvent OnStopPulling;

    public UnityEvent OnPlayer1StartPulling;
    public UnityEvent OnPlayer2StartPulling;
    private string _currentPlayer;

    [Space][Header("Events delaying")]
    public float StartDelay;
    public float StopDelay;

    private bool _pulled;
    private bool _isHolding;
    private Animator _anim;

    public Image fillImage;

    public bool isFirstLever;
    private bool dialogIsPlayed = false;

    private void Start()
    {
        actualState = state.hold;
        _anim = GetComponent<Animator>();
    }

    public void Update()
    {
        //Debug.Log(_isHolding);
        if(_isHolding) fillImage.fillAmount = TriggerTimePercentage();
    }

    public float TriggerTimePercentage()
    {
        float totalTime = _triggerEndTime - _triggerStartTime;
        float actualTime = _triggerEndTime - Time.time;

        return -((actualTime/totalTime)-1);
    }

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        if (isFirstLever && !dialogIsPlayed)
        {
            if(playerInteraction.name == "Player1")
                AkSoundEngine.PostEvent("john_holdlever", gameObject);
            else AkSoundEngine.PostEvent("vikky_holdlever", gameObject);
            dialogIsPlayed = true;
        }
            
        if (_alreadyTriggered)
            return;

        _currentPlayer = playerInteraction.name;

        if (KeepPulled && _pulled)
        {
            StopPull();
            _triggerEndTime = float.MaxValue;
        }
        else if (!_pulled)
        {
            _isHolding = true;
            _triggerStartTime = Time.time;
            _triggerEndTime = Time.time + TimeToTrigger;
        }
    }

    public override void InteractHold(PlayerInteraction playerInteraction)
    {
        _currentPlayer = playerInteraction.name;

        // If pulled for long enough, activate lever
        if (!_pulled && _triggerEndTime < Time.time && !_alreadyTriggered)
        {
            StartPull();
            if (SingleEvent)
                _alreadyTriggered = true;
        }
    }

    public override void InteractStop(PlayerInteraction playerInteraction)
    {
        // When stop interract and not keep pulled, the lever disactivate
        Debug.Log("STOP INTERRACT");
        if (_currentPlayer == playerInteraction.name)
        {
            _isHolding = false;
            fillImage.fillAmount = 0;
            if (!KeepPulled)
                StopPull();
        }
    }

    public void Pull(bool pull)
    {
        if (pull && !_pulled)
            StartPull();
        else if (!pull && _pulled)
            StopPull();
    }

    private void StartPull()
    {
        StartCoroutine(IInvokeStart());
        if (_anim != null)
            _anim.SetBool("Pulled", true);

        _pulled = true;
    }

    private void StopPull()
    {
        StopAllCoroutines();
        StartCoroutine(IInvokeStop());
        if (_anim != null)
            _anim.SetBool("Pulled", false);

        _pulled = false;
    }

    private IEnumerator IInvokeStart()
    {
        yield return new WaitForSeconds(StartDelay);
        OnStartPulling.Invoke();

        if (_currentPlayer == "Player1")
            OnPlayer1StartPulling.Invoke();
        else if (_currentPlayer == "Player2")
            OnPlayer2StartPulling.Invoke();
        else
            Debug.LogWarning("Can't decide if it's Vicky or Johnson", gameObject);
    }

    private IEnumerator IInvokeStop()
    {
        yield return new WaitForSeconds(StopDelay);
        OnStopPulling.Invoke();
    }
}
