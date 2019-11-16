using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class DualMove : MonoBehaviour
{
    public PlayerTag Player;
    public float Speed;
	
	// Update is called once per frame
	[UsedImplicitly]
	private void Update()
	{
	    float horizontal = 0f;
	    float vertical = 0f;

	    if (Player == PlayerTag.Bob)
	    {
	        horizontal = Input.GetAxis("Horizontal");
	        vertical = Input.GetAxis("Vertical");
	    }
        else if (Player == PlayerTag.Jhonson)
	    {
	        horizontal = Input.GetAxis("Horizontal2");
	        vertical = Input.GetAxis("Vertical2");
        }

        Move(horizontal, vertical);
	}

    private void Move(float x, float y)
    {
        Vector3 move = new Vector3(x, 0, y);
        move = Vector3.Normalize(move);
        move *= Time.deltaTime * Speed;
        transform.Translate(move);
    }
    

    public enum PlayerTag
    {
        Bob,
        Jhonson
    }
}
