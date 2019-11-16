using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool ManualyOpenable = false;
    public bool CloseWhenDropped = false;
    public bool Locked = false;

    public Vector3 OffsetPosition;
    public Vector3 OffsetRotation;

    public AnimationCurve OpenCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public float OpenSpeed = 1;
    public float CloseSpeed = 1;

    private Vector3 _basePosition;
    private Vector3 _endPosition;
    private Vector3 _baseRotation;
    private Vector3 _endRotation;

    [ReadOnly]
    public bool IsOpening;
    public bool Opening { get; private set; }

    public List<Door> LinkedDoors;

    private float _currentOpeningPercentage;
    private Light light;
    
    void Start ()
    {
        if (LinkedDoors.Count != 0)
        {
            foreach(Door door in LinkedDoors)
                door.RegisterLinkedDoors(LinkedDoors);
        }

        if (!ManualyOpenable) this.gameObject.layer = 0;

        _basePosition = transform.position;
        _endPosition = transform.position + OffsetPosition;

        _baseRotation = transform.eulerAngles;
        _endRotation = transform.eulerAngles + OffsetRotation;
        _currentOpeningPercentage = 0;

        if (gameObject.name == "Door1")
            light = transform.parent.Find("Light").GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // If the door has to be opened and is not locked, and it didn't finished his translation
		if (Opening && !Locked && _currentOpeningPercentage < 1)
        { 
            _currentOpeningPercentage += Time.deltaTime / OpenSpeed;
            if (_currentOpeningPercentage > 1)
            {
                _currentOpeningPercentage = 1;
                if (gameObject.name == "Door1") light.enabled = true;
            }

            float openValue = OpenCurve.Evaluate(_currentOpeningPercentage);
            transform.position = Vector3.Lerp(_basePosition, _endPosition, openValue);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(_baseRotation), Quaternion.Euler(_endRotation), openValue);
        }
        else if (!Opening && _currentOpeningPercentage > 0)
        {
           
            _currentOpeningPercentage -= Time.deltaTime / CloseSpeed;
            if (_currentOpeningPercentage < 0)
            {
                _currentOpeningPercentage = 0;
                if (gameObject.name == "Door1") light.enabled = false;
            }
            float openValue = OpenCurve.Evaluate(_currentOpeningPercentage);
            transform.position = Vector3.Lerp(_basePosition, _endPosition, openValue);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(_baseRotation), Quaternion.Euler(_endRotation), openValue);
        }
	}

    public void Open(bool open = true)
    {
        if (gameObject.name == "Door1")
        {
            if (open && _currentOpeningPercentage == 0)
            {
                AkSoundEngine.PostEvent("play_door_open", gameObject);
                AkGameObj akobj = gameObject.GetComponent<AkGameObj>();
                Destroy(akobj);
            }
            if (!open && (_currentOpeningPercentage == 1))
            {
                AkSoundEngine.PostEvent("play_door_open", gameObject);
                AkGameObj akobj = gameObject.GetComponent<AkGameObj>();
                Destroy(akobj);
            }
            
        }
        
        InternalOpen(open);
    }

    public void Unlock(bool unlocked = true)
    {
        Locked = !unlocked;
        foreach(Door door in LinkedDoors)
            door.Unlock(unlocked);
    }

    public override void ShowInteractFeedbackNormal()
    {
        if (ManualyOpenable)
            base.ShowInteractFeedbackNormal();
    }

    public override void InteractStart(PlayerInteraction playerInteraction)
    {
        if (!ManualyOpenable || Locked)
            return;

        if (CloseWhenDropped)
            InternalOpen(true);
        else
            InternalOpen(!Opening);
    }

    public override void InteractHold(PlayerInteraction playerInteraction)
    {
    }

    public override void InteractStop(PlayerInteraction playerInteraction)
    {
        if (!ManualyOpenable || Locked)
            return;

        if (CloseWhenDropped)
            InternalOpen(false);
        
    }

    public void RegisterLinkedDoors(List<Door> doors)
    {
        foreach(Door door in doors)
        {
            if (!LinkedDoors.Exists(d => d == door))
                LinkedDoors.Add(door);
        }
    }

    private void InternalOpen(bool opened)
    {
        Opening = IsOpening = opened;
        foreach (Door door in LinkedDoors)
            door.Opening = door.IsOpening = opened;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + OffsetPosition, .2f);
    }

}
