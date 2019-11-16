using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class Credits : MonoBehaviour
{
    private Animator _anim;
    private List<Rewired.Player> RInputs;

    // Use this for initialization
    void Start ()
    {
        _anim = GetComponentInChildren<Animator>();
        _anim.SetTrigger("Display");
        StartCoroutine(ReturnToMenu());

        RInputs = new List<Rewired.Player>();
        foreach (Rewired.Player player in Rewired.ReInput.players.GetPlayers())
        {
            try
            {
                RInputs.Add(player);
            }
            catch (System.Exception) { }
        }
    }

    private void Update()
    {
        foreach (Rewired.Player RInput in RInputs)
        {
            if (RInput.GetButtonDown("Interact"))
            {
                AkSoundEngine.StopAll();
                PlayTestMaster ptm = GameObject.Find("PlayTestMaster").GetComponent<PlayTestMaster>();
                ptm.SwitchScene(ptm.MenuScene);
                //enabled = false;
            }
        }
    }

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(60.9f);
    }
}
