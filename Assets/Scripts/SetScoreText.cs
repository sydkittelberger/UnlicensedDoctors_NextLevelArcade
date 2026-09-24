using UnityEngine;
using TMPro;

public class SetScoreText : MonoBehaviour
{
    //Private Variables
    
    //Public Variables
    public TMP_Text initials, score;

    public void SetText(string _initials, string _score)
    {
        initials.text = _initials;
        score.text = _score;
    }

}
