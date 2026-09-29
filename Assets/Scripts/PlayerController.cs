using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Private Variables
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private bool enableControl = true;
    private Vector2 moveDirection;
    private float tiltAxis;

    //Public Variables
    public GameObject tool; 
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float percentage = 0;
    public float increasePercentagePerSecond = 5f;
    public SetPercentageText percentageDisplay;


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

        //Allow Tool to Rotate
        float rotationValue = tiltAxis * rotationSpeed * Time.fixedDeltaTime;
        tool.transform.localRotation *= Quaternion.Euler(0f, 0f, rotationValue);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tool"))
        {
            //Increase Percentage; Cap it at 100%
            percentage = Mathf.Min(100f, percentage + increasePercentagePerSecond * Time.fixedDeltaTime);

            //Display Percentage
            percentageDisplay.SetPercentage(percentage);
        }
    }

    public void MovePressed(InputAction.CallbackContext aContext)
    {
        //If Player is Not Allowed to Move, Return
        if (!enableControl) return;
        
        //Otherwise, Read Player Input
        moveDirection = aContext.ReadValue<Vector2>();
    }

    public void OnTilt(InputAction.CallbackContext aContext)
    {
        //If Player is Not Allowed to Move, Return
        if (!enableControl) return;

        //Otherwise, Read Player Input (will retun either +1 or -1)
        tiltAxis = aContext.ReadValue<float>();
    }
}
