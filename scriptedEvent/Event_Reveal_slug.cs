using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Reveal_slug : MonoBehaviour {

    //le slug est éclairé par deux lumières: l'une d'entre elle s'éteint 3 secondes, lui laissant le temps d'avancer lentement vers les joueurs.
    //La lumière se rallume: il redevient immobile. L'autre lumière s'éteint, il se remet à bouger lentement et... les deux lumières s'éteignent.
    //Les joueurs sont dans la mouise.
    //POMMES
    //POIRES
    //PIERRE

    
    public OnOffLight lamp1;
    public OnOffLight lamp2;
    public OnOffLight lamp3;

    [Header ("Delay before lamp1 (at right) is turn on")]
    public float delay1=3;
    [Header("Delay before lamp2 (at left) is turn off")]
    public float delay2=3;
    [Header("Delay before lamp2 and lamp 1 are turn off")]
    public float delay3=3;

    IEnumerator startSequence()
    {
        //Turn off lamp1
        lamp1.ToggleOff();
        lamp3.ToggleOff();

        yield return new WaitForSeconds(delay1);
        //Turn on lamp1
        lamp1.ToggleOn();

        yield return new WaitForSeconds(delay2);
        //Turn off lamp2
        lamp2.ToggleOff();

        yield return new WaitForSeconds(delay3);
        //Turn off lamp1
        lamp1.ToggleOff();
        lamp3.ToggleOn();
    }

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void startEvent()
    {
        StartCoroutine("startSequence");
    }
}
