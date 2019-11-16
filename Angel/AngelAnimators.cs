using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AngelAnimators : MonoBehaviour
{
    public RuntimeAnimatorController[] Animators;

	// Use this for initialization
	void Start ()
    {
        int index = Random.Range(0, Animators.Length);
        GetComponent<Animator>().runtimeAnimatorController = Animators[index];
	}
}
