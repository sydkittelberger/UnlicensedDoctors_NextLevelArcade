using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    //Private Variables
    private Vector3 startingPosition;
    private float shakeAmount; //Current intensity 
    private float startAmount; //Original intensity
    private float shakePercentage; //How far along the shake is
    private float shakeDuration; //Reminaing duration
    private float startDuration; //Original duration 
    private bool isErrorMade = false;

    //Public Variables

    void Start()
    {
        //Find Starting Position of Camera 
        startingPosition = transform.localPosition;
    }

    void Update()
    {
        //
    }

    //Method Called from PatternGameController (for errors) and PatternInput (for lives)
    public void ShakeCamera(float amount, float duration)
    {
        //Set Values
        shakeAmount = amount;
        startAmount = shakeAmount;
        shakeDuration = duration;
        startDuration = shakeDuration;

        //Start Camera Shake (assuming player isn't already making a mistake and thus already shaking the camera)
        if (!isErrorMade)
        {
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        //Prevents Multiple Coroutines
        isErrorMade = true;
        
        while (shakeDuration > 0f)
        {
            //Generate Random Movement (do not affect z)
            Vector3 shakeVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f) * shakeAmount;

            //Fade the Shake Over Time 
            shakePercentage = shakeDuration / startDuration;
            shakeAmount = startAmount * shakePercentage;

            //Reduce Time Left on Shake
            shakeDuration -= Time.deltaTime;

            //Shift Position According to Calculated Shake
            transform.localPosition = startingPosition + shakeVector;

            //Pause Until Next Frame
            yield return null;
        }

        //Reset Position; Allow for New Coroutines
        transform.localPosition = startingPosition;
        isErrorMade = false;
    }
}
