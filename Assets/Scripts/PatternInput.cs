using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PatternInput : MonoBehaviour
{
    //Private Variables
    private PatternGenerator patternGenerator;
    private PatternGameController patternGameController;
    private bool enableControl = true;
    private bool patternCompleted = false;

    //Public Variables
    public List<InputAction> currentSequence;
    public int lives = 3; 


    void Start()
    {
        //Find Pattern Generator and Controller
        patternGenerator = GetComponent<PatternGenerator>();
        patternGameController = GetComponent<PatternGameController>();

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
        patternGameController.UpdatePatternText();

        //Display Lives
        patternGameController.UpdateLivesText();
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
        patternGameController.UpdatePatternText();

        //Compare Against First Remaining Action (use IDs to avoid copies of actions in the asset)
        if (patternPressed.id == currentSequence[0].id)
        {
            //Remove Action From List
            currentSequence.RemoveAt(0);

            //Display Next Action
            patternGameController.UpdatePatternText();

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
            LoseALife();
        }
    }

    public void StartNewPattern()
    {
        //Reset Values
        patternCompleted = false;
        enableControl = true;
        
        //Create and Display New Pattern
        currentSequence = patternGenerator.randomActionSequence(10);
        patternGameController.UpdatePatternText();
    }

    public bool GetPatternCompleted()
    {
        return patternCompleted;
    }

    public void LoseALife()
    {
        lives--;
        patternGameController.UpdateLivesText();

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            patternGameController.ResetTimer();
        }
    }

    public void GameOver()
    {
        enableControl = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
