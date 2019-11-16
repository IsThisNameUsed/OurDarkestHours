using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScene : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(StopVib());
        StartCoroutine(BackToMenu());
    }

    private IEnumerator StopVib()
    {
        yield return new WaitForSeconds(2);
        foreach (Rewired.Player rInput in Rewired.ReInput.players.GetPlayers())
        {
            rInput.SetVibration(0, 0, true);
        }
    }

    private IEnumerator BackToMenu()
    {
        PlayTestMaster ptm = GameObject.Find("PlayTestMaster").GetComponent<PlayTestMaster>();
        yield return new WaitForSeconds(59.5f);
        ptm.SwitchScene(ptm.MenuScene);
    }
}
