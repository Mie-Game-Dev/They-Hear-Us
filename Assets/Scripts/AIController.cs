using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform[] patrolPoints;        // Array of patrol points for AI to move between
    public Transform target;                // Target destination for AI (e.g., the player)
    public float detectionRadius = 10f;     // Radius to detect the player
    public float stoppingDistance = 1f;     // Distance to stop when near the target
    public LayerMask targetMask;            // Layer mask for target detection
    public bool isChasing = false;          // Boolean to check if AI is chasing target

    private NavMeshAgent agent;             // Reference to the AI's NavMeshAgent
    private int currentPatrolIndex;         // Current patrol point index

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartPatrolling();                  // Start the AI patrolling by default
    }

    private void Update()
    {
        if (isChasing)
        {
            ChaseTarget();
        }
        else
        {
            Patrol();
        }
        CheckForTarget();
    }

    // Patrol between points
    private void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    // Chase the player or a target
    private void ChaseTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);

            // Stop chasing if close enough
            if (agent.remainingDistance <= stoppingDistance)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }
        }
    }

    // Check if a target is within detection range
    private void CheckForTarget()
    {
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);

        if (targetsInView.Length > 0)
        {
            Transform detectedTarget = targetsInView[0].transform;
            target = detectedTarget;
            isChasing = true;  // Switch to chase mode if a target is detected
        }
        else
        {
            isChasing = false;  // Continue patrolling if no target is detected
        }
    }

    // Start patrolling
    private void StartPatrolling()
    {
        currentPatrolIndex = 0;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    // Visualize the detection radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
