using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Angel : Enemy
{
    public float BonesTime = 2f;
    public float RefreshRate = 5;
    public float TimeToSpeedBack = 1f;
    [Range(0.01f, 10)] public float SpeedRecoverMultiplier = 2f;
    public float SearchDistance = 10f;
    public float LoseDistance = 15f;
    private float _nextSetDestination;
    public float AttackRange = .8f;
    public float AttackCooldown = 1f;
    public float SpeedDecreasePerLightLevel = 7f;
	
	private NavMeshAgent _agent;
	private float _originalSpeed;
    private float _originalAngularSpeed;
    private float _nextGo;
    private float _timeExposed;
    private float _nextAttack;
    private Rigidbody _rb;
    private bool _enemyBonesIsPlaying;

    public bool Activated = true;

    private Animator _animator;

    // Use this for initialization
    void Start ()
	{
        _enemyBonesIsPlaying = false;
        _rb = GetComponent<Rigidbody>();
		_agent = GetComponent<NavMeshAgent>();
        _originalSpeed = _agent.speed;
        _originalAngularSpeed = _agent.angularSpeed;
        _animator = GetComponentInChildren<Animator>();

        StartCoroutine(UpdateTarget(1 / RefreshRate));
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (!Activated)
        {
            _agent.speed = 0;
            _agent.angularSpeed = 0;
            _rb.velocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            return;
        }
        else
            _rb.constraints = RigidbodyConstraints.None;
        
	    float time = Time.time;

        if (time > _nextSetDestination && Target != null)
        {
            _nextSetDestination = Time.time + .5f;
            _agent.SetDestination(Target.transform.position);
        }

        // Stop following if too far
	    if (Target != null && Vector3.Distance(Target.transform.position, transform.position) > LoseDistance)
	    {
            LoseFocus();
	    }
        else if (LightSources == 0 && _agent.speed != _originalSpeed && time > _nextGo && time > _nextAttack)
        {
            if (_enemyBonesIsPlaying == false)
            {
                StartCoroutine(PlayBones());
            }

            _agent.speed = _originalSpeed;
            _agent.angularSpeed = _originalAngularSpeed;
            _timeExposed = 0;
           
        }
        else if (LightSources > 0)
        {
            if (_enemyBonesIsPlaying == true)
            {
                AkSoundEngine.PostEvent("stop_enemy_bones", gameObject);
                _enemyBonesIsPlaying = false;
            }

            float speed = _originalSpeed - (LightSources * SpeedDecreasePerLightLevel);
            if (speed <= 0)
            {
                speed = 0;
                _rb.velocity = Vector3.zero;
            }
            
            _agent.speed = speed;
            //_agent.angularSpeed = _originalAngularSpeed * (_agent.speed / _originalSpeed);
            _agent.angularSpeed = 1;
            _timeExposed += Time.deltaTime;
            float quickRecoverTime = _timeExposed / SpeedRecoverMultiplier;

            if (quickRecoverTime < TimeToSpeedBack)
                _nextGo = Time.time + quickRecoverTime;
            else
                _nextGo = Time.time + TimeToSpeedBack;
        }

        if (Target != null && _agent.speed > 0 && time > _nextAttack)
        //else if (Target != null && LightSources == 0 && _agent.speed > 0 && time > _nextAttack)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) < AttackRange && !Target.IsDown && !Target.IsDead)
            {
                if (_enemyBonesIsPlaying)
                    AkSoundEngine.PostEvent("stop_enemy_bones", this.gameObject);

                AkSoundEngine.PostEvent("play_enemy_attack", this.gameObject);
                
                _nextAttack = time + AttackCooldown;

                Target.Attack(AttackPower);
                _animator.SetTrigger("Attack");
            }
        }
	}

    void FixedUpdate()
    {
        _rb.constraints = LightSources > 0 ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
        _rb.velocity = Vector3.zero;
    }

    IEnumerator PlayBones()
    {
        _enemyBonesIsPlaying = true;
        AkSoundEngine.PostEvent("play_enemy_bones", gameObject);
        yield return new WaitForSeconds(BonesTime);
        AkSoundEngine.PostEvent("stop_enemy_bones", gameObject);
    }

    IEnumerator UpdateTarget(float wait)
    {
        while (true)
        {
            FindTarget();
            //GoToTarget();
            FocusOn(Target);

            yield return new WaitForSeconds(wait);
        }
    }

	void FindTarget()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		Transform minDistancePlayer = null;
		float minDistance = float.MaxValue;

		foreach (GameObject player in players)
		{
		    Player pl = player.GetComponent<Player>();
		    if (pl == null || pl.IsDead || pl.IsDown)
		        continue;

			float distance = Vector3.Distance(player.transform.position, transform.position);
			if (distance < minDistance)
			{
				minDistancePlayer = player.transform;
				minDistance = distance;
			}
		}

	    if (minDistance > SearchDistance && Target == null)
	    {
	        LoseFocus();
            //Debug.Log("Lost focus because no player in sight");
	    }
	    else if (minDistancePlayer != null)
	    {
	        Target = minDistancePlayer.GetComponent<Player>();
	        //FocusOn(Target);
        }
	}
    
    public override void FocusOn(Player target)
    {
        if (Target == null)
            return;

        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(target.transform.position, path);
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.Log("Can't go to target");
            LoseFocus();
        }
        else
        {
            Target = target;
            if (Time.time > _nextSetDestination)
            {
                _nextSetDestination = Time.time + .5f;
                _agent.SetDestination(Target.transform.position);
            }
        }
    }

    public override void LoseFocus()
    {
        Target = null;
        if (_agent.isOnNavMesh)
            _agent.SetDestination(transform.position);
    }

    public void Activate(bool activated)
    {
        _agent.speed = activated ? _originalSpeed : 0;
        _agent.angularSpeed = activated ? _originalAngularSpeed : 0;
        Activated = activated;

        if (activated)
            LoseFocus();
    }

    public void Show(bool showed)
    {
        transform.Find("Body").GetComponent<MeshRenderer>().enabled = showed;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SearchDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, LoseDistance);
    }
}
