using UnityEngine;
using UnityEngine.AI;

public class NPCWander : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints; // Assign waypoints in Inspector
    public float waypointReachDistance = 0.5f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to " + gameObject.name);
            return;
        }

        // Start walking to first waypoint
        GoToWaypoint(currentWaypointIndex);
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        // Check if NPC reached current waypoint
        if (!agent.pathPending && agent.remainingDistance <= waypointReachDistance)
        {
            // Move to next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            GoToWaypoint(currentWaypointIndex);
        }
    }

    void GoToWaypoint(int index)
    {
        agent.SetDestination(waypoints[index].position);
    }

    // Draw waypoint path in Scene view for easy editing
    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            // Draw sphere at each waypoint
            Gizmos.DrawSphere(waypoints[i].position, 0.3f);

            // Draw line to next waypoint
            int nextIndex = (i + 1) % waypoints.Length;
            if (waypoints[nextIndex] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
        }
    }
}