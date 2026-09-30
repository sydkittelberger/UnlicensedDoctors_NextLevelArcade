using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PatternGenerator : MonoBehaviour
{
    //Private Variables
    [SerializeField] private List<InputActionReference> playerActions; 

    //Public Variables

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Generate the Pattern of Actions
    public List<InputAction> randomActionSequence(int sequenceLength)
    {
        //Generate a New List
        List<InputAction> sequence = new List<InputAction>();
        
        //Ensure There are Actions Available
        if (playerActions == null || playerActions.Count == 0)
        return sequence;

        //Choose a Random Action for Each Position in Pattern
        for (int i = 0; i < sequenceLength; i++)
        {
            //Pick a Random Index
            int randomIndex = Random.Range(0, playerActions.Count);

            //Extract Player Action From Index
            sequence.Add(playerActions[randomIndex].action);
        }

        return sequence;
    }
}
