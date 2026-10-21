using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReturnToTitleScreen : MonoBehaviour
{
    //Private Variables

    //Public Variables

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TitleScreen(InputAction.CallbackContext context)
    {
        if (!context.performed)
        return;

        //Find Active Audio Controller
        AudioController audioController = FindAnyObjectByType<AudioController>();

        //Reset Music
        if (audioController != null)
        {
            audioController.ResetMusic();
        }

        //Load Title Screen
        SceneManager.LoadScene("TitleScreen");
    }
}
