using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GammaScene : MonoBehaviour
{
    private PlayTestMaster _ptm;

	// Use this for initialization
	void Start ()
    {
        _ptm = GameObject.Find("PlayTestMaster").GetComponent<PlayTestMaster>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetButtonDown("InteractKey") || Input.GetButtonDown("InteractJoy"))
        {
            _ptm.SwitchScene(_ptm.MenuScene);
        }
	}
}
