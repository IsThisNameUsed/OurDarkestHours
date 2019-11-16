using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : Interactable
{

    public bool turnedOn;
    private bool isActivable;

    public UnityEvent OnToggleOn;
    public UnityEvent OnToggleOff;

    private void Start()
    {
        isActivable = true;
    }

    IEnumerator WaitForNewActivation()
    {
        yield return new WaitForSeconds(0.1f);
        isActivable = true;
    }

    public void updateState()
    {
        if(isActivable == true) turnedOn = !turnedOn;
        isActivable = false;
        StartCoroutine(WaitForNewActivation());

        if (turnedOn)
            OnToggleOn.Invoke();
        else
            OnToggleOff.Invoke();
    }
    
    public bool returnState()
    {
        return turnedOn;
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
