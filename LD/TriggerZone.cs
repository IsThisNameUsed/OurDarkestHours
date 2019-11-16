using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public UnityEvent On1PlayerEnter;
    public UnityEvent On2PlayersEnter;
    public UnityEvent OnPlayer1Enter;
    public UnityEvent OnPlayer2Enter;
    public UnityEvent OnEnemyEnter;

    public UnityEvent OnPlayerLeave;
    public UnityEvent OnNoPlayerLeft;

    public bool SingleEvent;
    private bool _triggered = false; // TODO more elegant solution

    [HideInInspector]
    public Collider Collider;

    private List<Transform> _players = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        /*if (_triggered)
            return;*/
        if (other.tag == "Player")
        {
            if (!_players.Contains(other.transform))
            {
                _players.Add(other.transform);
                if (_players.Count == 1)
                {
                    On1PlayerEnter.Invoke();
                    if (SingleEvent)
                    {
                        On1PlayerEnter.RemoveAllListeners();
                        _triggered = true;
                    }
                }
                else if (_players.Count == 2)
                {
                    On2PlayersEnter.Invoke();
                    if (SingleEvent)
                    {
                        On2PlayersEnter.RemoveAllListeners();
                        _triggered = true;
                    }
                } 
            }

            if(other.name == "Player1")
            {
                OnPlayer1Enter.Invoke();
                if (SingleEvent)
                    _triggered = true;
            }

            else if(other.name == "Player2")
            {
                OnPlayer2Enter.Invoke();
                if (SingleEvent)
                    _triggered = true;
            }
        }

        else if (other.GetComponent<Enemy>())
        {
            OnEnemyEnter.Invoke();
            if (SingleEvent)
            {
                OnEnemyEnter.RemoveAllListeners();
                _triggered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _players.Remove(other.transform);

        if (other.tag == "Player")
        {
            OnPlayerLeave.Invoke();
            if (SingleEvent)
            {
                OnPlayerLeave.RemoveAllListeners();
                _triggered = true;
            }

            if (_players.Count == 0)
            {
                OnNoPlayerLeft.Invoke();
                if (SingleEvent)
                {
                    OnNoPlayerLeft.RemoveAllListeners();
                    _triggered = true;
                }
            }

        }
    }
}
