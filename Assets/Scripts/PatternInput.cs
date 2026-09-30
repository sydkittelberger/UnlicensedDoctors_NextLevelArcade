using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PatternInput : MonoBehaviour
{
    //Private Variables
    private PatternGenerator patternGenerator;
    private bool enableControl = true;

    //Public Variables
    public List<InputAction> currentSequence;
    public SetPatternText setPatternText;


    void Start()
    {
        //Find Pattern Generator on Game Object
        patternGenerator = GetComponent<PatternGenerator>();

        //Prevent Gameplay Without a Generator
        if (patternGenerator == null)
        {
            enableControl = false;
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
        setPatternText.SetPattern(pattern);

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
            setPatternText.SetPattern(currentSequence[0].name);
        }
        else
        {
            setPatternText.SetPattern("Pattern Completed!");
        }
    }
}
