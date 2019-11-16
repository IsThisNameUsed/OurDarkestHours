using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractFeedbackMove : MonoBehaviour {

    public GameObject player;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(player!=null)
        {
            Vector3 position = player.transform.position;
            position.y = position.y + 3;
            transform.position = position;
        }
        
	}
}
