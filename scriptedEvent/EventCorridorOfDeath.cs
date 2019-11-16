using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCorridorOfDeath : MonoBehaviour {
    //CORRIDOR OF DEATH
    //Players are walking along the tunnel. On peut entendre autours d'eux des bruits de monstres marchant (derrière et devant).
    //Chaque trigger placé fait spawn un monstre (loin du joueur): jusqu'à créer une situation incontrôlable -> obligation de lancer le flare, 
    //le couloir doit être assez long pour décharger les batteries assez tôt.
    //Dialogue john_theyrecomming au moment ou il voit le premier monstre 2 seconde après ->vikki_throwflare

    public GameObject[] popAngelsTriggerOne;
    public GameObject[] popAngelsTriggerTwo;
    public GameObject[] popAngelsTriggerThree;
    public GameObject[] popAngelsTriggerFour;
    public GameObject[] popAngelsTriggerFive;
    public GameObject[] popAngelsTriggerSix;
    private bool TriggerOneThrowed = false;

    IEnumerator PlayEnemySound()
    {
        AkSoundEngine.PostEvent("play_enemy_bones", this.gameObject);
        yield return new WaitForSeconds(1f);
        AkSoundEngine.PostEvent("vikky_hearthat", this.gameObject);
        Debug.Log("tour");
        while(!TriggerOneThrowed)
        {
            yield return new WaitForSeconds(0.5f);
        }
        AkSoundEngine.PostEvent("stop_enemy_bones", this.gameObject);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playEnemySound()
    {
        Debug.Log("Play sound");
        StartCoroutine("PlayEnemySound");
    }


    public void ActiveAngels(int trigger)
    {
        switch(trigger){
            case 1: InternalAngelsActivation(popAngelsTriggerOne);
                    break;
            case 2: InternalAngelsActivation(popAngelsTriggerTwo);
                break;
            case 3:
                InternalAngelsActivation(popAngelsTriggerTwo);
                break;
            case 4:
                InternalAngelsActivation(popAngelsTriggerFour);
                break;
            case 5:
                InternalAngelsActivation(popAngelsTriggerFive);
                break;
            case 6:
                InternalAngelsActivation(popAngelsTriggerSix);
                break;
        }
    }

    private void InternalAngelsActivation(GameObject[] angels)
    {
        for(int i=0;i<angels.Length;i++)
        {
            angels[i].SetActive(true);
        }
    }
}
