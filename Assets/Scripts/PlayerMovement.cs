using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    
    public GameObject enemy;
    public GameObject enemy2;

    public GameObject P_transparente;
    public GameObject P_transparente_2;

    public GameObject gameOverText;

    private bool isInvulnerable = false;
    private bool hasTeleported = false;
    private Renderer playerRenderer;

    public Transform teleportDestination; // 🔹 Punto al que se teletransportará el jugador

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        winTextObject.SetActive(false);
        gameOverText.SetActive(false);
        playerRenderer = GetComponent<Renderer>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

void SetCountText()
{
    countText.text = "Score: " + count.ToString();

    // 🔹 PRIMERA VICTORIA (antes de teletransportarse)  
    if (count == 5 && !hasTeleported)  
    {
        winTextObject.SetActive(true);
        if (enemy != null)
        {
            enemy.SetActive(false);
        }
    }

    // 🔹 VICTORIA FINAL (después del teletransporte)
    if (count == 4 && hasTeleported)  
    {
        winTextObject.SetActive(true);
        if (enemy2 != null)
        {
            enemy2.SetActive(false);
        }
        StartCoroutine(RestartGame()); // Reiniciar el juego después de ganar
    }
}

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            if (count >= 1)
            {
                P_transparente.SetActive(false);
            }
            if (count >= 5)
            {
                P_transparente_2.SetActive(false);
            }

            StartCoroutine(Invulnerability());
        }

        // 🔹 Teletransporte cuando toque un objeto con la etiqueta "TeleportZone"
        if (other.gameObject.CompareTag("TeleportZone"))
        {
            TeleportPlayer();
        }
    }

    IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }

        playerRenderer.enabled = true;
        isInvulnerable = false;
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    // 🔹 Función para teletransportar al jugador
void TeleportPlayer()
{
    if (teleportDestination != null)
    {
        transform.position = teleportDestination.position;
        count = 0; // 🔹 Reiniciar el contador al teletransportarse
        SetCountText(); // 🔹 Actualizar el UI del contador
        winTextObject.SetActive(false); // 🔹 Ocultar el mensaje de victoria de la primera zona
        hasTeleported = true; // 🔹 Marcar que el jugador ya se teletransportó

        if (enemy2 != null)
        {
            enemy2.SetActive(true); // Activa el segundo castor
        }
    }
    else
    {
        Debug.LogWarning("No se ha asignado un destino de teletransporte.");
    }
}
}
