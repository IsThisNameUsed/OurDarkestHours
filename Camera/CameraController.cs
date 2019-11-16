using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player1;
    public GameObject player2;
    public Camera cam;
    
    //Camera settings
    [Header("Camera Settings")]
    [Range(0.0f, 360.0f)]
    public float cameraAngleX;
    [Range(0.0f, 360.0f)]
    public float cameraAngleY;
    [Range(0.0f, 360.0f)]
    public float cameraAngleZ;
    public Vector3 CameraRotation;
    public float cameraOffsetX;
    public float cameraOffsetY;
    public float cameraOffsetZ;
    public float camPosYMax;
    public float camPosYMin;

    private Vector3 velocity;
    private float _velocity;
    private float camTargetPositionX;
    private float camTargetPositionY;
    private float camTargetPositionZ;
    private float camFixedX;
    private float camFixedZ;
    private bool camXIsFixed;
    private bool camZIsFixed;
    private Vector3 camTargetPosition;
    private Vector3 p1ScreenPos;
    private Vector3 p2ScreenPos;
    private bool isZoomIn;
    private bool isZoomOut;

    void Start ()
    {
        //AkSoundEngine.PostEvent("stop_music_menu", gameObject);
        AkSoundEngine.PostEvent("play_amb", gameObject);
        if (player1 == null)
            player1 = GameObject.Find("Player1");
        
        if (player2 == null)
            player2 = GameObject.Find("Player2");

        camXIsFixed = false;
        camZIsFixed = false;
        velocity = Vector3.zero;
        isZoomIn = true;
        isZoomOut = false;
        CameraRotation = new Vector3(cameraAngleX, cameraAngleY, cameraAngleZ);
        transform.eulerAngles = CameraRotation;
    }

    private void Update()
    {
        //réglage angle caméra
        if (transform.rotation.x != cameraAngleX || transform.rotation.z != cameraAngleZ || transform.rotation.y != cameraAngleY)
        {
            CameraRotation = new Vector3(cameraAngleX, cameraAngleY, cameraAngleZ);
            transform.eulerAngles = CameraRotation;
        }
       
        p1ScreenPos = cam.WorldToScreenPoint(player1.transform.position);
        p2ScreenPos = cam.WorldToScreenPoint(player2.transform.position);

        //Distance entre les joueurs en pixels
        //playersDistance = Vector3.Distance(cam.WorldToScreenPoint(player1.transform.position), cam.WorldToScreenPoint(player2.transform.position));
    }

    private void LateUpdate()
    {
        //Determines the X position to block camera if player's distance > limit
        if (camXIsFixed == false)
        {
            if (blockXposition(p1ScreenPos, p2ScreenPos))
            {
                camXIsFixed = true;
                camFixedX = (player1.transform.position.x + player2.transform.position.x) / 2;
            }
        }
        //Unblocked camera if player's distance < limit on X axis
        if (camXIsFixed == true)
        {
            if (!blockXposition(p1ScreenPos, p2ScreenPos))
            {
                camXIsFixed = false;
            }
        }

        //Determines the Z position to block camera if player's distance > limit on Z axis
        if (camZIsFixed == false)
        {
            if (blockZposition(p1ScreenPos, p2ScreenPos))
            {
                camZIsFixed = true;
                camFixedZ = (player1.transform.position.z + player2.transform.position.z) / 2;
            }
        }
        //Unblocked camera if player's distance < limit on Z axis
        if (camZIsFixed == true)
        {
            if (!blockZposition(p1ScreenPos, p2ScreenPos))
            {
                camZIsFixed = false;
            }
        }


        if (camXIsFixed) camTargetPositionX = camFixedX;
        else camTargetPositionX = (player1.transform.position.x + player2.transform.position.x) / 2;

        if (camZIsFixed) camTargetPositionZ = camFixedZ;
        else camTargetPositionZ = (player1.transform.position.z + player2.transform.position.z) / 2;

        /* if (ZoomIn())
         {
             float newZoom = Mathf.SmoothDamp(cameraOffsetY,camPosYMin, ref _velocity, 1f);
             cameraOffsetY = newZoom;
             if (cameraOffsetY < camPosYMin + 0.5f)
             {
                 cameraOffsetY = camPosYMin;
                 isZoomIn = true;
                 isZoomOut = false;
             }
         }
         else if (ZoomOut())
         {
             float newZoom = Mathf.SmoothDamp(cameraOffsetY, camPosYMax, ref _velocity, 1f);
             cameraOffsetY = newZoom;
             if (cameraOffsetY > camPosYMax - 0.5f)
             {
                 cameraOffsetY = camPosYMax;
                 isZoomOut = true;
                 isZoomIn = false;
             }
         }*/
        camTargetPositionY = player1.transform.position.y > player2.transform.position.y ? 
            player1.transform.position.y + cameraOffsetY : player2.transform.position.y + cameraOffsetY;
        camTargetPosition = new Vector3(camTargetPositionX + cameraOffsetX, camTargetPositionY, camTargetPositionZ + cameraOffsetZ);
        transform.position = Vector3.SmoothDamp(transform.position, camTargetPosition, ref velocity,0.5f, 5);
        
    }

    //player's distance (pixels) on X axis
    private bool blockXposition(Vector3 p1ScreenPos, Vector3 p2ScreenPos)
    {
        if ((Math.Abs(p1ScreenPos.x - p2ScreenPos.x) > Screen.width - 400)) return true;
        return false;
    }

    //player's distance (pixels) on Z axis
    private bool blockZposition(Vector3 p1ScreenPos, Vector3 p2ScreenPos)
    {
        if ((Math.Abs(p1ScreenPos.y - p2ScreenPos.y) > Screen.height - 400)) return true;
        return false;
    }

    private bool ZoomIn()
    {
        if (!isZoomOut) return false; 
        else if ((Math.Abs(p1ScreenPos.x - p2ScreenPos.x) < Screen.width - Screen.width/4)) return true; 
        else if ((Math.Abs(p1ScreenPos.y - p2ScreenPos.y) < Screen.height - Screen.height /4)) return true; 
        else return false; 
    }

    private bool ZoomOut()
    {
        if (!isZoomIn) return false;
        else if ((Math.Abs(p1ScreenPos.x - p2ScreenPos.x) > Screen.width - Screen.width/6)) return true;
        else if ((Math.Abs(p1ScreenPos.y - p2ScreenPos.y) > Screen.height - Screen.height /6)) return true;
        else return false;
    }
    

    void OnDrawGizmosSelected()
    {
        Vector3 targetPositionOnFloor = new Vector3(camTargetPosition.x, 0, camTargetPosition.z);
        Debug.DrawLine(transform.position, targetPositionOnFloor, Color.red);

        var camera = GetComponent<Camera>();
        Vector3[] frustumCorners = new Vector3[4];
        camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), camera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);
        for (int i = 0; i < 4; i++)
        {
            var worldSpaceCorner = camera.transform.TransformVector(frustumCorners[i]);
            Debug.DrawRay(camera.transform.position, worldSpaceCorner, Color.cyan);
        }
    }

}
