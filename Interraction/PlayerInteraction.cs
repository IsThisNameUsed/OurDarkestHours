using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interraction avec les portes, peut être étendu à d'autre interractions

public class PlayerInteraction : MonoBehaviour {

    public float interactRange;
    public LayerMask interactableMask;
    private Interactable interactable;

    private InputManager inputManager;
    public Rewired.Player RInput { get; private set; }
    
    public string playerName;

    public bool DebugKeyboard;

    [Tooltip("Max valid angle between player and interactable")] public float interactAngle;

    private Vector3 _testPosition
    {
        get { return transform.position + Vector3.up * 1; }
    }

    [ReadOnly][SerializeField]
    public bool _isInterracting;

    private void Start()
    {
        inputManager = GameObject.Find("GameManager").GetComponent<InputManager>();
        if (name == "Player2")
        {
            RInput = Rewired.ReInput.players.GetPlayer(inputManager.Player2Index);
        }
        else
        {
            RInput = Rewired.ReInput.players.GetPlayer(inputManager.Player1Index);
        }

        playerName = this.gameObject.name;
    }

    void Update()
    {
        //interactable = DetectInteractableWithinReach();
        interactable = DetectNearestInteractable();

        if (interactable == null)
        {
            return;
        }
        else
        {
            interactable.ShowInteractFeedback();
        }

        bool interactingThisFrame = RInput.GetButton("Interact");
        

        if (_isInterracting && interactingThisFrame)
        {
            interactable.InteractHold(this);
        }

        if (!_isInterracting && interactingThisFrame)
        {
            _isInterracting = true;
            interactable.InteractStart(this);
        }
        else if (_isInterracting && !interactingThisFrame)
        {
            _isInterracting = false;
            interactable.InteractStop(this);
        }
        return;
    }

  
    private Interactable DetectNearestInteractable()
    {
        if (interactable != null && Vector3.Distance(interactable.transform.position, _testPosition) > interactRange)
        {
            if (playerName == "Player2")
                interactable.player2AtRange = false;
            else interactable.player1AtRange = false;
            interactable.InteractStop(this);
            if(!interactable.player1AtRange && !interactable.player2AtRange)
                interactable.DestroyInteractFeedback();
            interactable = null;
        }
        Collider[] interactablesInRange = Physics.OverlapSphere(transform.position, interactRange, interactableMask);
        // 1 because there is always our own interactable
        if (interactablesInRange.Length < 1)
        {
            return null;
        }
            

        Collider nearestCollider = null;
        float nearestDistance = float.MaxValue;
        foreach(Collider collider in interactablesInRange)
        {
            // Prevent to check our interaction
            if (collider.transform == transform)
                continue;
            
            float distance = Vector3.Distance(collider.transform.position, _testPosition);
            // Double check interact distance to check is the object center is really in range
            if (distance < interactRange && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCollider = collider;
            }
        }

        // No element really in range
        if (nearestCollider == null)
        {
            return null;
        }
            

        Interactable newInteractable = nearestCollider.GetComponent<Interactable>();
        if (newInteractable == null)
        {
            return null;
        }
           
        // TODO add angle test

        newInteractable.PlayerAtRange(playerName, true);

        if (interactable != null && interactable != newInteractable)
        {
            interactable.DestroyInteractFeedback();
            interactable.PlayerAtRange(playerName, false);
        }
        return newInteractable;
    }

    //unused for the moment

    private Interactable DetectInteractableWithinReach()
    {
        Collider[] interactableInRange = Physics.OverlapSphere(transform.position, interactRange, interactableMask);
        for (int i = 0; i < interactableInRange.Length; i++)
        {
            Interactable newInteractable = interactableInRange[i].GetComponent<Interactable>();

            if (newInteractable.transform.parent != null && newInteractable.transform.parent.name == playerName)
                newInteractable = null;

            if (newInteractable != null)
            {
                if (newInteractable != interactable)
                {
                    if (interactable != null)
                    {
                        interactable.DestroyInteractFeedback();
                        interactable.PlayerAtRange(playerName, false);
                    }
                    newInteractable.PlayerAtRange(playerName, true);
                    return newInteractable;
                }
                else return interactable;
            }
            else continue;
        }
        if (interactable != null)
        {
            interactable.DestroyInteractFeedback();
            interactable.PlayerAtRange(playerName, false);
        }
        return null;
    }


    private bool IsInFrontOf(Interactable interactable)
    {
        Vector3 playerPosition = new Vector3(transform.position.x,1, transform.position.z);
        Vector3 interactablePosition = new Vector3(interactable.transform.position.x,1, interactable.transform.position.z);

        Vector3 dir = interactablePosition - playerPosition;

        Debug.DrawRay(transform.position,dir, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        float angle = Vector3.Angle(dir, transform.forward);
        if (angle > interactAngle) return false;
        else return true;
    }
}
