using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;
    public GameObject playButton;
    public GameObject background; // 👈 Add this line

    void Start()
    {
        playButton.SetActive(false);
        background.SetActive(false); // 👈 Add this line
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        playButton.SetActive(true);
        background.SetActive(true); // 👈 Add this line
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }
}