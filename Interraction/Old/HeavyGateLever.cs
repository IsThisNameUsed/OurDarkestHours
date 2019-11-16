using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyGateLever : Interactable
{
    public float delayToStartAction;
    public float closingTime;
    public float openingTime;
    public AudioSource audioSource;
    public AudioClip closingSound;
    public AudioClip openingSound;

    public HeavyGateLever otherLever;
    private Transform door1;
    private Transform door2;
    public Vector3 door1ClosedPosition;
    public Vector3 door1OpenedPosition;
    public Vector3 door2ClosedPosition;
    public Vector3 door2OpenedPosition;

    private bool player1IsOpening;
    private bool player2IsOpening;
    public bool _unlocked;
    private bool _activated;
    private bool isOpening;

    [Header("key object")]
    private ItemReceiver _itemReceiver;
    [Tooltip("Need a key to open apparently")]
    public bool needKey;
    [Tooltip("Name of the key object expected, must be identical to the name of the object himself ")]
    public string keyName;

    [Header("Switch")]
    public bool hasSwitch;
    [Tooltip("put a GameObject with a Switch script")]
    public GameObject linkedSwitch;
    private Switch switchScript;

    private void Awake()
    {
        if (this.name == "Lever1")
            otherLever = this.transform.parent.GetChild(1).gameObject.GetComponent<HeavyGateLever>();
        else if(this.name == "Lever2")
            otherLever = this.transform.parent.GetChild(0).gameObject.GetComponent<HeavyGateLever>();
        door1 = this.transform.parent.GetChild(2);
        door2 = this.transform.parent.GetChild(3);
    }
    private void Start()
    {
        float soundLength;
        if (openingSound != null)
            soundLength = openingSound.length;
        door1OpenedPosition = door1.position + door1.right * (1.32f);
        door1ClosedPosition = door1.position;
        door2OpenedPosition = door2.position + door2.right * (1.32f);
        door2ClosedPosition = door2.position;

        if (needKey)
        {
            _itemReceiver = gameObject.AddComponent<ItemReceiver>();
            _itemReceiver.expectedItem = keyName;
            _unlocked = false;
        }
        else _unlocked = true;

        if (hasSwitch)
        {
            switchScript = linkedSwitch.GetComponent<Switch>();
            _activated = false;
        }
        else _activated = true;
       
    }

    private void Update()
    {
        if (!onePlayerAtRange && door1.position != door1ClosedPosition && !otherLever.isOpening) Closing();
    }

    private bool switchStateUpdate()
    {
        if (!hasSwitch) return true;
        return switchScript.returnState();
    }

    private bool keyUpdate(string playerName)
    {
        return _itemReceiver.Interact(playerName); 
    }

    //Check key and switch
    private bool isOpenable (string playerName)
    {
        if (!_unlocked)
        {
            otherLever._unlocked = keyUpdate(playerName);
            _unlocked = keyUpdate(playerName);
        }
        _activated = switchStateUpdate();
        return (_unlocked && _activated);
    }

    public void Opening()
    {
        if (!isOpening && openingSound != null)
        {
            audioSource.clip = openingSound;
            audioSource.time = 0.1f;
            audioSource.Play();
            Debug.Log("PLAY");
            isOpening = true;
        }
        
        float openingSpeed = (door1OpenedPosition.x - door1ClosedPosition.x) / openingTime;
        door1.position = Vector3.MoveTowards(door1.position, door1OpenedPosition, Time.deltaTime * openingSpeed);
        door2.position = Vector3.MoveTowards(door2.position, door2OpenedPosition, Time.deltaTime * openingSpeed);
    }

    public void Closing()
    {
        if(isOpening)
        {
            audioSource.clip = openingSound;
            audioSource.time = 0.1f;
            audioSource.Play();
            isOpening = false;
        }
        float closingSpeed = (door1OpenedPosition.x - door1ClosedPosition.x) / closingTime;
        door1.position = Vector3.MoveTowards(door1.position, door1ClosedPosition, Time.deltaTime * closingSpeed);
        door2.position = Vector3.MoveTowards(door2.position, door2ClosedPosition, Time.deltaTime * closingSpeed);
    }

    //Appelée depuis PlayerInteraction
    public void UpdateGate(string playerName, bool isOpening)
    {
        if (playerName == "Player1") player1IsOpening = isOpening;
        else if(playerName == "Player2")player2IsOpening = isOpening;
        if(isOpenable(playerName)) interactGate();
    }

    private void interactGate()
    {
        if (player1IsOpening == true || player2IsOpening == true)
        {
            Opening();
        }
        else if (player1IsOpening == false && player2IsOpening == false && !otherLever.isOpening) Closing();
    }

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        Debug.LogError("Deprecated item");
        throw new System.NotImplementedException();
    }

    public override void InteractHold(PlayerInteraction playerInteraction)
    {
        Debug.LogError("Deprecated item");
        throw new System.NotImplementedException();
    }

    public override void InteractStop(PlayerInteraction playerInteraction)
    {
        Debug.LogError("Deprecated item");
        throw new System.NotImplementedException();
    }
}
