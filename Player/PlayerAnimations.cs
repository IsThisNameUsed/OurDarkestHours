using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private GameObject player;
 
    private void Start()
    {
        player = transform.parent.parent.gameObject;
    }

    public void PlayStepSound()
    {
        RaycastHit hit;
        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + 0.5f,transform.position.z);
        if(Physics.Raycast(startPosition,Vector3.down, out hit, Mathf.Infinity))
        {
            string tag = hit.transform.gameObject.tag;
            if (tag == "Concrete" || tag == "Untagged")
            {
                AkSoundEngine.SetSwitch("Ground_texture", "concrete", player);
            }
            else if (tag == "Metal") AkSoundEngine.SetSwitch("Ground_texture", "metal", player);
        }
        AkSoundEngine.PostEvent("play_ftps", player);
    }
}
