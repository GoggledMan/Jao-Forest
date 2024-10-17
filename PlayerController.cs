using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Normal movement speed
    public float crouchSpeed = 2.5f; // Movement speed while crouching
    public float sprintSpeed = 8f; // Speed when sprinting
    public float lookSpeed = 2f;
    public float gravity = -9.81f; // Gravity force
    public float jumpHeight = 2f; // Optional jump mechanic
    public float crouchHeightOffset = 0.5f; // Height adjustment when crouching
    public float crouchColliderHeight = 0.5f; // Collider height when crouching
    public float standingColliderHeight = 2f; // Normal collider height

    private Vector3 velocity; // Tracks the player's velocity
    private Vector3 originalCameraPosition; // To store the original camera position

    private CharacterController characterController;
    private Camera playerCamera; // Reference to the player's camera
    private float verticalRotation = 0f;
    private bool isInputLocked = false; // Flag to control input
    private bool isGrounded; // To check if the player is grounded
    private bool isCrouching = false; // Flag for crouching state

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main; // Get the player's camera
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        characterController.height = standingColliderHeight; // Set the initial collider height
        originalCameraPosition = playerCamera.transform.localPosition; // Store the original camera position
    }

    void Update()
    {
        if (isInputLocked)
        {
            // Prevent movement and looking around
            return;
        }

        // Check if the player is grounded
        isGrounded = characterController.isGrounded;

        // Reset velocity.y when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small value to ensure player stays grounded
        }

        // Handle crouching
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            isCrouching = !isCrouching; // Toggle crouching state

            // Adjust camera height based on crouching state
            if (isCrouching)
            {
                playerCamera.transform.localPosition = new Vector3(originalCameraPosition.x, originalCameraPosition.y - crouchHeightOffset, originalCameraPosition.z);
                characterController.height = crouchColliderHeight; // Reduce collider height
            }
            else
            {
                playerCamera.transform.localPosition = originalCameraPosition; // Reset camera position to original
                characterController.height = standingColliderHeight; // Reset collider height
            }
        }

        // Move the player
        float currentSpeed = moveSpeed;

        // Handle sprinting
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && Input.GetAxis("Vertical") > 0)
        {
            currentSpeed = sprintSpeed; // Increase speed while sprinting
        }
        else if (isCrouching)
        {
            currentSpeed = crouchSpeed; // Use crouch speed when crouching
        }

        float moveX = Input.GetAxis("Horizontal") * currentSpeed;
        float moveZ = Input.GetAxis("Vertical") * currentSpeed;
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * Time.deltaTime);

        // Jumping logic
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calculate jump velocity
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // Look around with the mouse
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f); // Clamp the vertical rotation
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // Method to lock/unlock player input
    public void LockInput(bool lockInput)
    {
        isInputLocked = lockInput;
        // Optionally, you can also unlock the cursor when the input is locked
        Cursor.lockState = lockInput ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
