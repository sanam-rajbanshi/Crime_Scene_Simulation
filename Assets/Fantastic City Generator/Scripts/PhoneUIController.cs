using UnityEngine;
using UnityEngine.UI;

public class PhoneUIController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject phoneHomePanel;
    public GameObject messagesPanel;
    public GameObject photosPanel;

    [Header("Message Slides")]
    public GameObject[] messageSlides;
    private int currentSlide = 0;

    [Header("Buttons")]
    public Button messagesButton;
    public Button photosButton;

    void Start()
    {
        messagesButton.onClick.AddListener(OpenMessages);
        photosButton.onClick.AddListener(OpenPhotos);
        ShowPanel("home");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            // If in messages or photos go back to home
            if (messagesPanel.activeSelf || photosPanel.activeSelf)
            {
                ShowPanel("home");
                return; // stop here dont close phone
            }
        }

        if (messagesPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentSlide < messageSlides.Length - 1)
                {
                    currentSlide++;
                    RefreshSlides();
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentSlide > 0)
                {
                    currentSlide--;
                    RefreshSlides();
                }
            }
        }
    }

    public void OpenMessages()
    {
        currentSlide = 0;
        ShowPanel("messages");
        RefreshSlides();
    }

    public void OpenPhotos()
    {
        ShowPanel("photos");
    }

    void RefreshSlides()
    {
        for (int i = 0; i < messageSlides.Length; i++)
            messageSlides[i].SetActive(i == currentSlide);
    }

    public void ShowPanel(string panel)
    {
        phoneHomePanel.SetActive(panel == "home");
        messagesPanel.SetActive(panel == "messages");
        photosPanel.SetActive(panel == "photos");
    }
}