using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFirstFight : MonoBehaviour {

    // Quand les joueurs arrivent la spotLight qui immobilise l'ange clignote vite fait et puis s'éteint rapidement
    // Use this for initialization

    public OnOffLight light;

    IEnumerator LightFlashing()
    {
        light.FlickeringType = OnOffLight.FlickerType.RandomFlicker;
        yield return new WaitForSeconds(3);
        light.Toggle();
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartEvent()
    {
        StartCoroutine("LightFlashing");
    }
}
