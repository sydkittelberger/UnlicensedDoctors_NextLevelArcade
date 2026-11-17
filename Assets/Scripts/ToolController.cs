using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ToolController : MonoBehaviour
{
    //Private Variables
    [Header("Slide Animation")]
    [SerializeField] private float slideDuration = 0.4f;
    [SerializeField] private float slideInDistance = 15f;
    [SerializeField] private float slideOutDistance = 15f;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float toolRotationAmount = 15f;
    private Vector3 toolRestingPosition;
    private Transform outgoingTool;
    private bool hasDisplayedTool = false;

    [SerializeField] private Transform toolDisplay;
    private SpriteRenderer spriteRenderer;

    //Public Variables
    public List<ToolSO> toolsList;
    public ToolSO currentTool; //Stores current tool's SO to give access to germ positions later
    public Transform germ;
    public Transform incomingGerm;
    

    void Awake()
    {
        //Get Component
        spriteRenderer = toolDisplay.GetComponent<SpriteRenderer>();

        //Use the Tool's Scene Position as its Destination (where it's placed in the editor)
        toolRestingPosition = toolDisplay.localPosition;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public List<ToolSO> randomizeTools(int sequenceLength)
    {
        //Generate Working Sequence
        List<ToolSO> sequence = new List<ToolSO>();
        
        //Ensure There are Tools Available
        if (toolsList == null || toolsList.Count == 0)
        return sequence;

        //Generate List of Available Tools (to avoid repeats)
        List<ToolSO> availableTools = new List<ToolSO>(toolsList);

        //Choose a Random Tool for Each Position in Pattern
        for (int i = 0; i < sequenceLength; i++)
        {
            //Refill After Every Tool Has Been Used
            if (availableTools.Count == 0)
            {
                availableTools = new List<ToolSO>(toolsList);
            }
            
            //Pick a Random Index (amongst available tools)
            int randomIndex = Random.Range(0, availableTools.Count);

            //Extract Tool From Index
            sequence.Add(availableTools[randomIndex]);

            //Remove Tool From AvailableTools List to Avoid Repetition
            availableTools.RemoveAt(randomIndex);
        }

        return sequence;
    }

    public void SetTool(ToolSO tool)
    {
        //Remove Any Previous Exit Visual
        if (outgoingTool != null)
        {
            Destroy(outgoingTool.gameObject);
            outgoingTool = null;
        }

        //Copy the Completed Tool Into a New Game Object Before Changing Its Sprite (so it can animated separately from the incoming one)
        if (hasDisplayedTool)
        {
            GameObject outgoingObject = new GameObject("Outgoing Tool");
            outgoingTool = outgoingObject.transform;
            outgoingTool.SetParent(toolDisplay.parent, false);
            outgoingTool.localPosition = toolDisplay.localPosition;
            outgoingTool.localRotation = toolDisplay.localRotation;
            outgoingTool.localScale = toolDisplay.localScale;
            SpriteRenderer outgoingRenderer = outgoingObject.AddComponent<SpriteRenderer>();
            outgoingRenderer.sprite = spriteRenderer.sprite;
            outgoingRenderer.color = spriteRenderer.color;
            outgoingRenderer.sharedMaterial = spriteRenderer.sharedMaterial;
            outgoingRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
            outgoingRenderer.sortingOrder = spriteRenderer.sortingOrder;
            outgoingRenderer.flipX = spriteRenderer.flipX;
            outgoingRenderer.flipY = spriteRenderer.flipY;

            //Give the Shine Material to Outgoing Tool
            outgoingObject.AddComponent<ToolShineController>();
        }
        
        //Change "Real" Display to New Tool
        currentTool = tool;
        spriteRenderer.sprite = currentTool.toolSprite;
        toolDisplay.localRotation = Quaternion.Euler(0f, 0f, currentTool.rotationAmount);
        toolDisplay.localPosition = toolRestingPosition + Vector3.left * slideInDistance; //Start it to the left of screen
        hasDisplayedTool = true;
    }

    public void SetGermPosition (string actionName, Transform targetGerm = null)
    {
        //Determine Which Germ to Set; If Left Unspecified, Default to the Current Germ
        if (targetGerm == null)
        {
            targetGerm = germ;
        }
        
        SpriteRenderer germSpriteRenderer = targetGerm.GetComponent<SpriteRenderer>();
        
        //Check actionName Against Possible Cases; Depending on Which It Matches, Follow the Code Block Then Return
        //Cases Match the Names Given in Input Actions asset
        switch (actionName)
        {
            case "Up":
            targetGerm.localPosition = currentTool.upPosition; //use localPosition since germ is a child of the tool object
            germSpriteRenderer.sprite = currentTool.germUpSprite;
            break;

            case "Down":
            targetGerm.localPosition = currentTool.downPosition;
            germSpriteRenderer.sprite = currentTool.germDownSprite;
            break;

            case "Left":
            targetGerm.localPosition = currentTool.leftPosition;
            germSpriteRenderer.sprite = currentTool.germLeftSprite;
            break;

            case "Right":
            targetGerm.localPosition = currentTool.rightPosition;
            germSpriteRenderer.sprite = currentTool.germRightSprite;
            break;

            case "B1":
            targetGerm.localPosition = currentTool.b1Position;
            germSpriteRenderer.sprite = currentTool.germB1Sprite;
            break;

            case "B2":
            targetGerm.localPosition = currentTool.b2Position;
            germSpriteRenderer.sprite = currentTool.germB2Sprite;
            break;
        }
    }

    public IEnumerator AnimateToolTransition(bool isFinalTool = false)
    {
        Transform exitingTool;
        if (isFinalTool)
        {
            //Hide Germs (as final tool is cleaned)
            if (germ != null)
            {
                germ.gameObject.SetActive(false);
            }
            if (incomingGerm != null)
            {
                incomingGerm.gameObject.SetActive(false);
            }
            
            //Final Transition Uses Current Tool
            exitingTool = toolDisplay;
        }
        else
        {
            //Normal Transitions Use the Outgoing Copy
            exitingTool = outgoingTool;
        }

        if (exitingTool != null)
        {
            //Wait for Shake Before Sliding
            yield return ShakeTool(exitingTool);

            //Wait for Shine Before Sliding
            ToolShineController toolShine = exitingTool.GetComponent<ToolShineController>();
            if (toolShine != null)
            {
                yield return toolShine.PlayShineAndWait();
            }
        }
        
        //Set Up Positions/Destinations
        Vector3 incomingStart = toolDisplay.localPosition; //Tool starts at the left
        Vector3 outgoingStart = Vector3.zero; //Must declare outside of if statement to be used elsewhere
        Vector3 outgoingEnd = toolRestingPosition + Vector3.right * slideOutDistance;
        if (exitingTool != null)
        {
            outgoingStart = exitingTool.localPosition;
        }

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            //Track Time and Progress
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed/slideDuration);

            //Ease In and Out (start slow, speed up, end slow)
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            //Only Bring Another Tool In During Normal Transitions
            if (!isFinalTool)
            {
                //Slide New Tool In
                toolDisplay.localPosition = Vector3.Lerp(incomingStart, toolRestingPosition, easedProgress);
            }

            //Slide Completed Tool Out Simultaneously
            if (exitingTool != null)
            {
                exitingTool.localPosition = Vector3.Lerp(outgoingStart, outgoingEnd, easedProgress);
            }

            //Pauses Coroutine Until Next Frame
            yield return null;
        }

        //Leave the Final Tool Offscreen
        if (isFinalTool)
        {
            toolDisplay.localPosition = outgoingEnd;
        }
        else
        {
            //Ensure Tool Reaches Destination
            toolDisplay.localPosition = toolRestingPosition;

            //Remove Completed Tool
            if (outgoingTool != null)
            {
                Destroy(outgoingTool.gameObject);
                outgoingTool = null;
            }
        }
    }

    private IEnumerator ShakeTool(Transform toolToShake)
    {
        if (toolToShake == null || shakeDuration <= 0f)
        {
            yield break;
        }
        
        //Store Tool's Original Rotation
        Quaternion startingRotation = toolToShake.localRotation;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            //Track Time and Progress
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed/shakeDuration);

            //Shake Tool
            float angle = Mathf.Sin(progress * Mathf.PI * 2f) * toolRotationAmount; //Essentially rotates between 0, rotation, -rotation
            toolToShake.localRotation = startingRotation * Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }

        //Restore Original Rotation
        toolToShake.localRotation = startingRotation;
    }
}
