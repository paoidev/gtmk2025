using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputs inputs;
    [SerializeField] private CharacterController controller;
    [SerializeField] private CinemachineCamera CMCam;
    [SerializeField] private Camera cam;
    [SerializeField] CinemachineOrbitalFollow orbitalFollow;
    [SerializeField] Animator animator;

    [Header("Movement settings")]
    public float baseMoveSpeed = 5f;
    public float gravity = 30f;
    public float jumpHeight = 5f;
    public float turnSpeed = 10f;

    [Header("Camera settings")]
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    [Header("Stored values")]
    [SerializeField] private Vector3 movementMagnitude;
    public float verticalVelocity;
    public bool isGrounded = false;
    public bool isMoving = false;
    public bool isSprinting = false;
    public bool isJumping = false;

    void Start()
    {
        inputs = GetComponent<PlayerInputs>();    
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        cameraForward = Camera.main.transform.forward;
        cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Move();
    }

    private void Move()
    {
        Vector3 move = (cameraForward * inputs.MoveInput.y + cameraRight * inputs.MoveInput.x);
        move *= baseMoveSpeed;
        move.y = VerticalMovement();

        controller.Move(move * Time.deltaTime);

        #region Debug
        Vector3 horizontalMove = new Vector3(move.x, 0f, move.z);
        isMoving = horizontalMove.magnitude > 0.1f;
        movementMagnitude = move;
        #endregion

        if (isMoving)
        {
            animator.SetBool("run", true);
            Vector3 lookDirection = new Vector3(move.x, 0f, move.z);
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("run", false);
        }
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

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
