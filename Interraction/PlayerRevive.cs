using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRevive : Interactable
{
    public float timeToResuscite;
    private float startTime;
    private float endTime;
    public string playerName;
    public Player playerScript;
    public Image fillImage;
    public bool isResuscitating;
    private PlayerInteraction _pi;
    private MashToSurvive mashScript;

    private void Awake()
    {
        interactFeedbackNormal = this.gameObject.transform.Find("basic_interaction_canvas").gameObject;
        interactFeedbackHold = this.gameObject.transform.Find("hold_interaction_canvas").gameObject;
        actualState = state.none;
    }

    void Start () {
        playerName = transform.parent.name;
        playerScript = this.gameObject.GetComponent<Player>();
        mashScript = gameObject.GetComponent<MashToSurvive>();
	}

    private void Update()
    {
        if (playerScript.IsDown && !isResuscitating) actualState = state.normal;
        else if(playerScript.IsDown && isResuscitating) actualState = state.hold;
        else actualState = state.none;
        if (isResuscitating)
        {
            fillImage.fillAmount = TimePercentage();
            if(fillImage.fillAmount >=1)
            {
                playerScript.IsDown = false;
                playerScript.ActiveToken(playerScript.aliveToken);
                actualState = state.none;
                mashScript.StopMash();
            }
        }
    }

    private float TimePercentage()
    {
        return (Time.time - startTime) / timeToResuscite;
    }

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        actualState = state.hold;
        startTime = Time.time;
        endTime = Time.time + timeToResuscite;
        return;
    }

    public override void InteractHold(PlayerInteraction playerInteraction)
    {
        isResuscitating = true;
        return;
    }

    public override void InteractStop(PlayerInteraction playerInteraction)
    {
        isResuscitating = false;
        fillImage.fillAmount = 0;
        return;
    }
}
