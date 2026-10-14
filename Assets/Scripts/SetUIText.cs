using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetUIText : MonoBehaviour
{
     //Private Variables
     [SerializeField] private Image[] heartImages;

    //Public Variables
    public TMP_Text patternText;
    public TMP_Text timerText;
    public TMP_Text scoreText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetPattern(string patternInput)
    {
        //Set Text
        patternText.text = patternInput;
    }

    public void SetTimer(string timerInput)
    {
        //Set Timer
        timerText.text = timerInput;
    }

    public void SetLives(int livesInput)
    {
        //Set Lives 
        for (int i = 0; i<heartImages.Length; i++)
        {
            //Show One Heart Per Remaining Life
            if (i < livesInput)
            {
                heartImages[i].enabled = true;
            }
            else
            {
                heartImages[i].enabled = false;
            }
        }
    }

    public void SetScore(string scoreInput)
    {
        //Set Score
        scoreText.text = scoreInput;
    }
}
