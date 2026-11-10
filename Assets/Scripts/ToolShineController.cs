using UnityEngine;
using System.Collections;

public class ToolShineController : MonoBehaviour
{
    //Private Variables
    [SerializeField] private float shineDuration = 0.8f;
    private SpriteRenderer spriteRenderer;
    private Material shineMaterial;
    private Coroutine shineCoroutine;
    private static readonly int shinePositionID = Shader.PropertyToID("_ShinePosition");
    private static readonly int shineMinXID = Shader.PropertyToID("_ShineMinX");
    private static readonly int shineMaxXID = Shader.PropertyToID("_ShineMaxX");
    private static readonly int shineEnabledID = Shader.PropertyToID("_ShineEnabled");

    //Public Variables

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Create Separate Material Instance
        shineMaterial = new Material(spriteRenderer.sharedMaterial);
        spriteRenderer.material = shineMaterial;

        ResetShine();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayShine()
    {
        //Restart if Shine is Already Playing
        if (shineCoroutine != null)
        {
            StopCoroutine(shineCoroutine);
        }

        //Get the Tool's Left and Right Edges After Rotation + Scaling
        Bounds toolBounds = spriteRenderer.bounds;
        float minX = toolBounds.min.x;
        float maxX = Mathf.Max(toolBounds.max.x, minX + 0.001f); //Comparison is just a safeguard
        shineMaterial.SetFloat(shineMinXID, minX);
        shineMaterial.SetFloat(shineMaxXID, maxX);

        shineCoroutine = StartCoroutine(ShineRoutine());
    }

    public void ResetShine()
    {
        if (shineMaterial != null)
        {
            //Completely Hide Shine
            shineMaterial.SetFloat(shineEnabledID, 0f);
            
            //Reset Its Starting Position
            shineMaterial.SetFloat(shinePositionID, -0.5f);
        }
    }

    public void StopShine()
    {
        if (shineCoroutine != null)
        {
            StopCoroutine(shineCoroutine);
            shineCoroutine = null;
        }

        ResetShine();
    }

    void OnDisable()
    {
        StopShine();
    }

    void OnDestroy()
    {
        if (shineMaterial != null)
        {
            Destroy(shineMaterial);
        }
    }

    private IEnumerator ShineRoutine()
    {
        //Display Shine
        shineMaterial.SetFloat(shineEnabledID, 1f);
        
        float elapsed = 0f;
        float duration = Mathf.Max(0.01f, shineDuration);

        while (elapsed < duration)
        {
            float progress = elapsed/duration;
            float position = Mathf.Lerp(-0.5f, 1.8f, progress); //-0.5 is outside of the tool on one side, 1.8 is outside of the tool on the other

            shineMaterial.SetFloat(shinePositionID, position);

            elapsed += Time.deltaTime;
            yield return null;
        }

        ResetShine();
        shineCoroutine = null;
    }

    public IEnumerator PlayShineAndWait()
    {
        PlayShine();

        if (shineCoroutine != null)
        {
            yield return shineCoroutine;
        }
    }
}
