using UnityEngine;
using UnityEngine.UI;

public class SetDarkeningEffect : MonoBehaviour
{
    //Private Variables
    [SerializeField] private float darknessPerLifeLost = 0.2f;
    [SerializeField] private float maximumDarkness = 0.6f;
    [SerializeField] private float currentDarkness = 0f;

    //Public Variables
    public Image darkeningEffect;

    void Awake()
    {
        //Start With No Darkness
        SetDarkness(0f);
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void IncreaseDarkness()
    {
        SetDarkness(currentDarkness + darknessPerLifeLost);
    }

    private void SetDarkness(float darkness)
    {
        currentDarkness = Mathf.Clamp(darkness, 0f, maximumDarkness);

        Color color = darkeningEffect.color;
        color.a = currentDarkness;
        darkeningEffect.color = color;
    }
}
