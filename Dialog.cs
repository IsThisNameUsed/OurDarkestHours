using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour {

    public GameObject emitter;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playAkEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, this.gameObject);
        Destroy(gameObject.GetComponent<AkGameObj>());
    }

    public void playAkEventOnCamera(string eventName)
    {
        AkSoundEngine.PostEvent(eventName,Camera.main.gameObject);
    }

    public void playAkEventMusicEmitter(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, emitter);
    }

    public void StopAkEventOnObject(GameObject obj)
    {
        AkSoundEngine.StopAll(obj);
    }
}
