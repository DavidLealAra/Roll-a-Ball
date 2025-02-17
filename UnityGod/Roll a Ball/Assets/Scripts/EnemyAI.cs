using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private PlayerController playerController; // Referencia al jugador

void Start()
{
    agent = GetComponent<NavMeshAgent>();

    // Asegurarte de que el enemigo sigue al jugador
    agent.SetDestination(player.position);
}


void Update()
{
    if (player != null)
    {
        agent.SetDestination(player.position);
    }
}


void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("¡El enemigo tocó al jugador!"); // Verifica si la colisión ocurre

        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null)
        {
            if (!playerController.IsInvulnerable()) // Si el jugador NO es invulnerable
            {
                Debug.Log("Ejecutando GameOver()"); // Verifica si entra a esta condición
                playerController.GameOver();
            }
            else
            {
                Debug.Log("El jugador es invulnerable, no se ejecuta GameOver.");
            }
        }
    }
}

}
