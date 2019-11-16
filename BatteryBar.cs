using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBar : MonoBehaviour {

    public FlashlightBattery _battery;
    private Image powerBar;
    public Transform Canvas;

    void Start ()
    {
        if (powerBar == null && Canvas != null)
        {
            if (name == "Player1")
            {
                Transform tr = Canvas.Find("Johnson_UI");
                powerBar = tr.Find("battery_johnson").GetComponent<Image>();
            }
                
            else
                powerBar = Canvas.Find("Victoria_UI").Find("battery_victoria").GetComponent<Image>();
        }

        if (powerBar == null)
        {
            this.enabled = false;
            return;
        }
        powerBar.fillAmount = 100;
        _battery = GetComponent<FlashlightBattery>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (powerBar != null)
            powerBar.fillAmount = _battery.Percentage;
    }
}
