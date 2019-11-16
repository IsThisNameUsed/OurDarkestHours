using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    public UnityEngine.EventSystems.EventSystem EventSystem;
    public UnityEngine.UI.Selectable Selected;

    public List<Rewired.Player> RInputs;
    private List<bool> _moveFlags;

    public bool ButtonToBack;
    public float InputAcceptation;
    [HideInInspector]
    public MainOptions MainOptions;

    // Use this for initialization
    private void Awake()
    {
        AkSoundEngine.StopAll();
    }
    
    void Start ()
    {
        EventSystem.SetSelectedGameObject(Selected.gameObject);

        RInputs = new List<Rewired.Player>();
        _moveFlags = new List<bool>();
        foreach(Rewired.Player player in Rewired.ReInput.players.GetPlayers())
        {
            try
            {
                RInputs.Add(player);
                _moveFlags.Add(false);
            }
            catch (System.Exception) { }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (ButtonToBack)
            ManageBackToMenu();
        else
        {
            ManageNavigation();
            EventSystem.SetSelectedGameObject(Selected.gameObject);
        }
	}

    private void ManageBackToMenu()
    {
        if (Time.time < InputAcceptation)
            return;

        for (int i = 0; i < RInputs.Count; ++i)
        {
            Rewired.Player RInput = RInputs[i];
            if (RInput.GetButtonDown("Interact"))
            {
                Debug.Log("RInput " + RInput.name + " pressed a button");
                MainOptions.SetPanel(main: true);
                ButtonToBack = false;
                StartCoroutine(ReturnToMenu());
            }
        }
    }

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(.1f);
        SetSelectedElement(Selected);
    }

    private void ManageNavigation()
    {
        for (int i = 0; i < RInputs.Count; ++i)
        {
            Rewired.Player RInput = RInputs[i];
            bool flag = _moveFlags[i];
            if (RInput.GetAxis("MoveV") < -0.2f && !flag)
            {
                _moveFlags[i] = true;
                SetSelectedElement(Selected.navigation.selectOnDown);
            }
            else if (RInput.GetAxis("MoveV") > 0.2f && !flag)
            {
                _moveFlags[i] = true;
                SetSelectedElement(Selected.navigation.selectOnUp);
            }
            else if (Mathf.Abs(RInput.GetAxis("MoveV")) == 0f && flag)
            {
                _moveFlags[i] = false;
            }
        }
    }

    public void SetSelectedElement(Selectable selectable)
    {
        Selected = selectable;
        EventSystem.SetSelectedGameObject(Selected.gameObject);
    }
}
