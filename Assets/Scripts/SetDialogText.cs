using UnityEngine;
using TMPro;

public class SetDialogText : MonoBehaviour
{
    //Private Variables


    //Public Variables
    public TMP_Text dialogText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetDialog(string dialog)
    {
        //Set Dialog
        dialogText.text = dialog;
    }
}
