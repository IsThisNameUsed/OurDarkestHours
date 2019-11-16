using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGate : Interactable
{

    public AudioSource audioSource;
    public AudioClip closingSound;
    public AudioClip openingSound;

    [Tooltip("Need a key to open apparently")]
    public bool needKey;
    [Tooltip("Name of the key object expected, must be identical to the name of the object himself ")]
    public string keyName;

    private float _smooth = 1.0f;
    private int _DoorOpenAngle = 90;
    private int _DoorCloseAngle = 0;
    private bool _opening;
    private bool _unlocked = true;
    private Quaternion target;
    private Transform pivot;
    private ItemReceiver _itemReceiver;
    


    private void Start()
    {
        _opening = false;
        if (needKey)
        {
            _itemReceiver = gameObject.AddComponent<ItemReceiver>();
            _itemReceiver.expectedItem = keyName;
            _unlocked = false;
        }
        audioSource = gameObject.AddComponent<AudioSource>();
        pivot = transform.GetChild(0);

    }
    void Update()
    {
        /*if (_opening == true)
        {
            var target = Quaternion.Euler(0, _DoorOpenAngle, 0);         
            pivot.transform.localRotation = Quaternion.Slerp(pivot.transform.localRotation, target, Time.deltaTime * _smooth);
        }

        if (_opening == false)
        {
            var target1 = Quaternion.Euler(0, _DoorCloseAngle, 0);           
            pivot.transform.localRotation = Quaternion.Slerp(pivot.transform.localRotation, target1,
            Time.deltaTime * _smooth);
        }*/
        if (pivot.localRotation != target)
        {
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, target, Time.deltaTime * _smooth);
        }
    }

    public void UpdateGate(bool isOpening, string player, Vector3 playerPosition)
    {
        if(!_unlocked)
        {
            bool possessKey = _itemReceiver.Interact(player);
            if (possessKey) _unlocked = true;
            else return;
        }    
        else if (isOpening)
        {
            _opening = !_opening;
            interactGate(playerPosition);
        }
        
    }

    private void interactGate(Vector3 playerPosition)
    {
        if (_opening == true)
        {
            if (openingSound != null)
            {
                audioSource.clip = openingSound;
                audioSource.Play();
            }
            float angle = Vector3.Angle(playerPosition - transform.position, transform.forward);
            //Debug.Log("angle=" + Vector3.Angle(playerPosition - transform.position, transform.forward));
            if (angle > 100)
            {
                target = Quaternion.Euler(0, _DoorOpenAngle, 0);
            }
            else
            {
                target = Quaternion.Euler(0, -_DoorOpenAngle, 0);
            }

            
        }

        if (_opening == false)
        {
            audioSource.clip = closingSound;
            audioSource.PlayDelayed(1.5f);
            target = Quaternion.Euler(0, _DoorCloseAngle, 0);
        }
    }

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        throw new System.NotImplementedException();
    }

    public override void InteractHold(PlayerInteraction playerInteraction)
    {
        throw new System.NotImplementedException();
    }

    public override void InteractStop(PlayerInteraction playerInteraction)
    {
        throw new System.NotImplementedException();
    }
}
