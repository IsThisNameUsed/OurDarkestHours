using System.Collections;
using UnityEngine;
//Gère le popup indiquant la touche d'interaction sur la porte, le mouvement de la porte.

public class HeavyGate : Interactable
{
 
    public float delayToStartAction;
    public float closingTime;
    public float openingTime;
    public bool gateIsOpened;
    private bool player1IsOpening;
    private bool player2IsOpening;
    public Vector3 closedPosition;
    public Vector3 openedPosition;
    private bool _unlocked;
    private ItemReceiver _itemReceiver;
    [Tooltip("Need a key to open apparently")]
    public bool needKey;
    [Tooltip("Name of the key object expected, must be identical to the name of the object himself ")]
    public string keyName;


    IEnumerator OpeningCoroutine()
    {
        float openingSpeed = (openedPosition.y - closedPosition.y) / openingTime;
        yield return new WaitForSeconds(delayToStartAction);
        if (player1IsOpening == true && player2IsOpening == true && transform.position.y < openedPosition.y)
            transform.position = Vector3.MoveTowards(transform.position, openedPosition, Time.deltaTime * openingSpeed);
    }

    private void Start()
    {
        openedPosition = transform.position + new Vector3(0, 3f, 0);
        closedPosition = transform.position;
        if (needKey)
        {
            _itemReceiver = gameObject.AddComponent<ItemReceiver>();
            _itemReceiver.expectedItem = keyName;
            _unlocked = false;
        }
        else _unlocked = true;
    }

    private void Update()
    {
        gateIsOpened = transform.position.y >= openedPosition.y ? true : false;

        if (player1IsOpening == false && player2IsOpening == false) Closing();
        else if (player1IsOpening == true && player2IsOpening == true) StartCoroutine(OpeningCoroutine());
        else if ((player1IsOpening == true || player2IsOpening == true) && !gateIsOpened) Closing();
          
    }

    public void Closing()
    {
        float closingSpeed = (openedPosition.y - closedPosition.y) / closingTime;
        transform.position = Vector3.MoveTowards(transform.position, closedPosition, Time.deltaTime * closingSpeed);
    }

    //Met à jour quel joueur interagit avec la porte
    public void  UpdateGate(string player,bool isOpening)
    {
        if (!_unlocked)
        {
            bool possessKey = _itemReceiver.Interact(player);
            if (possessKey) _unlocked = true;
            else return;
        }
        else
        {
            if (player == "Player1") player1IsOpening = isOpening;
            else player2IsOpening = isOpening;
        }  
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
