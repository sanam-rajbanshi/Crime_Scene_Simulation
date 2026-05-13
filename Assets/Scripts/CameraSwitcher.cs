using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject thirdPersonVCam;
    public GameObject firstPersonVCam;

    private bool isThirdPerson = true;

    void Start()
    {
        thirdPersonVCam.SetActive(true);
        firstPersonVCam.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isThirdPerson = !isThirdPerson;
            thirdPersonVCam.SetActive(isThirdPerson);
            firstPersonVCam.SetActive(!isThirdPerson);
        }
    }
}