using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public Vector3 Magnitude;
    private Vector3 _shakeOffset;
    
    public float DefaulShakeTime = 0.6f;

    public bool Shaking;

    public int FramesTimeBetweenShakes = 3;
    private int _shakeIndex;
    
    private void Update()
    {
        transform.position -= _shakeOffset;
    }

    public void LateUpdate()
    {
        if (!Shaking)
        {
            _shakeOffset = Vector3.zero;
            return;
        }

        ++_shakeIndex;
        if (_shakeIndex < FramesTimeBetweenShakes)
            return;
        _shakeIndex = 0;

        _shakeOffset.x = Random.Range(-Magnitude.x, Magnitude.x);
        _shakeOffset.y = Random.Range(-Magnitude.y, Magnitude.y);
        _shakeOffset.z = Random.Range(-Magnitude.z, Magnitude.z);

        transform.position += _shakeOffset;
    }
    
    public void SetShake(bool shake)
    {
        StopAllCoroutines();
        Shaking = shake;
    }

    public void ShakeFor(float time = 0)
    {
        if (time <= 0)
            time = DefaulShakeTime;

        StopAllCoroutines();
        StartCoroutine(IShakeTime(time));
    }

    private IEnumerator IShakeTime(float time)
    {
        Shaking = true;
        yield return new WaitForSeconds(time);
        Shaking = false;
    }
}
