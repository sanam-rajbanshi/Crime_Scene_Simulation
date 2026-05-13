using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SwatTalkTrigger : MonoBehaviour
{
    [Header("References")]
    public GameObject swat;                    // Drag Swat here
    public Transform playerTransform;          // Drag PlayerArmature here
    public AudioClip swatVoiceClip;            // Drag your voice audio here

    [Header("Settings")]
    public float talkingDistance = 2f;         // How close Swat gets before talking
    public float talkDuration = 5f;            // How long talking lasts

    private NavMeshAgent swatAgent;
    private Animator swatAnimator;
    private AudioSource swatAudio;
    private bool hasTriggered = false;         // Only trigger once

    void Start()
    {
        // Get components from Swat
        swatAgent = swat.GetComponent<NavMeshAgent>();
        swatAnimator = swat.GetComponent<Animator>();
        swatAudio = swat.GetComponent<AudioSource>();
    }

    // When PlayerArmature enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;  // Don't trigger again

        if (other.CompareTag("Player"))  // Make sure PlayerArmature has "Player" tag
        {
            hasTriggered = true;
            StartCoroutine(SwatApproachAndTalk());
        }
    }

    IEnumerator SwatApproachAndTalk()
    {
        // 1. Make Swat walk to player
        swatAgent.isStopped = false;
        swatAgent.SetDestination(playerTransform.position);

        // 2. Wait until Swat is close enough
        while (swatAgent.remainingDistance > talkingDistance || swatAgent.pathPending)
        {
            // Keep updating destination in case player moves
            swatAgent.SetDestination(playerTransform.position);
            yield return null;
        }

        // 3. Swat has arrived - stop moving
        swatAgent.isStopped = true;

        // 4. Make Swat face the player
        Vector3 lookDir = playerTransform.position - swat.transform.position;
        lookDir.y = 0;
        swat.transform.rotation = Quaternion.LookRotation(lookDir);

        // 5. Play talking animation
        swatAnimator.SetBool("isTalking", true);

        // 6. Play voice audio at the same time
        if (swatVoiceClip != null)
        {
            swatAudio.clip = swatVoiceClip;
            swatAudio.Play();
            talkDuration = swatVoiceClip.length;  // Match duration to audio length
        }

        // 7. Wait for talking to finish
        yield return new WaitForSeconds(talkDuration);

        // 8. Stop talking animation
        swatAnimator.SetBool("isTalking", false);
        swatAudio.Stop();

        // 9. Optional: Swat walks back to original position
        // swatAgent.isStopped = false;
        // swatAgent.SetDestination(originalPosition);
    }
}
