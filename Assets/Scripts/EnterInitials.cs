using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EnterInitials : MonoBehaviour
{
    //Private Variables
    private int currentLetterIndex = 0;
    private int currentInitialSlot = 0;
    private bool submitted = false;

    //Public Variables
    public TMP_Text scoretext;
    public int[] alphabetIndex = new int[26];
    public TMP_Text initial1, initial2, initial3;
    public TMP_Text[] initials = new TMP_Text[3];
    

    public void Start()
    {
        //Establish Initials List
        initials[0] = initial1;
        initials[1] = initial2;
        initials[2] = initial3;

        //Convert Score to String
        scoretext.text = GameManager.manager.score.ToString();
    }

    //Edit Letter on Column
    public void EditLetter(float _value)
    {
        alphabetIndex[currentLetterIndex] = (alphabetIndex[currentLetterIndex] + (int)_value + 26) % 26;
        initials[currentInitialSlot].text = ((char)('A' + alphabetIndex[currentLetterIndex])).ToString();
    }

    //Switch Between Columns
    public void MoveSlot(float _value)
    {
        initials[currentInitialSlot].color = Color.white;
        currentInitialSlot = Mathf.Clamp(currentInitialSlot + (int)_value, 0, initials.Length - 1);
        initials[currentInitialSlot].color = Color.red;

        alphabetIndex[currentLetterIndex] = initials[currentInitialSlot].text[0] - 'A';
    }

    //Return Initials
    string GetInitials()
    {
        string name = "";
        foreach (TMP_Text i in initials)
        {
            name += i.text;
        }
        return name;
    }

    //Functions from Player Input Actions
    public void OnUpDown(InputAction.CallbackContext context)
    {
        //Return if There is No Current Input
        if (!context.performed) return;

        //Read Player Input
        float value = context.ReadValue<float>();

        //Change Letters Based on Player Input
        if (value > 0.5f) EditLetter(1);
        else if (value < -0.5f) EditLetter(-1);
    }

    public void OnLeftRight(InputAction.CallbackContext context)
    {
        //Return if There is No Current Input
        if (!context.performed) return;

        //Read Player Input
        float value = context.ReadValue<float>();

        //Change Columns Based on Player Input
        if (value > 0.5f) MoveSlot(1);
        else if (value < -0.5f) MoveSlot(-1);
    }

    public void OnEnter(InputAction.CallbackContext context)
    {
        if (!context.performed || submitted) return;
        
        submitted = true;

        GameManager.manager.initials = GetInitials();
        GameManager.manager.AddNewScore();
        SceneManager.LoadScene("Leaderboard");
    }
}
