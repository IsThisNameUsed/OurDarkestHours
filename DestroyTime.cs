using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour {

    public float Time = 2;
    public bool OnlyDisactivate;

	// Use this for initialization
	void Start ()
    {
        if (OnlyDisactivate)
            gameObject.SetActive(false);
        else
            Destroy(gameObject, Time);
	}
}
