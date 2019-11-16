using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonySound : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AkSoundEngine.PostEvent("play_colony", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
