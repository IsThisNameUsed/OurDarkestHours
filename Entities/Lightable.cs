using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightable : MonoBehaviour
{
    [SerializeField]
    internal int LightSources;

    public float RayTargetHeight;
    public Vector3 RayTargetPoint
    {
        get
        {
            Vector3 pos = transform.position;
            pos.y += RayTargetHeight;
            return pos;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(RayTargetPoint, 0.2f);
    }
}
