using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHit : MonoBehaviour
{
    public float FeedbackTime = .6f;
    public Color Color = Color.red;

    private Light _light;
    private Color _basecolor;

	// Use this for initialization
	void Start ()
    {
        _light = GetComponent<Light>();
        _basecolor = _light.color;
	}

    public void Trigger()
    {
        StopAllCoroutines();
        StartCoroutine(ITrigger());
    }

    private IEnumerator ITrigger()
    {
        _light.color = Color;
        yield return new WaitForSeconds(FeedbackTime);
        _light.color = _basecolor;
    }
}
