using UnityEngine;
public class ProjectileLauncher : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private Rigidbody projectileRigidBody;
    [SerializeField] private float projectilePower = 4500;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private float COOLDOWN_TIME = 0.5f;
    [Header("Gun & Animation")]
    [SerializeField] private GameObject gunObject;
    [SerializeField] private Animator playerAnimator;
    [Header("Audio")]
    [SerializeField] private AudioClip gunShotSound;
    private AudioSource audioSource;
    private float coolDown = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        if (gunObject != null)
            gunObject.SetActive(false);
        playerAnimator.SetLayerWeight(1, 0);
    }

    void Update()
    {
        // CHANGED: Space key down = show gun
        if (Input.GetKeyDown(KeyCode.N))
        {
            gunObject.SetActive(true);
            playerAnimator.SetLayerWeight(1, 1);
            playerAnimator.SetBool("isAiming", true);
        }

        // CHANGED: Space key up = fire bullet
        if (Input.GetKeyUp(KeyCode.N) && coolDown <= 0)
        {
            coolDown = COOLDOWN_TIME;
            playerAnimator.SetTrigger("Shoot");
            FireBullet();
            gunObject.SetActive(false);
            playerAnimator.SetLayerWeight(1, 0);
            playerAnimator.SetBool("isAiming", false);
        }

        if (coolDown > 0)
            coolDown -= Time.deltaTime;
    }

    void FireBullet()
    {
        if (audioSource != null && gunShotSound != null)
            audioSource.PlayOneShot(gunShotSound);

        Rigidbody aInstance = Instantiate(
            projectileRigidBody,
            muzzle.transform.position,
            transform.rotation) as Rigidbody;

        Collider bulletCollider = aInstance.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>();
        if (bulletCollider != null && playerCollider != null)
            Physics.IgnoreCollision(bulletCollider, playerCollider);

        Collider[] gunColliders = gunObject
            .GetComponentsInChildren<Collider>(true);
        foreach (Collider c in gunColliders)
            Physics.IgnoreCollision(bulletCollider, c);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        aInstance.AddForce(forward * projectilePower);
        Destroy(aInstance.gameObject, 8);
    }
}