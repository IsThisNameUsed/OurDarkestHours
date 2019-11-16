using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Lightable
{
    public float InvulnerabilityTime = 0.5f;
    public float MaxLife = 12;
    public float RegenTimer = 5f;
    public float HitPointRegenPerSecond = 1f;

    private float _nextAttack;
    private float _startRegenTime;
    [SerializeField] private float _currentLife;

    public AudioClip HurtSound;
    public AudioClip DeathSound;

    public bool IsDown;
    public bool IsDead;
    public bool IsInvincible;


    //interaction
    [HideInInspector]
    public bool isInteractGate;
    public string objectCarrying;
    public GameObject ObjectCarryingRef;
    public bool isBackward;

    private Shaker _camShaker;
    private Rewired.Player RInput;

    //Interface
    public Image aliveToken;
    public Image woundedToken;
    public Image deadToken;

    // Use this for initialization
    void Start ()
    {
        _currentLife = MaxLife;
        _camShaker = Camera.main.GetComponent<Shaker>();
        aliveToken.enabled = true;

        InputManager inputManager = GameObject.Find("GameManager").GetComponent<InputManager>();
        if (name == "Player2")
            RInput = ReInput.players.GetPlayer(inputManager.Player2Index);
        else
            RInput = ReInput.players.GetPlayer(inputManager.Player1Index);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if(isDead) SceneManager.LoadScene("ODH_Fin");
        if (IsDown)
        {
            /*if(_redCross == null)
            {
                Vector3 targetPosition = transform.position;
                targetPosition.y += 3;
                _redCross = Instantiate(redCrossPrefab, targetPosition, new Quaternion(0.8f, 0f, 0f, 1));
            }*/
                
        }
		else if (_currentLife < MaxLife && Time.time > _startRegenTime)
        {
            _currentLife += HitPointRegenPerSecond * Time.deltaTime;
        }
        else if(_currentLife >= MaxLife && !aliveToken.IsActive())
                ActiveToken(aliveToken);


        //if(!IsDown && _redCross != null) Destroy(_redCross, 0.0f);
    }

    internal void Attack (float damage)
    {
        _startRegenTime = Time.time + RegenTimer;

        if (Time.time < _nextAttack || IsDead || IsDown)
            return;

        if (!IsInvincible)
            _currentLife -= damage;
        _nextAttack = Time.time + InvulnerabilityTime;

        _camShaker.ShakeFor();
        GetComponentInChildren<LightHit>().Trigger();// TODO optimize;
        StartCoroutine(IVibrateController(0.6f, 0.6f));
        if(_currentLife < MaxLife)
        {
            ActiveToken(woundedToken);
        }

        AudioClip clip;
        if (_currentLife <= 0)
        {
            _currentLife = 0;
            IsDown = true;
            ActiveToken(deadToken);
            clip = DeathSound;
        }
        else
        {
            clip = HurtSound;
        }

        if (clip != null)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.Play();
            Destroy(source, clip.length);
        }

        // check if lose
        GameObject.Find("PlayTestMaster").GetComponent<PlayTestMaster>().TestLose();
    }

    private IEnumerator IVibrateController(float vibration, float time)
    {
        RInput.SetVibration(0, vibration);
        RInput.SetVibration(1, vibration);

        yield return new WaitForSeconds(time);

        RInput.SetVibration(0, 0, true);
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public void SetInputOneJoystick()
    {
        if (name == "Player1")
            Destroy(GetComponent<PlayerLookJoy>());
        else
            Destroy(GetComponent<PlayerLookMouse>());
    }

    public void SetInputTwoJoystick()
    {
       Destroy(GetComponent<PlayerLookMouse>());
    }
    
    public void ActiveToken(Image token)
    {
        aliveToken.enabled = false;
        woundedToken.enabled = false;
        deadToken.enabled = false;
        token.enabled = true;
    }

}
