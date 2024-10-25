using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;

    private CharacterController characterController;
    private float verticalRotation = 0f;
    private bool isInputLocked = false; // Flag to control input

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    void Update()
    {
        if (isInputLocked)
        {
            // Prevent movement and looking around
            return;
        }

        // Move the player
        float moveX = Input.GetAxis("Horizontal") * moveSpeed;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed;
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * Time.deltaTime);

        // Look around with the mouse
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f); // Clamp the vertical rotation
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
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
