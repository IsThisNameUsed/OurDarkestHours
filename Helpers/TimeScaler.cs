using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    [Range(0.01f, 2f)]
    public float TimeScale = 1f;
    
	void Update ()
    {
        Time.timeScale = TimeScale;
	}
}
