using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightOnFloor : Interactable
{
    public override void InteractHold(PlayerInteraction playerInteraction) {}

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        OnOffLight oof = playerInteraction.GetComponent<OnOffLight>();
        if (oof.enabled)
        {
            oof.enabled = true;
            playerInteraction.GetComponent<FlashlightBattery>().enabled = true;
            playerInteraction.GetComponent<FlashLightController>().enabled = true;
            playerInteraction.GetComponent<FieldOfView>().enabled = true;
            Destroy(gameObject);
        }
    }

    public override void InteractStop(PlayerInteraction playerInteraction) {}
}
