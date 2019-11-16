using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTestMaster : MonoBehaviour
{
    public string GammaScene = "GammaOption";
    public string MenuScene = "ODH";
    public string PlayScene = "VS_lvl";
    public string EndScene = "ODH_Fin";

    public Vector3 Spawn1;
    public Vector3 Spawn2;
    public Vector3 Spawn3;

    private int _spawnTarget;
    private string _sceneTarget;

    private Player _player1;
    private Player _player2;

    private Animator _animator;
    private bool isLoading;
    // Use this for initialization
    void Start ()
    {
        isLoading = false;
        SceneManager.sceneLoaded += OnSceneLoad;

        try
        {
            _player1 = GameObject.Find("Player1").GetComponent<Player>();
            _player2 = GameObject.Find("Player2").GetComponent<Player>();
        }
        catch (Exception e) {}

        _animator = GetComponent<Animator>();
    }

    public void TestLose()
    {
        if (_player1.IsDead || _player2.IsDead || (_player1.IsDown && _player2.IsDown))
        {
            SwitchScene(EndScene);
            _animator.SetFloat("Speed", .2f);
            _animator.SetTrigger("EndScene");
        }
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name == PlayScene)
        {
            Transform players = GameObject.Find("Players").transform;
            switch(_spawnTarget)
            {
                case 1:
                    players.position = Spawn1;
                    break;
                case 2:
                    players.position = Spawn2;
                    break;
                case 3:
                    players.position = Spawn3;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            SceneManager.LoadScene(GammaScene);
        }
		else if (Input.GetKeyDown(KeyCode.F11))
        {
            SceneManager.LoadScene(PlayScene);
        }
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            SceneManager.LoadScene(MenuScene);
        }
        else if (Input.GetKeyDown(KeyCode.F12))
        {
            SceneManager.LoadScene(EndScene);
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            _spawnTarget = 1;
            SceneManager.LoadScene(PlayScene);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            _spawnTarget = 2;
            SceneManager.LoadScene(PlayScene);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            _spawnTarget = 3;
            SceneManager.LoadScene(PlayScene);
        }

        if (_player1 == null || _player2 == null)
            return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            _player1.IsInvincible = true;
            _player2.IsInvincible = true;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            _player1.IsInvincible = false;
            _player2.IsInvincible = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(Spawn1, 1);
        Gizmos.DrawWireSphere(Spawn2, 1);
        Gizmos.DrawWireSphere(Spawn3, 1);
    }

    public void SwitchScene(string name, bool longEnd = false)
    {
        _sceneTarget = name;
        if(SceneManager.GetActiveScene().name != "Vs_lvl Small")
            AkSoundEngine.StopAll();
        if (longEnd)
            _animator.SetTrigger("LongEndScene");
        else
            _animator.SetTrigger("EndScene");
    }

    //Worst code in the world below!! Special load scene for stroboscope (do not stop Wwise event)
    public void SwitchSceneStrobo(string name, bool longEnd = false)
    {
        _sceneTarget = name;
        if (longEnd)
            _animator.SetTrigger("LongEndScene");
        else
            _animator.SetTrigger("EndScene");
    }

    private void TriggerSwitchScene()
    {
        //SceneManager.LoadScene(_sceneTarget);
        if (!isLoading)
        {
            isLoading = true;
            Debug.Log("Loading scene " + _sceneTarget);
            SceneManager.LoadSceneAsync(_sceneTarget);
        }
    }
}
