using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject missionPanel;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI subText;
    [SerializeField] private Image panelBackground;
    [SerializeField] private Image lineLeft;
    [SerializeField] private Image lineRight;

    [Header("Timing")]
    [SerializeField] private float delayBeforeShow = 3.5f;
    [SerializeField] private float displayDuration = 5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip missionPassedSound;

    void Awake()
    {
        Instance = this;
        missionPanel.SetActive(false);
    }

    public void ShowMissionPassed()
    {
        StartCoroutine(DelayThenShow());
    }

    IEnumerator DelayThenShow()
    {
        // Wait for death animation to play first
        yield return new WaitForSeconds(delayBeforeShow);

        missionPanel.SetActive(true);
        ResetUI();

        // Play sound
        if (audioSource != null && missionPassedSound != null)
            audioSource.PlayOneShot(missionPassedSound);

        // Run all animations together
        StartCoroutine(FadeInBackground());
        StartCoroutine(AnimateMissionText());
        StartCoroutine(AnimateSubText());
        StartCoroutine(AnimateLines());

        // Wait then fade out everything
        yield return new WaitForSeconds(displayDuration);
        StartCoroutine(FadeOutAll());
    }

    void ResetUI()
    {
        // Reset everything to invisible
        SetAlpha(panelBackground, 0f);
        SetTextAlpha(missionText, 0f);
        SetTextAlpha(subText, 0f);
        SetAlpha(lineLeft, 0f);
        SetAlpha(lineRight, 0f);

        // Reset text scale
        missionText.transform.localScale = Vector3.zero;
        subText.transform.localScale = new Vector3(1f, 0f, 1f);

        // Reset line width
        lineLeft.rectTransform.sizeDelta =
            new Vector2(0, lineLeft.rectTransform.sizeDelta.y);
        lineRight.rectTransform.sizeDelta =
            new Vector2(0, lineRight.rectTransform.sizeDelta.y);
    }

    // Fade in dark background
    IEnumerator FadeInBackground()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Color c = panelBackground.color;
        c.a = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 0.85f, elapsed / duration);
            panelBackground.color = c;
            yield return null;
        }
    }

    // Main text pops in with scale bounce
    IEnumerator AnimateMissionText()
    {
        yield return new WaitForSeconds(0.3f);

        SetTextAlpha(missionText, 1f);
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Bounce scale effect
            float scale = BounceEase(t);
            missionText.transform.localScale =
                Vector3.one * scale;

            yield return null;
        }

        missionText.transform.localScale = Vector3.one;

        // Subtle pulse after appearing
        StartCoroutine(PulseText(missionText));
    }

    // Subtitle slides up from below
    IEnumerator AnimateSubText()
    {
        yield return new WaitForSeconds(0.7f);

        SetTextAlpha(subText, 1f);
        float duration = 0.4f;
        float elapsed = 0f;

        Vector3 startScale = new Vector3(1f, 0f, 1f);
        Vector3 endScale = Vector3.one;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            subText.transform.localScale =
                Vector3.Lerp(startScale, endScale,
                SmoothStep(t));
            yield return null;
        }

        subText.transform.localScale = Vector3.one;
    }

    // Gold lines expand outward
    IEnumerator AnimateLines()
    {
        yield return new WaitForSeconds(0.3f);

        SetAlpha(lineLeft, 1f);
        SetAlpha(lineRight, 1f);

        float duration = 0.6f;
        float elapsed = 0f;
        float targetWidth = 200f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = SmoothStep(elapsed / duration);
            float w = Mathf.Lerp(0f, targetWidth, t);

            lineLeft.rectTransform.sizeDelta =
                new Vector2(w,
                lineLeft.rectTransform.sizeDelta.y);
            lineRight.rectTransform.sizeDelta =
                new Vector2(w,
                lineRight.rectTransform.sizeDelta.y);

            yield return null;
        }
    }

    // Gentle pulse on mission text
    IEnumerator PulseText(TextMeshProUGUI text)
    {
        float duration = 1.5f;
        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;
            float scale = 1f + Mathf.Sin(elapsed * 2f) * 0.03f;
            if (text != null)
                text.transform.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    // Fade everything out
    IEnumerator FadeOutAll()
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            SetTextAlpha(missionText, alpha);
            SetTextAlpha(subText, alpha);
            SetAlpha(lineLeft, alpha);
            SetAlpha(lineRight, alpha);
            SetAlpha(panelBackground, alpha * 0.85f);

            yield return null;
        }

        missionPanel.SetActive(false);
    }

    // Helper functions
    void SetAlpha(Image image, float alpha)
    {
        if (image == null) return;
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }

    void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        if (text == null) return;
        Color c = text.color;
        c.a = alpha;
        text.color = c;
    }

    float BounceEase(float t)
    {
        if (t < 0.7f)
            return t / 0.7f * 1.2f;
        else if (t < 0.85f)
            return 1.2f - (t - 0.7f) / 0.15f * 0.2f;
        else
            return 1f + (t - 0.85f) / 0.15f * 0.05f;
    }

    float SmoothStep(float t)
    {
        return t * t * (3f - 2f * t);
    }
}