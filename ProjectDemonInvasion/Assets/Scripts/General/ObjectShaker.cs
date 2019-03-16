using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShaker : MonoBehaviour {

    public float shakeTime;
    public float shakeMagnitude;
    Vector3 startPosition;

    public void ShakeIt()
    {
        startPosition = this.transform.position;
        InvokeRepeating("StartObjectShake", 0f, 0.005f);
        Invoke("StopObjectShake", shakeTime);
    }

    void StartObjectShake()
    {
        float shakeOffsetx = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float shakeOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        Vector3 relativePosition = startPosition;
        relativePosition.x += shakeOffsetx;
        relativePosition.y += shakeOffsetY;
        this.transform.position = relativePosition;
    }

    void StopObjectShake()
    {
        CancelInvoke("StartObjectShake");
        this.transform.position= startPosition;
    }
}
