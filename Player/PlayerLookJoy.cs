using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerLookJoy : MonoBehaviour
{

    public float speed;
    private InputManager inputManager;
    private Player _player;

    private Rewired.Player RInput;

    void Start()
    {
        inputManager = GameObject.Find("GameManager").GetComponent<InputManager>();
        if (name == "Player2")
        {
            RInput = ReInput.players.GetPlayer(inputManager.Player2Index);
        }
        else
        {
            RInput = ReInput.players.GetPlayer(inputManager.Player1Index);
        }

        _player = GetComponent<Player>();
    }

    void Update ()
    {
        if (_player.IsDown)
            return;

        Vector3 direction;

        float lookBackwardForward = RInput.GetAxis("OrientationX");
        float lookLeftRight = RInput.GetAxis("OrientationY");

        if (lookLeftRight != 0 || lookBackwardForward != 0)
        {
            direction = new Vector3(lookBackwardForward, 0, lookLeftRight);
            float step = speed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }  
}
