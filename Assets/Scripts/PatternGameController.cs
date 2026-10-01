using UnityEngine;
using UnityEngine.SceneManagement;

public class PatternGameController : MonoBehaviour
{
    //Private Variables
    private PatternInput patternInput;
    private bool timerIsRunning = false;

    //Public Variables
    public SetUIText setUIText;
    public float totalTime = 5f;
    public float timeRemaining;


    void Start()
    {
        //Assign Pattern Input
        patternInput = GetComponent<PatternInput>();

        //Prevent Gameplay Without an Input
        if (patternInput == null)
        {
            Debug.Log("Pattern Input is missing.");
            return;
        }

        //Start and Set Time
        timerIsRunning = true;
        timeRemaining = totalTime;
    }

    void Update()
    {
        RunTimer();
    }

    private void RunTimer()
    {
        if (timerIsRunning)
        { 
            //Check for Pattern Completion
            if (patternInput.GetPatternCompleted())
            {
                //Start a New Pattern
                patternInput.StartNewPattern();
                    
                //Reset the Timer
                timeRemaining = totalTime;
                timerIsRunning = true;
            }
            
            else
            {
                timeRemaining -= Time.deltaTime;

                if (timeRemaining <= 0f)
                {
                    timeRemaining = 0f;
                    timerIsRunning = false;

                    //End Game for Player; Transfer to the Next Scene
                    patternInput.GameOver();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }

        //Display Updated Time
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        //Convert Float to Int to String
        int myIntTimer = Mathf.CeilToInt(timeRemaining);
        string myStringTimer = myIntTimer.ToString();
        
        //Display Text
        setUIText.SetTimer(myStringTimer);
    }
}