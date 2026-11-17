using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    //Private Variables

    //Public Variables
    public static BackgroundMusic instance;

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

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
