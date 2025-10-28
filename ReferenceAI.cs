using UnityEngine;
using UnityEngine.AI;

public class ReferenceAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectionRange = 10f;
    public float moveSpeed = 3.5f;
    public float stopDistance = 1.5f; // dystans, w którym AI przestaje podchodziæ i "atakuje"
    public Transform player; // Przypisz gracza w inspektorze
    public Transform[] patrolPoints; // Punkty patrolowe
    public float patrolWaitTime = 2f;
    public float lostPlayerWaitTime = 2f;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private float patrolTimer = 0f;
    private bool chasingPlayer = false;
    private bool lostPlayer = false;
    private float lostPlayerTimer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = stopDistance;
    }

    void Start()
    {
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= detectionRange)
            {
                chasingPlayer = true;
                lostPlayer = false;
                lostPlayerTimer = 0f;

                if (distance > stopDistance)
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                }
                else
                {
                    agent.isStopped = true;
                    // Tu mo¿esz dodaæ logikê ataku w przysz³oœci
                    // Na razie AI tylko siê zatrzymuje i patrzy na gracza
                    Vector3 lookDirection = (player.position - transform.position).normalized;
                    lookDirection.y = 0;
                    if (lookDirection != Vector3.zero)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 5f);
                    }
                }
                return;
            }
            else if (chasingPlayer && !lostPlayer)
            {
                // Gracz uciek³ — idŸ do ostatniej pozycji
                agent.isStopped = false;
                agent.SetDestination(player.position);
                chasingPlayer = false;
                lostPlayer = true;
                lostPlayerTimer = 0f;
                return;
            }
        }

        // Jeœli zgubi³ gracza i dotar³ do ostatniej pozycji, czeka przez lostPlayerWaitTime
        if (lostPlayer && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            lostPlayerTimer += Time.deltaTime;
            if (lostPlayerTimer >= lostPlayerWaitTime)
            {
                lostPlayer = false;
                lostPlayerTimer = 0f;
                GoToNextPatrolPoint();
            }
        }
        else if (!lostPlayer)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                GoToNextPatrolPoint();
            }
        }
        else
        {
            patrolTimer = 0f;
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        patrolTimer = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}