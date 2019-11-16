using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    [Tooltip("Object which is displayed when player is at range with an interactable object")]
    public GameObject interactFeedbackNormal;
    public GameObject interactFeedbackMash;
    public GameObject interactFeedbackHold;
    public float delayDestroyFloatingText;
    //[HideInInspector]
    public bool player1AtRange;
    //[HideInInspector]
    public bool player2AtRange;
    [HideInInspector]
    public bool onePlayerAtRange;

    public state actualState;
    public enum state { hold, mash, normal, none }

    private Vector3 SetPosition()
    {
        Vector3 position = transform.position;
        if (this is Door)
        {
            position.y = position.y + 3;
        }
        return position;
    }

    void Awake()
    {
        interactFeedbackNormal = this.gameObject.transform.Find("basic_interaction_canvas").gameObject;
        interactFeedbackMash = this.gameObject.transform.Find("mash_interaction_canvas").gameObject;
        interactFeedbackHold = this.gameObject.transform.Find("hold_interaction_canvas").gameObject;
        actualState = state.normal;
    }

    public virtual void ShowInteractFeedback()
    {  
        if (actualState == state.normal) ShowInteractFeedbackNormal();
        else if (actualState == state.mash) ShowInteractFeedbackMash();
        else if (actualState == state.hold) ShowInteractFeedbackHold();
        else if (actualState == state.none) DestroyInteractFeedback();
    }

    public virtual void ShowInteractFeedbackNormal()
    {
       // Debug.Log("Normal");
        if(interactFeedbackNormal != null) interactFeedbackNormal.SetActive(true);
        if (interactFeedbackMash != null) interactFeedbackMash.SetActive(false);
        if (interactFeedbackHold != null) interactFeedbackHold.SetActive(false);
    }

    public virtual void ShowInteractFeedbackMash()
    {
        //Debug.Log("Mash");
        if (interactFeedbackNormal != null)  interactFeedbackNormal.SetActive(false);
        if (interactFeedbackMash != null) interactFeedbackMash.SetActive(true);
        if (interactFeedbackHold != null) interactFeedbackHold.SetActive(false);
    }

    public virtual void ShowInteractFeedbackHold()
    {
        //Debug.Log("Hold");
        if (interactFeedbackNormal != null) interactFeedbackNormal.SetActive(false);
        if (interactFeedbackMash != null)  interactFeedbackMash.SetActive(false);
        if (interactFeedbackHold != null) interactFeedbackHold.SetActive(true);
        //interactFeedback = Instantiate(interactFeedbackHold, position, new Quaternion(0.8f, 0f, 0f, 1));
    }


    public virtual void DestroyInteractFeedback()
    {
        if (interactFeedbackNormal != null) interactFeedbackNormal.SetActive(false);
        if (interactFeedbackMash != null) interactFeedbackMash.SetActive(false);
        if (interactFeedbackHold != null) interactFeedbackHold.SetActive(false);
    }

    public void PlayerAtRange(string playerName, bool atRange)
    {
        if (playerName == "Player1") player1AtRange = atRange;
        else if (playerName == "Player2") player2AtRange = atRange;

        if (player1AtRange || player2AtRange) onePlayerAtRange = true;
        else onePlayerAtRange = false;
    }

    public abstract void InteractStart(PlayerInteraction playerInteraction);

    public abstract void InteractHold(PlayerInteraction playerInteraction);

    public abstract void InteractStop(PlayerInteraction playerInteraction);
}
