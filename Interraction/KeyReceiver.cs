using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyReceiver : Interactable
{
    public bool EnterToTrigger = true;
    public string ExpectedItem;

    public UnityEvent OnKeyReceived;

    public bool Interact(Player player)
    {
        return TestKey(player);
    }

    public override void InteractHold(PlayerInteraction playerInteraction)
    {
    }

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        Player player = playerInteraction.GetComponent<Player>();
        TestKey(player);
    }

    public override void InteractStop(PlayerInteraction playerInteraction) {}

    /*private void OnTriggerEnter(Collider other)
    {
        if (EnterToTrigger && other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            TestKey(player);
        }
    }*/

    private bool TestKey(Player player)
    {
        if (player.ObjectCarryingRef.name == ExpectedItem)
        {
            Destroy(player.ObjectCarryingRef);
            OnKeyReceived.Invoke();
            return true;
        }
        return false;
    }
}
