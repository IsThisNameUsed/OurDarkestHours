using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MainOptions : MonoBehaviour
{
    [Space]
    public GameObject GammaPanel;
    public GameObject MainPanel;
    public GameObject CreditPanel;

    [Space]
    public Navigation Navigation;

    [SerializeField]
    private PlayTestMaster _ptm;

    [SerializeField]
    private CameraAnimation _camAnim;

    // Use this for initialization
    void Start ()
    {
        //_ptm = GameObject.Find("PlayTestMaster").GetComponent<PlayTestMaster>();
        Navigation.MainOptions = this;
    }

    private void Update()
    {
        if (_camAnim.isCharacterSelection)
        {
            for (int i = 0; i < Navigation.RInputs.Count; ++i)
            {
                Rewired.Player RInput = Navigation.RInputs[i];

                if (RInput.GetButtonDown("Flare"))
                {
                    Debug.Log(i);
                    _camAnim.changecamera();
                }
                else if (RInput.GetButtonDown(("Interact")))
                {
                    _camAnim.LoadLevel();
                }
           }     
        }
    }

    public void NewGame()
    {
        _camAnim.changecamera();

    }

    public void StartGame()
    {
        SetPanel();
        _ptm.SwitchScene(_ptm.PlayScene);
    }

    public void GammaOptions()
    {
        SetPanel(gamma:true);
        Navigation.ButtonToBack = true;
        Navigation.InputAcceptation = Time.time + 0.5f;
    }

    public void StartCredits()
    {
        SetPanel(credits: true);
        Navigation.ButtonToBack = true;
        Navigation.InputAcceptation = Time.time + 0.5f;

        Animator anim = CreditPanel.GetComponent<Animator>();
        anim.SetTrigger("Display");
        StartCoroutine(BackCredits());
    }

    private IEnumerator BackCredits()
    {
        yield return new WaitForSeconds(59.5f);
        if (CreditPanel.activeSelf)
        {
            SetPanel(main: true);
            Navigation.ButtonToBack = false;
        }
    }

    public void Quit()
    {
        SetPanel();
        Application.Quit();
    }

    public void SetPanel(bool gamma = false, bool main = false, bool credits = false)
    {
        Debug.Log("Set panels (gamma:" + gamma + ", main:" + main + ", credits:" + credits + ")");
        GammaPanel.SetActive(gamma);
        MainPanel.SetActive(main);
        CreditPanel.SetActive(credits);
    }
}
