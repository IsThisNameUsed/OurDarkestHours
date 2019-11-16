using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : Interactable {

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        if(playerInteraction.gameObject.GetComponent<ThrowFlare>().pickupFlare())
        {
            AkSoundEngine.PostEvent("play_flare_pickup", gameObject);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            Destroy(gameObject, 1);
        }
    }

    public override void InteractHold(PlayerInteraction playerInteraction)
    {
       
    }

    public override void InteractStop(PlayerInteraction playerInteraction)
    {
        
    }
}
