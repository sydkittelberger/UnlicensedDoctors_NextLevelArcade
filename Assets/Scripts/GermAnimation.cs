using UnityEngine;

public class GermAnimation : MonoBehaviour
{
    //Private Variables
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private float pulseAmount = 0.08f;
    [SerializeField] private float pulsesPerSecond = 1f;
    private Vector3 originalScale;
    private Quaternion originalRotation;
    private float animationTime;

    //Public Variables

    void Awake()
    {
        //Set Original values Based on What's Set in the Inspector
        originalScale = transform.localScale;
        originalRotation = transform.localRotation;
    }

    //Whenever This Script Becomes Active, Perform the Listed Actions
    void OnEnable()
    {
        animationTime = 0f;
    }

    void OnDisable()
    {
        //Reset When Hidden (so next germ starts animation cleanly)
        transform.localScale = originalScale;
        transform.localRotation = originalRotation;
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        //Track Time
        animationTime += Time.deltaTime;

        //Rotate Around the Z Axis
        float angle = rotationSpeed * animationTime;
        transform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, angle);

        //Shrink and Grow 
        float pulse = Mathf.Sin(animationTime * pulsesPerSecond * Mathf.PI * 2f);
        float scaleMultiplier = 1f - pulse * pulseAmount;
        transform.localScale = new Vector3(originalScale.x * scaleMultiplier, originalScale.y * scaleMultiplier, originalScale.z);
    }
}
