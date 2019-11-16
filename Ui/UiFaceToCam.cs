using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFaceToCam : MonoBehaviour {

    public Camera cam;
    Transform player;

	void Start () {
        player = this.transform.parent;
        cam = Camera.main;
        gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().material.shader = Shader.Find("Custom/UiOnTop");
        Vector3 parentScale = transform.parent.localScale;
        if(transform.parent.localScale.x > 1)
        {
            Vector3 scale = new Vector3(1 / transform.parent.localScale.x, 1 / transform.parent.localScale.y, 1 / transform.parent.localScale.z);
            transform.localScale = scale;
            transform.localPosition = new Vector3(0, 1.5f / transform.parent.localScale.x, 0);
        }

        
    }	
	// Update is called once per frame
	void Update () {
        Quaternion newRotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, -player.transform.rotation.y, transform.rotation.z);
        this.transform.rotation = newRotation;
	}
}
