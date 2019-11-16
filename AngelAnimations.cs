using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AngelAnimations : MonoBehaviour
{
    private float _originalSpeed;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Angel _angel;
    
	// Use this for initialization
	void Start ()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _angel = GetComponent<Angel>();
        _originalSpeed = _agent.speed;

        StartCoroutine(ITestNavMesh());
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        _animator.SetFloat("Speed", _agent.speed / _originalSpeed);
        if (_agent.isOnNavMesh)
            _animator.SetBool("Walking", _agent.remainingDistance > _agent.stoppingDistance);          
        else
            _animator.SetBool("Walking", false);
	}

    private IEnumerator ITestNavMesh()
    {
        yield return new WaitForSeconds(1);
        if (!_agent.isOnNavMesh)
            Debug.LogWarning(name + " is not placed on a navmesh, move it on the map. If it is alrready the case, try rebaking the navmesh or tweaking it.", gameObject);
    }
}
