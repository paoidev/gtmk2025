using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputs inputs;
    [SerializeField] private CharacterController controller;

    [Header("Movement settings")]
    public float baseMoveSpeed = 5f;
    public float gravity = 30f;
    public float jumpHeight = 5f;

    [Header("Stored values")]
    public float verticalVelocity;
    public bool isGrounded = false;
    public bool isSprinting = false;
    public bool isJumping = false;

    void Start()
    {
        inputs = GetComponent<PlayerInputs>();    
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 move = (transform.forward * inputs.MoveInput.y + transform.right * inputs.MoveInput.x);
        move *= baseMoveSpeed;
        move.y = VerticalMovement();

        controller.Move(move * Time.deltaTime);
    }

    private float VerticalMovement()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -3f;
            if (inputs.JumpInput)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * 2 * gravity);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        
        return verticalVelocity;
    }
}
