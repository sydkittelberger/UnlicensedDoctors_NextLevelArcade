using UnityEngine;
using TMPro;

public class SetPatternText : MonoBehaviour
{
     //Private Variables


    //Public Variables
    public TMP_Text patternText;

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
}
