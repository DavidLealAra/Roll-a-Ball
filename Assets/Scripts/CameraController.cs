using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Referencia al GameObject del jugador.
    public GameObject player;

    // La distancia entre la cámara y el jugador en vista de tercera persona.
    private Vector3 offset;

    // Alternador para cambiar entre vistas de primera persona y tercera persona.
    private bool isFirstPerson = false;

    // Ajuste para la altura de la cámara en primera persona.
    public float firstPersonHeight = 0.5f;

    // Sensibilidad del ratón para rotar la cámara en vista de primera persona.
    public float mouseSensitivity = 100f;

    // Velocidad de movimiento del jugador.
    public float movementSpeed = 5f;

    // Variables para almacenar la rotación de la cámara en vista de primera persona.
    private float xRotation = 0f; // Rotación vertical
    private float yRotation = 0f; // Rotación horizontal

    // Rigidbody para el movimiento del jugador.
    private Rigidbody playerRigidbody;

    // Start se llama antes de la primera actualización del frame.
    void Start()
    {
        // Calcula el desplazamiento inicial para la vista de tercera persona.
        offset = transform.position - player.transform.position;

        // Bloquea el cursor en el centro de la pantalla en modo de primera persona.
        Cursor.lockState = CursorLockMode.Locked;

        // Obtiene el componente Rigidbody del jugador.
        playerRigidbody = player.GetComponent<Rigidbody>();
    }

    // Update se llama una vez por frame.
    void Update()
    {
        // Alterna entre las vistas de primera persona y tercera persona con la tecla "F".
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFirstPerson = !isFirstPerson;

            // Bloquea o desbloquea el cursor según la vista.
            Cursor.lockState = isFirstPerson ? CursorLockMode.Locked : CursorLockMode.None;
        }

        if (isFirstPerson)
        {
            // Obtiene la entrada del ratón para rotar la cámara.
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Ajusta la rotación vertical y la limita para evitar que se voltee.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Ajusta la rotación horizontal.
            yRotation += mouseX;

            // Aplica las rotaciones a la cámara.
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    // FixedUpdate se llama para actualizaciones físicas.
    void FixedUpdate()
    {
        if (isFirstPerson)
        {
            // Maneja el movimiento del jugador en modo de primera persona.
            HandleFirstPersonMovement();
        }
    }

    // LateUpdate se llama una vez por frame después de que todas las funciones Update se hayan completado.
    void LateUpdate()
    {
        if (isFirstPerson)
        {
            // Vista de primera persona: Coloca la cámara ligeramente por encima del jugador.
            transform.position = player.transform.position + new Vector3(0, firstPersonHeight, 0);
        }
        else
        {
            // Vista de tercera persona: Mantén el desplazamiento inicial sin heredar la rotación.
            transform.position = player.transform.position + offset;
            transform.LookAt(player.transform.position);
        }
    }

    private void HandleFirstPersonMovement()
    {
        // Obtiene la entrada para el movimiento.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calcula las direcciones hacia adelante y a la derecha basadas en la rotación de la cámara.
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Aplana los vectores forward y right para ignorar la rotación vertical.
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Calcula la dirección del movimiento.
        Vector3 movementDirection = (forward * moveVertical + right * moveHorizontal).normalized;

        // Aplica la velocidad directamente al Rigidbody del jugador.
        playerRigidbody.velocity = movementDirection * movementSpeed + new Vector3(0, playerRigidbody.velocity.y, 0);
    }
}
