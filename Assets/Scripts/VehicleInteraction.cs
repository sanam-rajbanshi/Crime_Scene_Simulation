using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class VehicleInteraction : MonoBehaviour
{
    [SerializeField] private float enterRadius = 3f;
    [SerializeField] private GameObject playerArmature;
    [SerializeField] private GameObject playerFollowCamera;

    [Header("Seat Offset")]
    [SerializeField] private Vector3 seatOffset = new Vector3(0.3f, 0.2f, 0f);

    // ✓ All private fields must be INSIDE the class
    private CarController carController;
    private ThirdPersonController playerController;
    private CarSound carSound;
    private bool inCar = false;

    void Start()
    {
        carSound = GetComponent<CarSound>();
        carController = GetComponent<CarController>();
        playerController = playerArmature.GetComponent<ThirdPersonController>();
        SetCarMode(false);
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, playerArmature.transform.position);

        if (!Input.GetKeyDown(KeyCode.F)) return;

        if (!inCar && dist <= enterRadius)
            SetCarMode(true);
        else if (inCar)
            SetCarMode(false);
    }

    void SetCarMode(bool entering)
    {
        inCar = entering;

        carSound.SetEngineActive(entering);

        if (entering)
        {
            playerArmature.transform.SetParent(transform);
            playerArmature.transform.localPosition = seatOffset;
        }
        else
        {
            playerArmature.transform.SetParent(null);
        }

        // Car
        carController.enabled = entering;

        // Player
        playerController.enabled = !entering;
        playerArmature.GetComponent<CharacterController>().enabled = !entering;
        playerArmature.GetComponent<PlayerInput>().enabled = !entering;
        playerFollowCamera.SetActive(!entering);
        playerArmature.GetComponent<Animator>().enabled = !entering;
    }
}