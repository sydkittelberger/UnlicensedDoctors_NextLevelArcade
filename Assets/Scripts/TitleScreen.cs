using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    void Start()
    {
        //
    }

    void Update()
    {
        //
    }

    public void PlayGame()
    {
        //Load Next Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        //Allow Exit of Unity Editor:
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            //Allow Exit of Actual Built Game:
            Application.Quit();
        #endif
    }
}
