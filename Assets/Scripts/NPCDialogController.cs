using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogController : MonoBehaviour
{
    //Public Variables
    public NPCDialogSO dialog;

    public void InteractPressed(InputAction.CallbackContext aContext)
    {
        if (aContext.phase != InputActionPhase.Performed) return;

        //Find the Dialog Manager
        NPCDialogManager manager = NPCDialogManager.instance;

        if (manager == null)
        {
            Debug.LogWarning("NPC Dialog Manager is missing.");
            return;
        }

        //If Dialog is Running, Progress It
        if (manager.IsDialogRunning())
        {
            manager.ProgressDialog();
        }
        else
        {
            //Otherwise, Begin the Assigned Dialog
            manager.PlayDialog(dialog);
        }
    }
}