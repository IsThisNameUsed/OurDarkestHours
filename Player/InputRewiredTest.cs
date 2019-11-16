using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputRewiredTest : MonoBehaviour
{
    public int PlayerId;

    public int Motor = 0;
    public float Speed = 1;
    public bool Vibrate;
	
	// Update is called once per frame
	void Update ()
    {
        Rewired.Player player = ReInput.players.GetPlayer(PlayerId);
        Debug.Log("MoveH : " + player.GetAxis("MoveH"));
        Debug.Log("MoveV : " + player.GetAxis("MoveV"));
        Debug.Log("Flash : " + player.GetButton("Flashlight"));
        Debug.Log("Inter : " + player.GetButton("Interact"));

        if (Vibrate)
            player.SetVibration(Motor, Speed);
        else
            player.SetVibration(0, 0, true);

        Controller controller = ReInput.controllers.GetController(ControllerType.Joystick, 0);
        
    }
}
