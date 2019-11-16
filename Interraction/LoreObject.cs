using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoreObject :Interactable {
    
    [Header("In event put GameManager as object, ReadLoreObject.ReadObject()")]
    [Header("as fonction and the desired sprite as input below")]
    public UnityEvent read;

    void Start () {
    }
	
	void Update () {
		
	}

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        read.Invoke();   
    }

    public override void InteractHold(PlayerInteraction playerInteraction)
    {

    }

    public override void InteractStop(PlayerInteraction playerInteraction)
    {
    }

}
