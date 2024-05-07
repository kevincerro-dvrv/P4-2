using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public float amplitude;
    public float period;

    private float elapsedTime;

    // Start is called before the first frame update
    void Start() {
        elapsedTime = period / 4;
    }

    // Update is called once per frame
    void  Update() {
        elapsedTime += Time.deltaTime;

        Vector3 newPosition = transform.localPosition;
        newPosition.z = CalculatePosition();
        transform.localPosition = newPosition;
    }

    private float CalculatePosition() {
        float pinPon = Mathf.PingPong(elapsedTime * 2 / period, 1);

        float smoothStep = Mathf.SmoothStep(0, 1, pinPon);

        return (smoothStep-0.5f)*amplitude;
    }
}
