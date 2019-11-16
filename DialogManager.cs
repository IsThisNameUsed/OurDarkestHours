using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    private StringDisplayer stringDisplayer;
    [Tooltip("UICanvas/PanelJohn/player1Dialog")]
    public Text player1Text;
    [Tooltip("UICanvas/PanelVicky/player2Dialog")]
    public Text player2Text;
    private DialogResources dialogResources;
    private IEnumerator coroutine;
    [Tooltip("UICanvas/PanelJohn/JohnPortrait")]
    public Image John;
    [Tooltip("UICanvas/PanelVicky/VickyPortrait")]
    public Image Vicky;

    IEnumerator PlayDialog(string[] dialog, int[] dialogOrder)
    {
        Text text;
        float time;
        Image image;
        for(int i = 0; i< dialog.Length; i ++)
        {
            if (dialogOrder[i] == 1) image = John;
            else image = Vicky;
            image.enabled = true;

            text = dialogOrder[i] == 1 ? player1Text : player2Text;
            stringDisplayer.Display(text, dialog[i]);
            time = dialog[i].Length * 0.02f + 2f;
            yield return new WaitForSeconds(time);
            stringDisplayer.StopDisplay(text);
            image.enabled = false;
        }
    }
    // Use this for initialization
    void Start () {
        dialogResources = GetComponent<DialogResources>();
        stringDisplayer = GetComponent<StringDisplayer>();
        

	}
	
    public void startDialogue(string name)
    {
        string[] dialog = dialogResources.getDialogStrings(name);
        int[] dialogOrder = dialogResources.getDialogOrder(name);
        coroutine = PlayDialog(dialog, dialogOrder);
        StartCoroutine(coroutine);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
