using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalCoopCanvas : MonoBehaviour
{
    public bool OneMoreScreen = false;

    private Dropdown _player1Screen;
    private Dropdown _player2Screen;

    private Cameraman _cameraman;

	// Use this for initialization
	void Start ()
	{
	    _cameraman = Cameraman.Instance;
        
	    _player1Screen = transform.Find("Panel/Content/Player 1 Screen/Dropdown").GetComponent<Dropdown>();
	    _player2Screen = transform.Find("Panel/Content/Player 2 Screen/Dropdown").GetComponent<Dropdown>();

        UpdatePlayerScreens();
	}

    public void UpdatePlayerScreens()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 1; i <= Display.displays.Length; ++i)
        {
            options.Add(new Dropdown.OptionData("Screen " + i));   
        }

        if (OneMoreScreen)
            options.Add(new Dropdown.OptionData("Debug Screen"));

        _player1Screen.options = options;
        _player2Screen.options = options;
    }
    
    public void SelectScreen1(int display)
    {
        _cameraman.Cameras[0].targetDisplay = Convert.ToInt32(display);
        if (!Display.displays[display].active)
            Display.displays[display].Activate();
    }

    public void SelectScreen2(int display)
    {
        _cameraman.Cameras[1].targetDisplay = Convert.ToInt32(display);
        Debug.Log(display);
        if (!Display.displays[display].active)
            Display.displays[display].Activate();
    }
}
