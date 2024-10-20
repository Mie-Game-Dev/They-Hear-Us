using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public DynamicSensor sensor;           // Reference to the DynamicSensor component
    public Transform[] waypoints;          // List of waypoints for patrolling
    public float stoppingDistance = 1f;    // Distance to stop when near the target
    public float patrolWaitTime = 2f;      // Time to wait at each waypoint
    public bool isChasing = false;         // Boolean to check if AI is chasing a target

    private NavMeshAgent agent;            // Reference to the AI's NavMeshAgent
    private Transform target;              // Current target (detected dynamically by sensor)
    private int currentWaypointIndex = 0;  // Current waypoint index for patrolling
    private bool waitingAtWaypoint = false;// Boolean to check if AI is waiting at a waypoint
    private float patrolWaitTimer;         // Timer for waiting at waypoints

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        patrolWaitTimer = patrolWaitTime;
        StartPatrolling();  // Start AI patrolling behavior
    }

    private void Update()
    {
        if (isChasing && target != null)
        {
            ChaseTarget();
        }
        else
        {
            Patrol();
        }

        CheckForTarget();  // Continually check for enemies during patrol or chase
    }

    // Use the sensor to check for nearby targets
    private void CheckForTarget()
    {
        Collider[] detectedTargets = sensor.Detect();

        if (detectedTargets.Length > 0)
        {
            target = detectedTargets[0].transform;  // Take the first detected target
            isChasing = true;  // Switch to chase mode
        }
        else
        {
            // If no targets are detected, stop chasing and resume patrolling
            target = null;
            isChasing = false;
        }
    }

    // Chase the detected target
    private void ChaseTarget()
    {
        if (target == null)
        {
            // If target is null, stop and return to patrol
            isChasing = false;
            return;
        }

        agent.SetDestination(target.position);

        if (agent.remainingDistance <= stoppingDistance)
        {
            agent.isStopped = true;  // Stop moving if close enough
        }
        else
        {
            agent.isStopped = false;
        }
    }

    // Patrol between waypoints
    private void Patrol()
    {
        if (waitingAtWaypoint)  // If waiting at the current waypoint
        {
            patrolWaitTimer -= Time.deltaTime;

            if (patrolWaitTimer <= 0f)
            {
                waitingAtWaypoint = false;
                patrolWaitTimer = patrolWaitTime;
                MoveToNextWaypoint();
            }
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)  // Reached a waypoint
        {
            waitingAtWaypoint = true;
            agent.isStopped = true;
        }
    }

    // Move to the next waypoint
    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;  // Loop waypoints
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        agent.isStopped = false;
    }

    // Start patrolling by moving to the first waypoint
    private void StartPatrolling()
    {
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    // Visualize the AI detection in the editor
    private void OnDrawGizmosSelected()
    {
        if (sensor != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sensor.detectionRadius);
        }
    }
}
