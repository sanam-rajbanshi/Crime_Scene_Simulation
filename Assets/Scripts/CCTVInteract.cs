using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class CCTVInteract : MonoBehaviour
{
    [Header("CCTV Settings")]
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Video")]
    public VideoPlayer videoPlayer;
    public GameObject cctvScreen;

    [Header("Player")]
    public MonoBehaviour playerMovementScript;
    public GameObject playerCamera;

    [Header("UI")]
    public GameObject interactPrompt; // "Press E to view CCTV" text

    private bool playerInRange = false;
    private bool isWatching = false;
    private Transform playerTransform;

    void Start()
    {
        // Hide screen at start
        if (cctvScreen != null)
            cctvScreen.SetActive(false);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        // Find player
        playerTransform = GameObject.FindWithTag("Player").transform;

        // Listen for video end
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Check distance to CCTV
        float distance = Vector3.Distance(
            transform.position,
            playerTransform.position
        );

        if (distance <= interactDistance)
        {
            playerInRange = true;

            // Show interact prompt
            if (interactPrompt != null && !isWatching)
                interactPrompt.SetActive(true);

            // Press E to interact
            if (Input.GetKeyDown(interactKey) && !isWatching)
            {
                StartWatchingCCTV();
            }
        }
        else
        {
            playerInRange = false;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }

        // Press Escape to stop watching early
        if (isWatching && Input.GetKeyDown(KeyCode.Escape))
        {
            StopWatchingCCTV();
        }
    }

    void StartWatchingCCTV()
    {
        isWatching = true;

        // Hide interact prompt
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        // Show CCTV screen
        if (cctvScreen != null)
            cctvScreen.SetActive(true);

        // Disable player movement
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        // Lock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Play video
        videoPlayer.time = 0;
        videoPlayer.Play();

        // Pause game time optionally
        // Time.timeScale = 0f;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(ReturnToGame());
    }

    IEnumerator ReturnToGame()
    {
        yield return new WaitForSeconds(0.5f);
        StopWatchingCCTV();
    }

    void StopWatchingCCTV()
    {
        isWatching = false;

        // Stop video
        videoPlayer.Stop();

        // Hide CCTV screen
        if (cctvScreen != null)
            cctvScreen.SetActive(false);

        // Re-enable player movement
        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        // Lock cursor back
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}