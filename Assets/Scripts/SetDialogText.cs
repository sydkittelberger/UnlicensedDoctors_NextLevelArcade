using UnityEngine;
using TMPro;

public class SetDialogText : MonoBehaviour
{
    //Private Variables


    //Public Variables
    public TMP_Text dialogText;
    public GameObject instructions;
    public GameObject textBox;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetDialog(string dialog)
    {
        ShowTextBox();
        
        //Set Dialog
        dialogText.text = dialog;
    }

    public void ShowTextBox()
    {
        textBox.SetActive(true);
        instructions.SetActive(true);
    }

    public void HideTextBox()
    {
        textBox.SetActive(false);
        instructions.SetActive(false);
    }
}
