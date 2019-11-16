using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTeleport : MonoBehaviour
{
    public Vector3 Move;
	
    public void Teleport()
    {
        transform.position += Move;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + Move, .5f);
    }
}
