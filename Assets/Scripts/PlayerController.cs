using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Private Variables
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private bool enableControl = true;
    private Vector2 moveDirection;

    //Public Variables
    public float moveSpeed = 5f;


    void Start()
    {
        //Enable All Actions
        playerInput = GetComponent<PlayerInput>();

        //Find Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    void FixedUpdate()
    {
        //Prevent Player Movement When Disabled
        if (!enableControl)
        {
            return;
        }

        //Allow Player to Move
        rb.linearVelocity = moveDirection.normalized * moveSpeed;
    }

    public void MovePressed(InputAction.CallbackContext aContext)
    {
        //If Player is Not Allowed to Move, Return
        if (!enableControl) return;
        
        //Otherwise, Read Player Input
        moveDirection = aContext.ReadValue<Vector2>();
    }
}
