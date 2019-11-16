using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Item : Interactable
{

    private bool _isCarried;
    private Rigidbody _rigidbody;
    private GameObject _carrierPlayer;
    private Player _CarrierPlayerScript;

	void Start ()
    {
        _isCarried = false;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Interact(string player)
    {
        _isCarried = !_isCarried;
        if (_isCarried == true) PickUp(player);
        else Release();
    }

    private void PickUp(string player)
    {
        if(_carrierPlayer == null || _carrierPlayer.name != player)
        {
            _carrierPlayer = GameObject.Find(player);
            _CarrierPlayerScript = _carrierPlayer.GetComponent<Player>();
        }

        _CarrierPlayerScript.objectCarrying = name;
        _CarrierPlayerScript.ObjectCarryingRef = gameObject;
        Transform parent = _carrierPlayer.transform.Find("Body");
        gameObject.transform.parent = parent;
        Vector3 position = new Vector3(parent.position.x, parent.position.y + 1, parent.position.z + 1);
        transform.position = position;
    }

    private void Release()
    {
        _CarrierPlayerScript.objectCarrying = null;
        _CarrierPlayerScript.ObjectCarryingRef = null;
        transform.parent = null;
    }

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        /*Player pl = playerInteraction.GetComponent<Player>();
        /*if (pl.ObjectCarryingRef != gameObject)
        {
            ShowFloatingText(playerInteraction.name);
        }
        else if (pl.ObjectCarryingRef == gameObject)
        {
            DestroyFloatingText();
        }*/
    }

    public override void InteractHold(PlayerInteraction playerInteraction)
    {
        if (playerInteraction.RInput.GetButtonDown("Interact"))
            Interact(playerInteraction.name);
        
    }

    public override void InteractStop(PlayerInteraction playerInteraction)
    {
        Debug.Log("Interact stop with " + name, gameObject);
    }
}
