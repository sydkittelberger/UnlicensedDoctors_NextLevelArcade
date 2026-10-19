using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    //Private Variables
    private static BackgroundMusic instance;

    //Public Variables

    void Awake()
    {
        //Remove Duplicate Music Object(s)
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        //Keep Object Alive Between Scenes
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
