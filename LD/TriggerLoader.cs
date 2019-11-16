using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLoader : MonoBehaviour
{
    public GameObject[] ObjectsToDisable;
    public GameObject[] ObjectToEnable;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach(GameObject obj in ObjectsToDisable)
            {
                obj.SetActive(false);
            }

            foreach(GameObject obj in ObjectToEnable)
            {
                obj.SetActive(true);
            }

            enabled = false;
        }
    }
}
