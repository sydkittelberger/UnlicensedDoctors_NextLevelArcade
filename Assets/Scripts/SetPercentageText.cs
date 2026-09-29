using UnityEngine;
using TMPro;

public class SetPercentageText : MonoBehaviour
{
    //Private Variables


    //Public Variables
    public TMP_Text percentageText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetPercentage(float percentageVal)
    {
        //Set Text
        percentageText.text = Mathf.FloorToInt(percentageVal) + "%";
    }
}
