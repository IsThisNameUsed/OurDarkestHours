using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelReveal2 : MonoBehaviour {
    // Les lumières du couloir s'éteignent quand les joueurs arrivent à la deuxième lampe et l'ange pop derrière eux (pop dans tourniquet? -> bruit de tourniquet  pour indiquer son passage)
   // Mettre une pause mettant l'ange dans la lumière avant les sequence d'extinction. Régler les fields Of view et tout le bordel pour qu'il avance bien par étape à chaque extinction.
   // dialog_reveal quand le monstre est visible pour les joueurs et dialog_reveal2 quand l'ange passe la deuxième lampe.
   // john_pleasecommon -> quand il commence à masher  vikky_itscoming quand elle a masher la moitié ; john_comeon_shout quand la porte est ouverte
    public GameObject[] lights;
    public float delay = 2;
    public GameObject angel;
    private GameObject player1;
    private GameObject player2;

    IEnumerator turnOffLight()
    {
        OnOffLight _oolP1 = player1.GetComponent<OnOffLight>();
        OnOffLight _oolP2 = player2.GetComponent<OnOffLight>();
        FlashLightController _flcP1 = player1.GetComponent<FlashLightController>();
        FlashLightController _flcP2 = player2.GetComponent<FlashLightController>();

        int indice = 0;
        angel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        //_flcP1.DesactivFlashLight();
        //_flcP2.DesactivFlashLight();
        //AkSoundEngine.PostEvent("play_jumpscare", this.gameObject);
        while (indice<lights.Length)
        {
            yield return new WaitForSeconds(delay);
            lights[indice].GetComponent<OnOffLight>().ToggleOff();
            lights[indice].transform.parent.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            lights[indice + 1].GetComponent<OnOffLight>().ToggleOff();
            lights[indice + 1].transform.parent.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            indice += 2;
            Debug.Log(indice);
            if(indice == lights.Length - 4)
            {
                _flcP1.ActivFlashLight();
                _flcP2.ActivFlashLight();
            }
        }
        

    }
	void Start () {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startSequence()
    {
        StartCoroutine("turnOffLight");
    }
}
