using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator npcAnimator;
    [SerializeField] private string deathTriggerName = "isDead";

    private NPCReaction npcReaction;
    private UnityEngine.AI.NavMeshAgent agent;
    private CapsuleCollider capsuleCollider;
    private bool isDead = false;

    void Start()
    {
        npcAnimator = GetComponent<Animator>();
        npcReaction = GetComponent<NPCReaction>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if (other.CompareTag("Bullet"))
        {
            Die();
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Die();
            Destroy(collision.gameObject);
        }
    }

    void Die()
    {
        isDead = true;

        // Stop NPCReaction
        if (npcReaction != null)
            npcReaction.enabled = false;

        // Stop NavMesh completely
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.enabled = false;
        }

        // Disable capsule collider
        if (capsuleCollider != null)
            capsuleCollider.enabled = false;

        // Stop all bools
        npcAnimator.SetBool("isWalking", false);
        npcAnimator.SetBool("isRunning", false);
        npcAnimator.SetBool("playerNear", false);

        // Apply root motion OFF via script too
        npcAnimator.applyRootMotion = false;

        // Play death
        npcAnimator.SetTrigger(deathTriggerName);

        // ← ADD THIS LINE
        MissionManager.Instance.ShowMissionPassed();


        StartCoroutine(FreezeAfterDeath());
    }

    System.Collections.IEnumerator FreezeAfterDeath()
    {
        yield return new WaitForSeconds(0.1f);

        AnimatorStateInfo stateInfo =
            npcAnimator.GetCurrentAnimatorStateInfo(0);
        float deathLength = stateInfo.length;

        yield return new WaitForSeconds(deathLength);

        // Freeze on last frame
        npcAnimator.speed = 0;
    }
}