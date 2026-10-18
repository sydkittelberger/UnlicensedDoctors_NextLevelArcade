using UnityEngine;
using System.Collections.Generic;

public class ToolController : MonoBehaviour
{
    //Private Variables
    private SpriteRenderer spriteRenderer;

    //Public Variables
    public List<ToolSO> toolsList;
    public ToolSO currentTool; //Stores current tool's SO to give access to germ positions later
    public Transform germ;
    

    void Awake()
    {
        //Get Component
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        //Remember Which Tool is Displayed
        currentTool = tool;
        
        //Display Tool
        spriteRenderer.sprite = currentTool.toolSprite;
    }

    public void SetGermPosition (string actionName)
    {
        //Check actionName Against Possible Cases; Depending on Which It Matches, Follow the Code Block Then Return
        //Cases Match the Names Given in Input Actions asset
        switch (actionName)
        {
            case "Up":
            germ.localPosition = currentTool.upPosition; //use localPosition since germ is a child of the tool object
            break;

            case "Down":
            germ.localPosition = currentTool.downPosition;
            break;

            case "Left":
            germ.localPosition = currentTool.leftPosition;
            break;

            case "Right":
            germ.localPosition = currentTool.rightPosition;
            break;

            case "B1":
            germ.localPosition = currentTool.b1Position;
            break;

            case "B2":
            germ.localPosition = currentTool.b2Position;
            break;
        }
    }
}
