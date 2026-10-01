using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PatternInput : MonoBehaviour
{
    //Private Variables
    private PatternGenerator patternGenerator;
    private bool enableControl = true;
    private bool patternCompleted = false;

    //Public Variables
    public List<InputAction> currentSequence;
    public SetUIText setUIText;


    void Start()
    {
        //Find Pattern Generator on Game Object
        patternGenerator = GetComponent<PatternGenerator>();

        //Prevent Gameplay Without a Generator
        if (patternGenerator == null)
        {
            Debug.Log("Pattern Generator is missing.");
            enableControl = false;
            return;
        }

        //Establish Current Pattern
        currentSequence = patternGenerator.randomActionSequence(10);

        //Display First Action
        UpdatePatternText();
    }

    void Update()
    {
        //
    }

    public void PatternPressed(InputAction.CallbackContext aContext)
    {
        //If Player is Not Allowed to Move, Return
        if (!enableControl || !aContext.performed) return;
        
        //Ensure There is a Pattern Left to Complete
        if (currentSequence == null || currentSequence.Count == 0) return;

        //Identify Which Action Was Pressed
        InputAction patternPressed = aContext.action;

        //Display Pattern Player Has to Follow
        string pattern = currentSequence[0].name;
        setUIText.SetPattern(pattern);

        //Compare Against First Remaining Action (use IDs to avoid copies of actions in the asset)
        if (patternPressed.id == currentSequence[0].id)
        {
            //Remove Action From List
            currentSequence.RemoveAt(0);

            //Display Next Action
            UpdatePatternText();

            //Check Whether the Entire Pattern Was Completed
            if (currentSequence.Count == 0)
            {
                Debug.Log("Pattern Completed!");
                patternCompleted = true;
            }
        }
        else
        {
            Debug.Log("Incorrect Key.");
        }
    }

    private void UpdatePatternText()
    {
        if (currentSequence.Count > 0)
        {
            setUIText.SetPattern(currentSequence[0].name);
        }
        else
        {
            setUIText.SetPattern("Pattern Completed!");
        }
    }

    public void StartNewPattern()
    {
        //Reset Values
        patternCompleted = false;
        enableControl = true;
        
        //Create and Display New Pattern
        currentSequence = patternGenerator.randomActionSequence(10);
        UpdatePatternText();
    }

    public bool GetPatternCompleted()
    {
        return patternCompleted;
    }

    public void GameOver()
    {
        enableControl = false;
    }
}
