using UnityEngine;
using TMPro;

public class SetUIText : MonoBehaviour
{
     //Private Variables


    //Public Variables
    public TMP_Text patternText;
    public TMP_Text timerText;

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
}
