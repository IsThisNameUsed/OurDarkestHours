using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class EndScene : MonoBehaviour
{
    private List<Rewired.Player> RInputs;
    private PlayTestMaster _ptm;
    
	void Start ()
    {
        RInputs = new List<Rewired.Player>();
        foreach(Rewired.Player player in ReInput.players.GetPlayers())
        {
            try
            {
                RInputs.Add(player);
            }
            catch (System.Exception) { }
        }

        _ptm = GameObject.Find("PlayTestMaster").GetComponent<PlayTestMaster>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		foreach(Rewired.Player RInput in RInputs)
        {
            if (RInput.GetAnyButtonDown())
            {
                _ptm.SwitchScene(_ptm.MenuScene);
                enabled = false;
            }
        }
	}
}
