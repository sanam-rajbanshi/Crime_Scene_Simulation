using UnityEngine;

public class PhoneToggle : MonoBehaviour
{
    public GameObject mobileCanvas;
    public GameObject phoneHomePanel; // drag PhoneHomePanel here
    public float interactDistance = 5f;
    private bool isOpen = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isOpen) return;

            Ray ray = mainCamera.ScreenPointToRay(
                new Vector3(Screen.width / 2, Screen.height / 2, 0)
            );
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                if (hit.transform == this.transform ||
                    hit.transform.IsChildOf(this.transform))
                {
                    TogglePhone();
                }
            }
        }

        // Only close phone with Backspace when on HOME panel
        if (Input.GetKeyDown(KeyCode.Backspace) && isOpen)
        {
            if (phoneHomePanel.activeSelf)
            {
                TogglePhone();
            }
            // else PhoneUIController handles going back to home
        }
    }

    void TogglePhone()
    {
        isOpen = !isOpen;
        mobileCanvas.SetActive(isOpen);

        if (isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}