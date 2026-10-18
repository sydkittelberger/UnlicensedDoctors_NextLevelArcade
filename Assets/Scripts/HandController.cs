using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour
{
    //Private Variables
    [SerializeField] private float moveDuration = 0.1f; 
    [SerializeField] private float pauseAtGerm = 0.05f;
    private Vector3 restingPos;
    private Coroutine movementRoutine;

    //Public Variables

    void Awake()
    {
        //Set Starting Position of Hand
        restingPos = transform.position; 
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

        movementRoutine = StartCoroutine(CleanGerm(germPosition));
    }

    private IEnumerator CleanGerm(Vector3 targetPos)
    {
        //Move to Germ
        yield return MoveToPosition(targetPos);

        //Stay at Germ for Sry Duration
        yield return new WaitForSeconds(pauseAtGerm);

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

            float progress = Mathf.Clamp01(elapsedTime / moveDuration);

            transform.position = Vector3.Lerp(startingPos, targetPos, progress);

            yield return null;
        }

        //Ensure It Reaches the Exact Position
        transform.position = targetPos;
    }
}
