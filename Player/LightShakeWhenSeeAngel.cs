using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShakeWhenSeeAngel : MonoBehaviour
{
    public FieldOfView Fov;
    public Shaker Shaker;

	// Use this for initialization
	void Start ()
    {
        if (Fov == null)
            Fov = GetComponentInParent<FieldOfView>();

        if (Shaker == null)
            Shaker = GetComponent<Shaker>();

        if (Shaker == null || Fov == null)
            enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Shaker.SetShake(Fov.VisibleTargets.Count > 0);
	}
}
