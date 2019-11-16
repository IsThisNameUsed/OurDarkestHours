using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReceiver : Interactable {

    public string expectedItem;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool Interact(string player)
    {
        GameObject interactingPlayer = GameObject.Find(player);
        Player interactingPlayerScript = interactingPlayer.GetComponent<Player>();
        if (interactingPlayerScript.objectCarrying == expectedItem)
        {
            GameObject item = GameObject.Find(expectedItem);

            Destroy(item);
            Debug.Log("QuestComplete");
            return true;
        }
        else return false;
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
