using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency : MonoBehaviour {

    private Renderer rend;
    private GameObject player1;
    private Camera cam;
    private LayerMask layerMask;
    private Color color;

    IEnumerator TransparencyOn(Renderer rend)
    {
        color = rend.material.color;
        Debug.Log("TRANSON");
        while (color.a > 0.0)
        {
            yield return new WaitForSeconds(0.05f);
            color.a = color.a - 0.05f;
            rend.material.color = color;
        } 
    }

    IEnumerator TransparencyOff(Renderer rend)
    {
        Debug.Log("TRANSOff");
        color = rend.material.color;
        while (color.a < 1)
        {
            yield return new WaitForSeconds(0.05f);
            color.a = color.a + 0.05f;
            rend.material.color = color;
        }
    }

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        player1 = GameObject.Find("Player1");
        layerMask = LayerMask.GetMask("Obstacle", "Interactable");
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 dir = player1.transform.position - cam.transform.position;
        float distance = Vector3.Distance(player1.transform.position, cam.transform.position);
        Debug.DrawRay(cam.transform.position, dir, Color.green);
        RaycastHit hit;
        
        if (Physics.Raycast(cam.transform.position, dir, out hit, distance, layerMask))
        {
            Renderer newRend = hit.transform.GetComponent<MeshRenderer>();
            if (newRend != rend && rend != null) StartCoroutine(TransparencyOff(rend));
            rend = newRend;
            StartCoroutine(TransparencyOn(rend));
        }
        else if (rend != null)
        {
            color.a = 1;
            rend.material.color = color;
        }
	}
}
