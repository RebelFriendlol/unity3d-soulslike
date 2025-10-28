using UnityEngine;
using UnityEngine.AI;
using System.Collections;
// dodac zeby obracalo sie w strone gracza kiedy stoi przed graczem (stopDistance)
public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectionRange = 10f;
    public float moveSpeed = 3.5f;
    public float stopDistance = 1.5f;
    public Transform player;
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    public float lostPlayerWaitTime = 2f;

    [Header("Attack Settings")]
    public Color attackColor = Color.red;
    public float chargeTime = 3f;         // czas ³adowania ataku
    public float flashDuration = 0.1f;
    public float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private float patrolTimer = 0f;
    private bool chasingPlayer = false;
    private bool lostPlayer = false;
    private float lostPlayerTimer = 0f;
    private float lastAttackTime = -Mathf.Infinity;
    private bool isAttacking = false;

    private Renderer rend;
    private Color originalColor;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = stopDistance;

        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    void Start()
    {
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (isAttacking) return;  // nie robi nic, jeœli trwa atak (³adowanie/ruch)

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

                    Vector3 lookDirection = (player.position - transform.position).normalized;
                    lookDirection.y = 0;
                    if (lookDirection != Vector3.zero)
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 5f);

                    // Atak co cooldown
                    if (Time.time - lastAttackTime >= attackCooldown)
                    {
                        lastAttackTime = Time.time;
                        StartCoroutine(ChargeAndAttack());
                    }
                }
                return;
            }
            else if (chasingPlayer && !lostPlayer)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                chasingPlayer = false;
                lostPlayer = true;
                lostPlayerTimer = 0f;
                return;
            }
        }

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

    private IEnumerator ChargeAndAttack()
    {
        isAttacking = true;

        // £adowanie ataku - stopniowe przejœcie koloru
        float timer = 0f;
        while (timer < chargeTime)
        {
            timer += Time.deltaTime;
            if (rend != null)
                rend.material.color = Color.Lerp(originalColor, attackColor, timer / chargeTime);
            yield return null;
        }

        // Ruch do przodu i powrót
        Vector3 startPos = transform.position;
        Vector3 attackPos = startPos + transform.forward * 1.5f;

        float moveSpeed = 10f;
        float t = 0f;

        // Ruch do przodu
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, attackPos, t);
            yield return null;
        }

        // Powrót
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(attackPos, startPos, t);
            yield return null;
        }

        // Powrót koloru do orygina³u
        if (rend != null)
            rend.material.color = originalColor;

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
