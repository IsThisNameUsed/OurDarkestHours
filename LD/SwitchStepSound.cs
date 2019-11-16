using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStepSound : MonoBehaviour {

    public AudioClip stepSound1;
    public AudioClip stepSound2;

	public void SwitchStepSoundPlayer1()
    {
        GameObject player1 = GameObject.Find("Player1");
        StepSoundPlayer stepSoundPlayer = player1.GetComponent<StepSoundPlayer>();
        stepSoundPlayer.stepSound1 = this.stepSound1;
        stepSoundPlayer.stepSound2 = this.stepSound2;
    }

    public void SwitchStepSoundPlayer2()
    {
        GameObject player2 = GameObject.Find("Player2");
        StepSoundPlayer stepSoundPlayer = player2.GetComponent<StepSoundPlayer>();
        stepSoundPlayer.stepSound1 = this.stepSound1;
        stepSoundPlayer.stepSound2 = this.stepSound2;
    }
}
