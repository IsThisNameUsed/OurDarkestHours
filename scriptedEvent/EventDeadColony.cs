using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDeadColony : MonoBehaviour {

    //Déclencher l'event sur un temps passé dans la colony (15 sec)
    //Désactiver torche des joueurs: desactiv Torch + desactiv FieldOfView + desactiv FlashLightController.
    // La lumière s'éteint -> bruit de grouillement/déplacement (les monstres sont dépop au début de l'event) pendant 3 ou 4 secondes -> spatialiser les sons si on a le time;
    // La lumière se rallume et il n'y a plus de monstre.


    public Light[] lights;
    public GameObject[] angel;
    public float delayBeforeStartEvent;
    public float timeLightAreTurnedOff;
    IEnumerator startEventCoroutine;
    IEnumerator disableAngelCoroutine;
    private bool disableAngelIsStarted;
    //Event for call activJumpScare() in EnemyTargeted on GameManager
    public UnityEvent reactivJumpScare;

    private FlashLightController lightController1;
    private FlashLightController lightController2;

    //List for the angels who are not disabled at the first pass because they are spotted by player's flashlight
    private List<Angel> angelsToDeactivateScripts;
    private List<GameObject> angelsToDeactivate;

    IEnumerator disableAngel()
    {
        bool allAngelDisabled = false;
        Debug.Log("start angel disable");
        while(!allAngelDisabled)
        {
            yield return new WaitForSeconds(0.01f);
            Debug.Log("count= " + angelsToDeactivateScripts.Count);
            if (angelsToDeactivateScripts.Count == 0)
                allAngelDisabled = true;
            for (int i = 0; i < angelsToDeactivateScripts.Count; i++)
            {
                if (angelsToDeactivateScripts[i].LightSources == 0)
                {
                    Debug.Log(angelsToDeactivate[i].name + " deactivated");
                    angelsToDeactivate[i].SetActive(false);
                    angelsToDeactivateScripts.RemoveAt(i);
                    angelsToDeactivate.RemoveAt(i);
                }   
            }
        }    
    }

    IEnumerator startEvent()
    {
        yield return new WaitForSeconds(delayBeforeStartEvent);
        //Flashlight deactivation
        lightController1.DesactivFlashLight();
        lightController2.DesactivFlashLight();


        //Environment light turn off
        AkSoundEngine.PostEvent("play_generator_off", this.gameObject);
        for (int i = 0;i<lights.Length;i++)
        {
            lights[i].enabled = false;
        }
        AkSoundEngine.PostEvent("play_jumpscare", this.gameObject);
        yield return new WaitForSeconds(1f);
        AkSoundEngine.PostEvent("vikky_damnit_anger", this.gameObject);
        AkSoundEngine.PostEvent("play_enemy_bones", this.gameObject);
        //START SOUND OF ANGELS MOVES

        yield return new WaitForSeconds(1);
        for (int i = 0; i < angel.Length; i++)
        {
            Angel angelScript = angel[i].GetComponent<Angel>();
            //If not visible, disable the angel
            if(angelScript.LightSources == 0)
                angel[i].SetActive(false);
            //else add to List to deal with it later in disableAngel coroutine
            else
            {
                angelsToDeactivate.Add(angel[i]);
                angelsToDeactivateScripts.Add(angelScript);

                if(disableAngelIsStarted == false)
                {
                    disableAngelIsStarted = true;
                }  
            }
        }
        //Activation flashlight
        lightController1.ActivFlashLight();
        lightController2.ActivFlashLight();
        if (disableAngelIsStarted == true)
        {
            disableAngelCoroutine = disableAngel();
            StartCoroutine(disableAngelCoroutine);
        }
        yield return new WaitForSeconds(timeLightAreTurnedOff-1);
        AkSoundEngine.PostEvent("stop_enemy_bones", this.gameObject);

        yield return new WaitForSeconds(1);
        //Environment lights turn on
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = true;
        }
        AkSoundEngine.PostEvent("dial_outpost", this.gameObject);
        if (disableAngelCoroutine!=null) StopCoroutine(disableAngelCoroutine);
        reactivJumpScare.Invoke();
    }

	void Start () {
        disableAngelIsStarted = false;
        angelsToDeactivateScripts = new List<Angel>();
        angelsToDeactivate = new List<GameObject>();
        lightController1 = GameObject.Find("Player1").GetComponent<FlashLightController>();
        lightController2 = GameObject.Find("Player2").GetComponent<FlashLightController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startEventDeadColony()
    {
        startEventCoroutine = startEvent();
        StartCoroutine(startEventCoroutine);
    }
}
