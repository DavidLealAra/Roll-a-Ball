using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Reference to the player GameObject.
    public GameObject player;

    // The distance between the camera and the player in third-person view.
    private Vector3 offset;

    // Toggle for switching between first-person and third-person views.
    private bool isFirstPerson = false;

    // Adjustment for the first-person camera height.
    public float firstPersonHeight = 0.5f;

    // Mouse sensitivity for rotating the camera in first-person view.
    public float mouseSensitivity = 100f;

    // Speed of the player's movement.
    public float movementSpeed = 5f;

    // Variables to store camera rotation in first-person view.
    private float xRotation = 0f; // Vertical rotation
    private float yRotation = 0f; // Horizontal rotation

    // Rigidbody for the player's movement.
    private Rigidbody playerRigidbody;

    // Start is called before the first frame update.
    void Start()
    {
        // Calculate the initial offset for third-person view.
        offset = transform.position - player.transform.position;

        // Lock the cursor to the center of the screen in first-person mode.
        Cursor.lockState = CursorLockMode.Locked;

        // Get the Rigidbody component of the player.
        playerRigidbody = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle between first-person and third-person views using the "F" key.
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFirstPerson = !isFirstPerson;

            // Lock or unlock the cursor depending on the view.
            Cursor.lockState = isFirstPerson ? CursorLockMode.Locked : CursorLockMode.None;
        }

        if (isFirstPerson)
        {
            // Get mouse input to rotate the camera.
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Adjust the vertical rotation and clamp it to avoid flipping.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Adjust the horizontal rotation.
            yRotation += mouseX;

            // Apply rotations to the camera.
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    // FixedUpdate is called for physics updates.
    void FixedUpdate()
    {
        if (isFirstPerson)
        {
            // Handle player movement in first-person mode.
            HandleFirstPersonMovement();
        }
    }

    // LateUpdate is called once per frame after all Update functions have been completed.
    void LateUpdate()
    {
        if (isFirstPerson)
        {
            // First-person view: Place the camera slightly above the player.
            transform.position = player.transform.position + new Vector3(0, firstPersonHeight, 0);
        }
        else
        {
            // Third-person view: Maintain the initial offset without inheriting rotation.
            transform.position = player.transform.position + offset;
            transform.LookAt(player.transform.position);
        }
    }

    private void HandleFirstPersonMovement()
    {
        // Get input for movement.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the forward and right directions based on the camera's rotation.
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Flatten the forward and right vectors to ignore vertical rotation.
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Calculate the movement direction.
        Vector3 movementDirection = (forward * moveVertical + right * moveHorizontal).normalized;

        // Apply the velocity directly to the player's Rigidbody.
        playerRigidbody.velocity = movementDirection * movementSpeed + new Vector3(0, playerRigidbody.velocity.y, 0);
    }
}

