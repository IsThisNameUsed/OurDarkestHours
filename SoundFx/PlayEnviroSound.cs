using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEnviroSound : MonoBehaviour {

    public string eventname;
	void Start () {

        AkSoundEngine.PostEvent(eventname, this.gameObject);
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
