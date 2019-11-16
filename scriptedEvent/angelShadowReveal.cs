using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Mettre son sur le props de téléphone, il se coupe quand les joueurs arrivent à 3 ou 4 mètre(au niveau des grosses visse).
//la lumière clignote tout le temps et l'ange apparait durant l'un des clignotements
public class angelShadowReveal : MonoBehaviour {

    private Light light;
    public GameObject angel;
    [Tooltip("flash time of light")]
    public float delay;
    public float DialogueDelay = 4f;
    private bool _eventPlayed = false;
    private bool _startEvent = false;
    private bool _angelIsActiv = false;
    IEnumerator coroutine;
    
    IEnumerator revealSequence()
    {
        float time;
        while (true)
        {
            if (_startEvent)
            {
                angel.SetActive(true);
                _angelIsActiv = true;
            }

            time = Random.Range(delay - 0.1f, delay + 0.1f);
            yield return new WaitForSeconds(time);
            light.enabled = true;
            if (_angelIsActiv)
            {
                AkSoundEngine.PostEvent("play_jumpscare", this.gameObject);
                StartCoroutine(TriggerEvent());
                _eventPlayed = true;
                _startEvent = false;
                _angelIsActiv = false;
            }
            time = Random.Range(delay - 0.2f, delay + 0.2f);
            yield return new WaitForSeconds(time);

            light.enabled = false;

            if (_eventPlayed)
                angel.SetActive(false);
        }
    }
    void Start ()
    {
        light = GetComponent<Light>();
        //angel.GetComponent<Angel>().enabled = false;
        light.enabled = false;
        angel.SetActive(false);
        coroutine = revealSequence();
        StartCoroutine(coroutine);
    }

    public void startReveal()
    {
        _startEvent = true;
    }

    private IEnumerator TriggerEvent()
    {
        yield return new WaitForSeconds(DialogueDelay);
        AkSoundEngine.PostEvent("dial_shadowgenerator", this.gameObject);
    }

    public void StopEvent()
    {
        StopAllCoroutines();
        light.enabled = false;
    }
}
