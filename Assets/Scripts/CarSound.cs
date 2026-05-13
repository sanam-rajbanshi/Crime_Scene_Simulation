using UnityEngine;
public class CarSound : MonoBehaviour
{
    [Header("Engine Clips")]
    [SerializeField] private AudioClip startupClip;
    [SerializeField] private AudioClip idleClip;
    [SerializeField] private AudioClip lowClip;
    [SerializeField] private AudioClip medClip;
    [SerializeField] private AudioClip highClip;

    [Header("Speed Thresholds")]
    [SerializeField] private float lowSpeed = 5f;
    [SerializeField] private float medSpeed = 12f;
    [SerializeField] private float highSpeed = 20f;

    private AudioSource audioSource;
    private AudioClip currentClip;
    private bool engineOn = false;

    void Start()
    {
        // Try to get existing AudioSource, if not found add one automatically
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    public void SetEngineActive(bool active)
    {
        if (audioSource == null) return; // Safety check

        engineOn = active;
        if (active)
        {
            if (startupClip != null)
            {
                audioSource.loop = false;
                audioSource.clip = startupClip;
                audioSource.Play();
                Invoke(nameof(PlayIdle), startupClip.length);
            }
            else
            {
                PlayIdle(); // No startup clip, go straight to idle
            }
        }
        else
        {
            CancelInvoke();
            audioSource.Stop();
            currentClip = null;
        }
    }

    void PlayIdle()
    {
        if (idleClip != null)
            PlayClip(idleClip);
    }

    void Update()
    {
        if (!engineOn) return;
        if (audioSource == null) return; // Safety check
        if (audioSource.clip == startupClip) return;

        float speed = Mathf.Abs(Input.GetAxis("Vertical"));
        AudioClip target;

        if (speed < 0.1f)
            target = idleClip;
        else if (speed < 0.4f)
            target = lowClip;
        else if (speed < 0.75f)
            target = medClip;
        else
            target = highClip;

        if (target != null && target != currentClip)
            PlayClip(target);
    }

    void PlayClip(AudioClip clip)
    {
        currentClip = clip;
        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.Play();
    }
}