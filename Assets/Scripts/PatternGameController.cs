using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PatternGameController : MonoBehaviour
{
    //Private Variables
    private PatternInput patternInput;
    private bool timerIsRunning = false;

    //Public Variables
    public SetUIText setUIText;
    public float totalTime = 5f;
    public float timeRemaining;


    void Awake()
    {
        //Assign Pattern Input
        patternInput = GetComponent<PatternInput>();
    }
    
    void Start()
    {
        //Prevent Gameplay Without an Input
        if (patternInput == null)
        {
            Debug.Log("Pattern Input is missing.");
            return;
        }

        //Start and Set Time
        ResetTimer();
        
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
                ResetTimer();
            }
            
            else
            {
                timeRemaining -= Time.deltaTime;

                if (timeRemaining <= 0f)
                {
                    timeRemaining = 0f;
                    timerIsRunning = false;

                    //Player Loses a Life
                    patternInput.LoseALife();
                }
            }
        }
        //Display Updated Time
        UpdateTimerText();
    }

    public void ResetTimer()
    {
        timeRemaining = totalTime;
        timerIsRunning = true;
    }

    public void UpdatePatternText()
    {
        if (patternInput.currentSequence.Count > 0)
        {
            setUIText.SetPattern(patternInput.currentSequence[0].name);
        }
        else
        {
            setUIText.SetPattern("Pattern Completed!");
        }
    }
    
    private void UpdateTimerText()
    {
        //Convert Float to Int to String
        int myIntTimer = Mathf.CeilToInt(timeRemaining);
        string myStringTimer = myIntTimer.ToString();
        
        //Display Text
        setUIText.SetTimer(myStringTimer);
    }

    public void UpdateLivesText()
    {
        //Convert Int to String
        string myStringLives = patternInput.lives.ToString(); 

        //Display Text
        setUIText.SetLives(myStringLives);
    }
}