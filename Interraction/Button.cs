using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : Interactable
{
    public UnityEvent OnPressed;
    public bool SingleEvent;

    private bool _triggered;

    public override void InteractHold(PlayerInteraction playerInteraction)
    {
        return;
    }

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        if (!_triggered)
            OnPressed.Invoke();

        if (SingleEvent)
            _triggered = true;
    }

    public override void InteractStop(PlayerInteraction playerInteraction)
    {
        return;
    }
}
