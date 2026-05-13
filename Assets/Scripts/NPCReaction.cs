using UnityEngine;

public class NPCReaction : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 10f;

    [Header("Waypoints")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Header("Movement")]
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private float waypointReachDistance = 0.5f;

    [Header("Animation")]
    [SerializeField] private Animator npcAnimator;

    private bool hasReacted = false;
    private bool isRunning = false;
    private Transform currentTarget;

    void Start()
    {
        transform.position = startPoint.position;
        currentTarget = endPoint;

        Vector3 dir = endPoint.position - startPoint.position;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void Update()
    {
        CheckPlayerDistance();
        CheckAnimationFinished();

        if (isRunning)
            RunBetweenWaypoints();
    }

    void CheckPlayerDistance()
    {
        if (player == null || hasReacted) return;

        float distance = Vector3.Distance(
            transform.position,
            player.position);

        if (distance <= detectionRange)
        {
            hasReacted = true;
            // Trigger look back animation
            npcAnimator.SetBool("playerNear", true);
        }
    }

    void CheckAnimationFinished()
    {
        if (!hasReacted || isRunning) return;

        // Check if LookBack animation has finished naturally
        AnimatorStateInfo stateInfo =
            npcAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("LookBack") &&
            stateInfo.normalizedTime >= 1.0f)
        {
            StartRunning();
        }
    }

    void StartRunning()
    {
        isRunning = true;
        npcAnimator.SetBool("playerNear", false);
        npcAnimator.SetBool("isRunning", true);
    }

    void RunBetweenWaypoints()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            runSpeed * Time.deltaTime);

        Vector3 dir = currentTarget.position - transform.position;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        float dist = Vector3.Distance(
            transform.position,
            currentTarget.position);

        if (dist <= waypointReachDistance)
        {
            if (currentTarget == endPoint)
                currentTarget = startPoint;
            else
                currentTarget = endPoint;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (startPoint != null && endPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPoint.position, endPoint.position);
            Gizmos.DrawSphere(startPoint.position, 0.3f);
            Gizmos.DrawSphere(endPoint.position, 0.3f);
        }
    }
}