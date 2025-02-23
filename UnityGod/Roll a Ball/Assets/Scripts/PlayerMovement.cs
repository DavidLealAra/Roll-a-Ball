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
    public float jumpForce = 4f;
    // private bool canJump = false;
    private bool isMoving = false;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    
    public GameObject enemy;
    public GameObject enemy2;

    public GameObject P_transparente;
    public GameObject P_transparente_2;

    public GameObject gameOverText;
    public GameObject finalWinText;
    private bool isInvulnerable = false;
    private bool hasTeleported = false;
    private bool hasFinalTeleported = false;
    private Renderer playerRenderer;

    public Transform teleportDestination;
    public Transform finalTeleportDestination;
    public Animator animator;

    public enum PlayerState {Idle, Moving, Jumping, Invulnerable}
    public PlayerState currentState;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        count = 0;
        winTextObject.SetActive(false);
        gameOverText.SetActive(false);
        finalWinText.SetActive(false);
        playerRenderer = GetComponent<Renderer>();
        currentState = PlayerState.Idle;
        UpdateState();
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        isMoving = movementX != 0 || movementY != 0;

        if (isMoving)
        {
            currentState = PlayerState.Moving;
        }
        else if (!isInvulnerable)
        {
            currentState = PlayerState.Idle;
        }

        UpdateState();
    }

    //void Update()
    //{
    //    if (Keyboard.current.spaceKey.wasPressedThisFrame && canJump)
    //    {
    //       rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //        canJump = false;
    //        currentState = PlayerState.Jumping;
    //        UpdateState();
    //    }
    //}
    void SetCountText()
    {
        countText.text = "Score: " + count.ToString();

        if (count == 5 && !hasTeleported)  
        {
            winTextObject.SetActive(true);
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
        }

        if (count == 4 && hasTeleported && !hasFinalTeleported)  
        {
            winTextObject.SetActive(true);
            if (enemy2 != null)
            {
                enemy2.SetActive(false);
            }
            StartCoroutine(TeleportToFinalZone());
        }
    }

    private void FixedUpdate()
    {
        //Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        //rb.AddForce(movement * speed);
        Vector3 dir = Vector3.zero;
        dir.x = -Input.acceleration.x;
        dir.z = Input.acceleration.y;
        if (dir.sqrMagnitude > 1)
            dir.Normalize();
        
        dir *= Time.deltaTime;
        transform.Translate(dir * speed);

        // Si el jugador deja de moverse, volvemos a Idle
        if (!isMoving && !isInvulnerable && currentState != PlayerState.Jumping)
        {
            currentState = PlayerState.Idle;
            UpdateState();
        }
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

        if (other.gameObject.CompareTag("TeleportZone"))
        {
            TeleportPlayer();
        }
        if (other.gameObject.CompareTag("FinalZone"))
        {
            FinalWin();        
        }
    }
    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;

            if (currentState == PlayerState.Jumping)
            {
                currentState = isMoving ? PlayerState.Moving : PlayerState.Idle;
                UpdateState();
            }
        }
    }*/

    IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        currentState = PlayerState.Invulnerable;
        UpdateState();
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }

        playerRenderer.enabled = true;
        isInvulnerable = false;

        // Volver al estado adecuado después de la invulnerabilidad
        currentState = isMoving ? PlayerState.Moving : PlayerState.Idle;
        UpdateState();
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

    void TeleportPlayer()
    {
        if (teleportDestination != null)
        {
            transform.position = teleportDestination.position;
            count = 0;
            SetCountText();
            winTextObject.SetActive(false);
            hasTeleported = true;

            if (enemy2 != null)
            {
                enemy2.SetActive(true);
            }

            // canJump = true;
        }
        else
        {
            Debug.LogWarning("No se ha asignado un destino de teletransporte.");
        }
    }

    IEnumerator TeleportToFinalZone()
    {
        yield return new WaitForSeconds(1f);
        if (finalTeleportDestination != null)
        {
            transform.position = finalTeleportDestination.position;
            count = 0;
            SetCountText();
            winTextObject.SetActive(false);
            hasFinalTeleported = true;
        }
        else
        {
            Debug.LogWarning("No se ha asignado un destino de teletransporte final.");
        }
    }

    void FinalWin()
    {   
        finalWinText.SetActive(true);
        StartCoroutine(RestartGame());
    }

void UpdateState()
{
    animator.SetBool("isMoving", currentState == PlayerState.Moving);
    animator.SetBool("isJumping", currentState == PlayerState.Jumping);
    animator.SetBool("isInvulnerable", currentState == PlayerState.Invulnerable);
}

}
