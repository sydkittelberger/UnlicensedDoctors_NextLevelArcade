using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NPCDialogManager : MonoBehaviour
{
    //Private Variables
    private Queue<DialogLine> currentLines = new Queue<DialogLine>();
    private DialogLine currentLine;
    private Coroutine typingCoroutine;
    private bool isDialogRunning = false;
    private bool isTyping = false;
    private bool isChangingScene = false;

    //Public Variables
    public static NPCDialogManager instance;
    public SetDialogText setDialogText;
    public float typingSpeed = 0.05f;

    void Awake()
    {
        //Set the Instance Before Input Can Begin
        instance = this;
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public bool IsDialogRunning()
    {
        return isDialogRunning;
    }

    //Starting Dialog
    public void PlayDialog(NPCDialogSO dialog)
    {
        //Prevent Restarting an Active Sequence
        if (isDialogRunning || isChangingScene) return;

        if (dialog == null)
        {
            Debug.Log("Assign a dialog asset to NPCDialogController.");
            return;
        }

        //Reset Queue
        currentLines.Clear();

        //Add the Dialog Lines to the Queue
        foreach (DialogLine eachLine in dialog.dialogLines)
        {
            currentLines.Enqueue(eachLine);
        }

        //Prevent Dequeuing an Empty Queue
        if (currentLines.Count == 0)
        {
            Debug.LogWarning("The dialog asset has no lines.");
            return;
        }

        //Begin Dialog and Display the First Line
        isDialogRunning = true;
        ShowNextLine();
    }

    public void ProgressDialog()
    {
        if (!isDialogRunning || isChangingScene) return;

        //If Text is Still Typing, Reveal the Full Line (allows player to skip effect if desired)
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            isTyping = false;
            setDialogText.SetDialog(currentLine.text);
            return;
        }

        //Otherwise, Advance or Finish
        if (currentLines.Count > 0)
        {
            ShowNextLine();
        }
        else
        {
            EndDialog();
        }
    }

    private void ShowNextLine()
    {
        currentLine = currentLines.Dequeue();
        typingCoroutine = StartCoroutine(ShowTextLine(currentLine));
    }

    private IEnumerator ShowTextLine(DialogLine aLine)
    {
        isTyping = true;
        setDialogText.SetDialog("");

        string text = aLine.text ?? ""; //Use aLine if it isn't null, otherwise use ""; this provides a fallback value to avoid errors
        string currentText = "";

        foreach (char c in text)
        {
            currentText += c;
            setDialogText.SetDialog(currentText);

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private void EndDialog()
    {
        //Prevent Further Input During Scene Changes
        isChangingScene = true;
        
        isDialogRunning = false;
        isTyping = false;
        currentLines.Clear();

        //Clear the Text When the Sequence Ends
        setDialogText.SetDialog("");

        //Load the Next Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}