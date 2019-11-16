using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float MoveSpeed;

    private Rigidbody _rb;
    private Camera _camera;
    private Vector3 _velocity;

	// Use this for initialization
	void Start ()
	{
	    _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Vector3 mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
	    Vector3 rotation = transform.eulerAngles;
	    rotation.x = 0;
	    rotation.z = 0;
	    transform.eulerAngles = rotation;
        _velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * MoveSpeed;
	}

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
    }
}
