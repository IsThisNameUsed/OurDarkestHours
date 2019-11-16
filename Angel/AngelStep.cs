using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelStep : MonoBehaviour
{
    public string SoundName = "play_enemy_ftps";

    public void PlayStepSound()
    {
        //Debug.Log("enemy ftps");
        AkSoundEngine.PostEvent(SoundName, this.gameObject);
    }
}
