using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//Store in a list all enemies visible on screen and targeted in the game by player or environment light (include Flare). 
public class EnemyTargeted : MonoBehaviour {

    public List<Lightable> enemyTargetedByPlayer1 = new List<Lightable>();
    public List<Lightable> enemyTargetedByPlayer2 = new List<Lightable>();
    public List<Lightable> enemyTargetedByEnvironmentLight = new List<Lightable>();
    public List<Lightable> enemyTargetedByAll = new List<Lightable>();

    private bool soundPlayable;
    public float delayToReplaySound = 15;
    private bool coroutinRunning;
    public int updateRate = 10;
    private bool jumpScareIsActive;
    private IEnumerator jumpScareCoroutine;
    

    IEnumerator Timer()
    {
        coroutinRunning = true;
        Debug.Log("reset timer");
        yield return new WaitForSeconds(delayToReplaySound);
        soundPlayable = true;
        coroutinRunning = false;
    }

    IEnumerator FindTargetsCoroutine()
    {
        Debug.Log("start FIND TARGET");
        while (true)
        {
            yield return new WaitForSeconds(1 / updateRate);
            enemyTargetedByAll.Clear();
            enemyTargetedByAll = fusionOfThreeList(enemyTargetedByPlayer1, enemyTargetedByPlayer2, enemyTargetedByEnvironmentLight);
            //Debug.Log("enemy visible" + target.name);
            if (soundPlayable && enemyTargetedByAll.Count > 0)
            {
                Debug.Log("JUMPSCARE !");
                AkSoundEngine.PostEvent("play_jumpscare", gameObject);
                soundPlayable = false;
            }
            if (enemyTargetedByAll.Count == 0 && !soundPlayable && !coroutinRunning)
            {
                IEnumerator timerCoroutine = Timer();
                StartCoroutine(timerCoroutine);
            }
           
        }
    }
    // Use this for initialization
    void Start () {
        
        jumpScareIsActive = true;
        coroutinRunning = false;
        soundPlayable = true;
        jumpScareCoroutine = FindTargetsCoroutine();
        StartCoroutine(jumpScareCoroutine);
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(enemyTargetedByAll.Count);
    }

    //Maj of the three enemyTargeted list, called from FieldOfView
    public void majVisibleTargeted(List<Lightable> VisibleTargets, string actorName)
    {
        List<Lightable> enemyTargetedTemp = new List<Lightable>();
        foreach (Lightable target in VisibleTargets)
        {
            if (target is Enemy)
            {
                if (target.GetComponent<Renderer>().isVisible)
                {
                    enemyTargetedTemp.Add(target);
                }
            }
        }
        if (actorName == "Player1")
            enemyTargetedByPlayer1 = enemyTargetedTemp;
        else if (actorName == "Player2")
            enemyTargetedByPlayer2 = enemyTargetedTemp;
        else
            enemyTargetedByEnvironmentLight = enemyTargetedTemp;
    }

    private List<Lightable> fusionOfThreeList(List<Lightable> list1, List<Lightable> list2, List<Lightable> list3)
    {
        List<Lightable> fusion = new List<Lightable>();
        foreach (Lightable elt in list1)
        {
            // Check if this is an unactivated angel
            if (elt.name.ToLower().Contains("angel"))
            {
                Angel angel = elt.GetComponent<Angel>();
                if (angel != null && !angel.Activated)
                    continue;
            }
            fusion.Add(elt);
        }

        foreach (Lightable elt in list2)
        {
            if (!fusion.Contains(elt))
            {
                // Check if this is an unactivated angel
                if (elt.name.ToLower().Contains("angel"))
                {
                    Angel angel = elt.GetComponent<Angel>();
                    if (angel != null && !angel.Activated)
                        continue;
                }
                fusion.Add(elt);
            }
        }

        foreach (Lightable elt in list3)
        {
            if (!fusion.Contains(elt))
            {
                // Check if this is an unactivated angel
                if (elt.name.ToLower().Contains("angel"))
                {
                    Angel angel = elt.GetComponent<Angel>();
                    if (angel != null && !angel.Activated)
                        continue;
                }
                fusion.Add(elt);
            }
        }
        return fusion;
    }

    public void disabledJumpScare()
    {
        StopCoroutine(jumpScareCoroutine);
    }

    public void activJumpScare()
    {
        StartCoroutine(jumpScareCoroutine);
    }

    /* private List<Lightable> FusionOfThreeList(params List<Lightable>[] lists)
     {
         List<Lightable> fusion = new List<Lightable>();
         foreach (List<Lightable> list in lists)
         {
             foreach (Lightable lightable in list)
             {
                 // Check if this is an unactivated angel
                 if (lightable.name.ToLower().Contains("angel"))
                 {
                     Angel angel = lightable.GetComponent<Angel>();
                     if (angel != null && !angel.Activated)
                         continue;
                 }
                 fusion.Add(lightable);
             }
         }
         return null;
     }*/

}
