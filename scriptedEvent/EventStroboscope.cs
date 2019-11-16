using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventStroboscope : MonoBehaviour
{
    //STROBOSCOPE
    //Au milieu de la pièce un trigger retire les lampes torches des joueurs et éteint la lumière de la pièce. Des bruits de monstres retentissent.
    //La lumière se rallume, laissant apparaître de nombreux monstres autours des joueurs: le stroboscope se lance et les monstres 
    //s'approche de plus en plus des joueurs. (déterminer le temps par rapport à la musique)
    //Très proche des joueurs: c'est la fin du jeu et on les entends hurler.
    // Use this for initialization
    //dial_end quand les anges commencent à les fritter.

    public GameObject[] lights;
    public FlashLightController _flcPlayer1;
    public FlashLightController _flcPlayer2;
    public OnOffLight _oolEnviroLight;
    private IEnumerator coroutine;
    private Angel[] angelScripts;
    public GameObject[] angels;

    public GameObject BlackImage;

    public string EndScene = "ODH_Fin";

    IEnumerator Sequence()
    {
        _oolEnviroLight.Toggle();
        AkSoundEngine.PostEvent("play_enemy_bones", gameObject);
        
        yield return new WaitForSeconds(1); // 1

        AkSoundEngine.PostEvent("dial_deathiscoming", gameObject);
        int indice = 0;
        foreach (GameObject angel in angels)
        {
            angel.SetActive(true);
            angelScripts[indice] = angel.GetComponent<Angel>();
            angelScripts[indice].Activated = false;
            indice++;
        }

        yield return new WaitForSeconds(2); // 3

        _flcPlayer1.GetComponent<Player>().IsInvincible = true;
        _flcPlayer2.GetComponent<Player>().IsInvincible = true;
        _oolEnviroLight.Toggle();
        _oolEnviroLight.FlickeringType = OnOffLight.FlickerType.STROBOSCOPE;
        _oolEnviroLight.StroboscopeRate = 6;
        foreach (Angel angel in angelScripts)
        {
            angel.Activated = true;
        }
        AkSoundEngine.PostEvent("stop_enemy_bones", gameObject);

        yield return new WaitForSeconds(1); // 4

        /*_flcPlayer1.DesactivFlashLight();
        _flcPlayer2.DesactivFlashLight();*/
        _oolEnviroLight.StroboscopeRate = 6;
        //AkSoundEngine.PostEvent("dial_cantturn", gameObject);
        
        yield return new WaitForSeconds(1); // 6

        _oolEnviroLight.StroboscopeRate = 6;
        AkSoundEngine.StopAll();
        AkSoundEngine.PostEvent("dial_end", gameObject);

        yield return new WaitForSeconds(4); // 10

        GameObject.Find("PlayTestMaster").GetComponent<PlayTestMaster>().SwitchSceneStrobo(EndScene, true);

        yield return new WaitForSeconds(3); // 13

        BlackImage.SetActive(true);
    }

    void Start ()
    {
        angelScripts = new Angel[angels.Length];
    }

    public void StartSequence()
    {
        Debug.Log("Start event");
        coroutine = Sequence();
        StartCoroutine(coroutine);
    }
}
