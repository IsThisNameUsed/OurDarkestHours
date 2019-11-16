using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    private Animator cameraAnimator;
    public bool isCharacterSelection = false;
    public GameObject canvasMenu;
    public GameObject canvasChara;
    public MainOptions mainOpt;


    private void Start()
    {
        cameraAnimator = gameObject.GetComponent<Animator>();
    }

    public void stateCanvasMenu()
    {
        if (!isCharacterSelection)
            canvasMenu.SetActive(false);
        else
        {
            canvasMenu.SetActive(true);
            isCharacterSelection = false;
        }
    }

    public void stateCanvasChara()
    {
        if (isCharacterSelection)
            canvasChara.SetActive(false);
        else
        {
            canvasChara.SetActive(true);
            isCharacterSelection = true;
        }
            
    }

    public void changecamera()
    {
        if (!isCharacterSelection)
        {
            cameraAnimator.SetBool("chara", true);
            cameraAnimator.SetBool("menu", false);
        }
        else
        {
            cameraAnimator.SetBool("chara", false);
            cameraAnimator.SetBool("menu", true);
        }
    }

    public void LoadLevel()
    {
        cameraAnimator.SetBool("chara", false);
        cameraAnimator.SetBool("startGame", true);
    }

    public void StartGame()
    {
        mainOpt.StartGame();
    }
}
