using UnityEngine;
using UnityEngine.SceneManagement;

public class PatternGameController : MonoBehaviour
{
    //Private Variables
    private PatternInput patternInput;
    private bool timerIsRunning = false;
    private bool isErrorMade = false;

    //Public Variables
    public SetUIText setUIText;
    public int numberOfPatterns = 8;
    public float totalTime = 5f;
    public float timeRemaining;
    public float timeMultiplier = 200f;
    public float errorPenalty = 50f;
    public float lostLifePenalty = 100f;
    public float score = 0;


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
        HandleScore();
        RunTimer();
        UpdateScoreText();
    }

    private void HandleScore()
    {
        if (!timerIsRunning)
        {
           return; 
        }

        if (isErrorMade)
        {
            //Subtract Points; Avoid Negative Values
            score = Mathf.Max(0f, score - errorPenalty);

            //Reset Error Flag
            isErrorMade = false;
        }
        
        if (patternInput.GetPatternCompleted())
        {
            score += timeRemaining * timeMultiplier;
        }
    }

    public void ErrorMade()
    {
        isErrorMade = true;
    }

    private void RunTimer()
    {
        if (timerIsRunning)
        { 
            //Check for Pattern Completion
            if (patternInput.GetPatternCompleted())
            {
                numberOfPatterns--;

                //Limit Amount of Patterns Per Level
                if (numberOfPatterns <= 0)
                {
                    timerIsRunning = false;

                    //Prepare and Save Score Before Moving to Next Scene
                    GameManager.manager.score = Mathf.CeilToInt(score);
                    
                    //Load the Next Scene
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

                    return;

                }
                
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

                    //Subtract Penalty (do before LoseAlife() to ensure score accurately transfers in case of a GameOver())
                    score = Mathf.Max(0f, score - lostLifePenalty);
                    
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
            setUIText.SetPattern("");
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

    public void UpdateScoreText()
    {
        //Convert Float to Int to String
        int myIntScore = Mathf.CeilToInt(score);
        string myStringScore = myIntScore.ToString();

        //Display Text
        setUIText.SetScore(myStringScore);
        
    }
}