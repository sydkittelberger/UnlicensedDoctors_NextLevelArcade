using UnityEngine;
using System.Collections.Generic;

public class ToolController : MonoBehaviour
{
    //Private Variables
    private SpriteRenderer spriteRenderer;

    //Public Variables
    public List<Sprite> toolsList;
    

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

    public List<Sprite> randomizeTools(int sequenceLength)
    {
        //Generate Working Sequence
        List<Sprite> sequence = new List<Sprite>();
        
        //Ensure There are Tools Available
        if (toolsList == null || toolsList.Count == 0)
        return sequence;

        //Generate List of Available Tools (to avoid repeats)
        List<Sprite> availableTools = new List<Sprite>(toolsList);

        //Choose a Random Tool for Each Position in Pattern
        for (int i = 0; i < sequenceLength; i++)
        {
            //Refill After Every Tool Has Been Used
            if (availableTools.Count == 0)
            {
                availableTools = new List<Sprite>(toolsList);
            }
            
            //Pick a Random Index (amongst available tools)
            int randomIndex = Random.Range(0, availableTools.Count);

            //Extract Tool From Index
            sequence.Add(toolsList[randomIndex]);

            //Remove Tool From AvailableTools List to Avoid Repetition
            availableTools.RemoveAt(randomIndex);
        }

        return sequence;
    }

    public void SetTool(Sprite toolSprite)
    {
        //Display Tool
        spriteRenderer.sprite = toolSprite;
    }
}
