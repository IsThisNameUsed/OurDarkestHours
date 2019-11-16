using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookMouse : MonoBehaviour
{
    Camera cam;
    public float speed;

    private Player _player;

    private void Start()
    {
        cam = Camera.main;
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (_player.IsDown)
            return;

        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            //Debug.DrawRay(cameraRay.origin, pointToLook-transform.position, Color.red);

            var targetRotation = Quaternion.LookRotation(pointToLook - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            //Debug.DrawRay(cameraRay.origin, pointToLook-transform.position, Color.red);

        }
    }
  
}