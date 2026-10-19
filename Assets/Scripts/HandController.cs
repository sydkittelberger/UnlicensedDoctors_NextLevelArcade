using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour
{
    //Private Variables
    [SerializeField] private float moveDuration = 0.1f; 
    [SerializeField] private float pauseAtGerm = 0.03f;
    [SerializeField] private float wipingDuration = 0.05f;
    [SerializeField] private float rotationAmount = 15f;
    private Vector3 restingPos;
    private Quaternion restingRotation; 
    private Coroutine movementRoutine;

    //Public Variables

    void Awake()
    {
        //Set Starting Position of Hand
        restingPos = transform.position; 
        restingRotation = transform.localRotation;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void MoveToGerm(Vector3 germPosition)
    {
        //Interrupt Previous Movement
        if (movementRoutine != null)
        {
            StopCoroutine(movementRoutine);
        }

        //Clear Any Interrupted Wiping Motion
        transform.localRotation = restingRotation;

        movementRoutine = StartCoroutine(CleanGerm(germPosition));
    }

    private IEnumerator CleanGerm(Vector3 targetPos)
    {
        //Move to Germ
        yield return MoveToPosition(targetPos);

        //Stay at Germ for Set Duration
        yield return new WaitForSeconds(pauseAtGerm);

        //Wiping Motion
        yield return WipingMotion(rotationAmount);

        //Return to Resting Position
        yield return MoveToPosition(restingPos);

        movementRoutine = null;
    }

    private IEnumerator MoveToPosition(Vector3 targetPos)
    {
        Vector3 startingPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            //Clamp01 keeps a value between 0 and 1, which works here since progress is acting as a percentage
            float progress = Mathf.Clamp01(elapsedTime/moveDuration);

            transform.position = Vector3.Lerp(startingPos, targetPos, progress);

            yield return null;
        }

        //Ensure It Reaches the Exact Position
        transform.position = targetPos;
    }

    private IEnumerator WipingMotion(float rotation)
    {
        float elapsedTime = 0f;

        while (elapsedTime < wipingDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime/wipingDuration);

            //Tilting
            float angle = Mathf.Sin(progress * Mathf.PI * 2f) * rotation; //Essentially rotates between 0, rotation, -rotation
            transform.localRotation = restingRotation * Quaternion.Euler(0f, 0f, angle); 

            yield return null;
        }

        //Restore Original Rotation
        transform.localRotation = restingRotation;
    }
}
