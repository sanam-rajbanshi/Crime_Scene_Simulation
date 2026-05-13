using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PhoneCallEvent : MonoBehaviour
{
    [Header("Timing")]
    public float delayBeforeCall = 5f;

    [Header("Animation")]
    public Animator playerAnimator;
    public string phoneAnimationTrigger = "ReceiveCall";
    public string returnAnimationTrigger = "ReturnToIdle";

    [Header("Audio")]
    public AudioClip phoneRingClip;
    public AudioClip voiceClip;
    public AudioSource audioSource;

    [Header("Phone UI")]
    public GameObject phoneUI;
    public TextMeshProUGUI callerNameText;
    public TextMeshProUGUI callerNumberText;
    public GameObject acceptButton;  // Drag AcceptButton here
    public GameObject declineButton; // Drag DeclineButton here

    [Header("Player Control")]
    public MonoBehaviour playerMovementScript;
    public CharacterController playerController;

    private bool callTriggered = false;

    void Start()
    {
        if (phoneUI != null)
            phoneUI.SetActive(false);
    }

    void Update()
    {
        if (!callTriggered && Time.time >= delayBeforeCall)
        {
            callTriggered = true;
            StartCoroutine(PlayPhoneCall());
        }
    }

    IEnumerator PlayPhoneCall()
    {
        // Step 1: Show phone UI
        if (phoneUI != null)
        {
            phoneUI.SetActive(true);

            if (callerNameText != null)
                callerNameText.text = "DISPATCH";
            if (callerNumberText != null)
                callerNumberText.text = "Incoming Call...";

            // Show BOTH buttons during ringing
            if (acceptButton != null)
                acceptButton.SetActive(true);
            if (declineButton != null)
                declineButton.SetActive(true);
        }

        // Step 2: Play ring sound
        if (phoneRingClip != null)
        {
            audioSource.clip = phoneRingClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        // Ring for 3 seconds
        yield return new WaitForSeconds(3f);

        // Step 3: Ring ends — hide Accept, keep only Decline visible
        if (acceptButton != null)
            acceptButton.SetActive(false);
        if (declineButton != null)
            declineButton.SetActive(true);

        // Update text
        if (callerNumberText != null)
            callerNumberText.text = "Connected...";

        // Stop ring sound
        audioSource.loop = false;
        audioSource.Stop();

        // Disable player movement
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        // Step 4: Play phone animation
        if (playerAnimator != null)
            playerAnimator.SetTrigger(phoneAnimationTrigger);

        // Step 5: Play voice after 1 second
        yield return new WaitForSeconds(1f);

        if (voiceClip != null)
        {
            audioSource.clip = voiceClip;
            audioSource.loop = false;
            audioSource.Play();
            yield return new WaitForSeconds(voiceClip.length);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }

        // Step 6: End call — hide everything
        if (phoneUI != null)
            phoneUI.SetActive(false);

        // Step 7: Return to idle animation
        if (playerAnimator != null)
            playerAnimator.SetTrigger(returnAnimationTrigger);

        // Step 8: Re-enable movement
        yield return new WaitForSeconds(0.5f);
        if (playerMovementScript != null)
            playerMovementScript.enabled = true;
    }
}