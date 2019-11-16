using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnStart : MonoBehaviour
{
    public Vector3 Position;

	// Use this for initialization
	void Start ()
	{
	    //transform.position = Position;
	    transform.localPosition = Position;
        Destroy(this);
	}
}
