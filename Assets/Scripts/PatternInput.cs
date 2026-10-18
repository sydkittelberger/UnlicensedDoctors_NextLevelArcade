using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PatternInput : MonoBehaviour
{
    //Private Variables
    private PatternGenerator patternGenerator;
    private PatternGameController patternGameController;
    private ToolController toolController;
    private HandController handController;
    private CameraShake cameraShakeController;
    private AudioController audioController;
    private SetDarkeningEffect setDarkeningEffect;
    private bool enableControl = true;
    private bool patternCompleted = false;

    //Public Variables
    public List<InputAction> currentSequence;
    public List<ToolSO> currentTools;
    public int lives = 3; 


    void Start()
    {
        //Find Script References
        patternGenerator = GetComponent<PatternGenerator>();
        patternGameController = GetComponent<PatternGameController>();
        toolController = FindAnyObjectByType<ToolController>();
        handController = FindAnyObjectByType<HandController>();
        cameraShakeController = FindAnyObjectByType<CameraShake>();
        audioController = FindAnyObjectByType<AudioController>();
        setDarkeningEffect = FindAnyObjectByType<SetDarkeningEffect>();

        //Prevent Gameplay Without a Generator or Tool Controller
        if (patternGenerator == null || toolController == null)
        {
            Debug.Log("Pattern Generator or Tool Controller is missing.");
            enableControl = false;
            return;
        }

        //Establish Current Pattern
        currentSequence = patternGenerator.randomActionSequence(10);

        //Display First Action
        patternGameController.UpdatePatternText();

        //Display First Tool
        currentTools = toolController.randomizeTools(patternGameController.numberOfPatterns);
        DisplayNextTool();

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
            //Record Germ's Current Position, Animate Hand Towards its Direction
            Vector3 germPosition = toolController.germ.position; //Using germ.position since the hand is not a parent of the germ
            handController.MoveToGerm(germPosition);
            
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

            //Position Germ for Next Action
            else
            {
                DisplayCurrentGerm();
            }
        }
        else
        {
            Debug.Log("Incorrect Key.");
            patternGameController.ErrorMade();
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

        //Display New Tool
        DisplayNextTool();
    }

    public bool GetPatternCompleted()
    {
        return patternCompleted;
    }

    private void DisplayNextTool()
    {
        if (currentTools == null || currentTools.Count == 0)
        return;

        //Display Next Tool
        toolController.SetTool(currentTools[0]);

        //Remove It From Remaining Tools
        currentTools.RemoveAt(0);

        //Position Germ
        DisplayCurrentGerm();
    }

    private void DisplayCurrentGerm()
    {
        if (currentSequence == null || currentSequence.Count == 0)
        return;

        toolController.SetGermPosition(currentSequence[0].name);
    }

    public void LoseALife()
    {
        lives--;
        patternGameController.UpdateLivesText();

        //Darken the Game
        setDarkeningEffect.IncreaseDarkness();

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            patternGameController.ResetTimer();

            //Trigger Camera Shake
            cameraShakeController.ShakeCamera(0.35f, 0.35f);

            //Update Music
            audioController.IntensifyMusic();
        }
    }

    public void GameOver()
    {
        enableControl = false;

        //Prepare and Save Score Before Moving to Next Scene
        GameManager.manager.score = Mathf.CeilToInt(patternGameController.score);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
