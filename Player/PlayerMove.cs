using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMove : MonoBehaviour
{
    
    private Rigidbody rb;   
    private Vector3 inputValue;
    private Player playerScript;
    private InputManager inputManager;
    [HideInInspector]
    public bool isWalking;

    [Header("Motion parameters")]
    public float Acceleration;
    public float sprintSpeedMultiplier;
    [Tooltip("used for carrying and backwards to")]
    public float speedInDarkMultiplier;
    public float walkSpeed;
    private float authorizedSpeed;
    
    public Camera cam;
    [Tooltip("Minimum pixel distance between player and screen edge")]
    public float pixelValue;
    public float pixelOffset;

    private Animator _animator;

    ////////Debug
    public float InputXValue;
    public float InputYValue;
    public Vector3 actualVelocity;
    public float sprintInputValue;
    public Vector3 position;
    ///////Debug

    private Rewired.Player RInput;

    /*
    IEnumerator SpeedDown(float targetedSpeed)
    {
        Debug.Log("DOWNSTART");
        float speedVariation = 0.02f;
        while (authorizedSpeed >targetedSpeed)
        {
            yield return new WaitForSeconds(0.01f);
            authorizedSpeed = authorizedSpeed - 0.01f;
            speedVariation -= 0.001f;
        }
    }

    IEnumerator SpeedUp(float targetedSpeed)
    {
        Debug.Log("UPSTART");
        while (authorizedSpeed < targetedSpeed)
        {
            yield return new WaitForSeconds(0.01f);
            authorizedSpeed = authorizedSpeed + 0.01f;
        }
    }*/

    private void Awake()
    {
        playerScript = transform.GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        isWalking = false;
        authorizedSpeed = walkSpeed;
        inputManager = GameObject.Find("GameManager").GetComponent<InputManager>();
        if (name == "Player1")
        {
            RInput = ReInput.players.GetPlayer(inputManager.Player1Index);
        }
        else
        {
            RInput = ReInput.players.GetPlayer(inputManager.Player2Index);
        }
    }

    private void Update()
    {
        if (rb.velocity != Vector3.zero)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
        
        PlayerIsBackwards();
        PlayerStrafeValue();

        //Management of speed
        if (playerScript.IsDown) {
            authorizedSpeed = 0;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            if (rb.constraints == RigidbodyConstraints.FreezeAll) rb.constraints = RigidbodyConstraints.FreezeRotation;
            if (playerScript.isBackward || playerScript.LightSources < 0 || playerScript.objectCarrying != null)
            {
                authorizedSpeed = walkSpeed * speedInDarkMultiplier;
            }
            else if (RInput.GetButton("Sprint"))
            //else if (Input.GetAxis(sprintInput) > 0)
            {
                authorizedSpeed = walkSpeed * sprintSpeedMultiplier;
            }

            else
            {
                authorizedSpeed = walkSpeed;
            }

            inputValue = new Vector3(RInput.GetAxis("MoveH"), 0, RInput.GetAxis("MoveV"));
            //inputValue = new Vector3(Input.GetAxis(horizontalInputName), 0, Input.GetAxis(verticalInputName));

            Vector3 newVelocity;
            if (playerScript.isInteractGate == false && playerScript.IsDown == false)
                rb.velocity += inputValue.normalized * Acceleration * Time.fixedDeltaTime;
            else rb.velocity = Vector3.zero;

            //Stopper le déplacement sur un axe quand l'input est à zéro
            float xVelocity = rb.velocity.x;
            float zVelocity = rb.velocity.z;

            if (RInput.GetAxis("MoveH") == 0)
                xVelocity = 0;
            if (RInput.GetAxisRaw("MoveV") == 0)
                zVelocity = 0;
            /*if (Input.GetAxisRaw(horizontalInputName) == 0)
                xVelocity = 0;
            if (Input.GetAxisRaw(verticalInputName) == 0)
                zVelocity = 0;*/

            newVelocity = new Vector3(xVelocity, rb.velocity.y, zVelocity);
            rb.velocity = newVelocity;


            if (rb.velocity.magnitude > authorizedSpeed)
            {
                float verticalVelocity = rb.velocity.y;
                rb.velocity = rb.velocity.normalized * authorizedSpeed;
                newVelocity = new Vector3(rb.velocity.x, verticalVelocity, rb.velocity.z);
                rb.velocity = newVelocity;
            }
        }
        BlockPlayerInScreen();

        //_animator.SetFloat("Speed", rb.velocity.magnitude / walkSpeed);

        #region Charles Animation

        /*

 float angle = Vector3.SignedAngle(inputValue, transform.forward,transform.up);
 _animator.SetFloat("Speed", RInput.GetAxis("MoveV"));
 _animator.SetBool("Walking", rb.velocity.magnitude > 0.1f);

 _animator.SetFloat("Strafing", RInput.GetAxis("MoveH"));
 _animator.SetBool("Strafe", PlayerStrafeValue() != 0);

 _animator.SetBool("Down", playerScript.IsDown);
 */

        #endregion


        _animator.SetFloat("Speed", playerScript.isBackward ? -1 : 1);
        _animator.SetBool("Walking", rb.velocity.magnitude > 0.1f);

        _animator.SetFloat("Strafing", PlayerStrafeValue());
        _animator.SetBool("Strafe", PlayerStrafeValue() != 0);

        _animator.SetBool("Down", playerScript.IsDown);

        ///////Debug
        //sprintInputValue = Input.GetAxis(sprintInput);

        InputXValue = RInput.GetAxis("MoveH");
        InputYValue = RInput.GetAxis("MoveV");
        actualVelocity = rb.velocity;
        /*InputXValue = Input.GetAxis(horizontalInputName);
        InputYValue = Input.GetAxis(verticalInputName);
        actualVelocity = rb.velocity;*/

        ///////Debug
    }

    private void PlayerIsBackwards()
    {
        if (Vector3.Angle(transform.forward, rb.velocity) > 100 || Vector3.Angle(transform.forward, rb.velocity) < -100) playerScript.isBackward = true;
        else if (rb.velocity == Vector3.zero) return;
        else playerScript.isBackward = false;
    }

    private float PlayerStrafeValue()
    {
        if (rb.velocity.magnitude < .1f)
            return 0;

        float angle = Vector3.Angle(transform.forward, rb.velocity);
        if (angle < 50 || angle > 130)
            return 0;
        else if (angle < 90)
            return +1;
        else
            return -1;
    }

    private void BlockPlayerInScreen()
    {
        if (cam == null)
            cam = Camera.main;

         position = cam.WorldToScreenPoint(rb.position);

        if (position.x < pixelValue)
        {
            if (inputValue.x < 0 || inputValue.x == 0)
            { 
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
                inputValue.x = 0;              
            }
            
        }
        if (position.x > cam.pixelWidth - pixelValue )
        {
            if (inputValue.x > 0 || inputValue.x == 0)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
                inputValue.x = 0;
            }
        }
        if (position.y < pixelValue)
        {
            if (inputValue.z < 0 || inputValue.z == 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
                inputValue.z = 0;
            }
               
        }
        if (position.y > cam.pixelHeight - pixelValue)
        {
            if (inputValue.z > 0 || inputValue.z == 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
                inputValue.z = 0;
            }

        }
    }
}


      
