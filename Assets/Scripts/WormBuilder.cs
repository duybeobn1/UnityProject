using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WormPiece
{
    public GameObject gameObject;
    public DampedTransform damp;

    public WormPiece(GameObject gameObject, DampedTransform dampedTransform)
    {
        this.gameObject = gameObject;
        damp = dampedTransform;
    }

}
public class WormBuilder : MonoBehaviour
{
    [SerializeField] int bodyCount = 10;
    [SerializeField] GameObject templatePiece = default;
    [SerializeField] Transform bodyParent = default;
    [SerializeField] Transform rigParent = default;

    DampedTransform templateDamp;
    List<WormPiece> pieces = new List<WormPiece>();

    void OnEnable()
    {

        if (Application.isPlaying)
        {
            templateDamp = (new GameObject("Damp")).AddComponent<DampedTransform>();
            templateDamp.data.dampRotation = 0.7f;
            templateDamp.data.maintainAim = true;
            SetupWorm();
        }
    }

    void SetupWorm()
    {
        Vector3 startPosition = bodyParent.position;
        for (int i = 0; i < bodyCount; i++)
        {
            startPosition += -transform.forward * 2.5f;
            GameObject obj = Instantiate(templatePiece);
            obj.transform.parent = i == 0 ? bodyParent : pieces[i - 1].gameObject.transform;
            obj.transform.position = startPosition;
            obj.transform.forward = transform.forward;

            DampedTransform damp = Instantiate(templateDamp);
            damp.transform.parent = rigParent;
            damp.data.constrainedObject = obj.transform;
            damp.data.sourceObject = i == 0 ? bodyParent : pieces[i - 1].gameObject.transform;

            WormPiece newPiece = new WormPiece(obj, damp);
            pieces.Add(newPiece);
        }
    }

    public GameObject bulletPrefab; // The prefab of the bullet
    public Transform firePoint; // The position where the bullet will be instantiated
    public float bulletSpeed = 10f; // The speed of the bullet
    public float fireRate = 1f; // The rate at which the object shoots (1 bullet per second)
    public float moveSpeed = 1f; // The speed at which the object moves towards the target

    [SerializeField] float orbitRadius = 5f; // Radius of the orbit around the target
    [SerializeField] float minDistance = 2f; // Minimum distance to maintain from the target
    [SerializeField] float orbitSpeed = 2f; // Speed of orbiting around the target
    [SerializeField] float smoothRotation = 5f; // Smoothing factor for rotation

    private Transform target; // The target object to orbit and shoot at

    private bool canShoot = true; // Control variable to manage shooting frequency

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine for automatic shooting

        if (Application.isPlaying)
        {
            templateDamp = (new GameObject("Damp")).AddComponent<DampedTransform>();
            templateDamp.data.dampRotation = 0.7f;
            templateDamp.data.maintainAim = true;
            SetupWorm();

            // Set the target to a predefined object
            SetTarget(GameObject.FindWithTag("Player").transform);
        }
        StartCoroutine(ShootRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        // Check if there's a target and move towards it
        if (target != null)
        {
            StartCoroutine(ShootRoutine());
            MoveAroundTarget();
        }
    }

    void MoveAroundTarget()
    {
        // Calculate the orbit position around the target
        Vector3 orbitPosition = target.position + Quaternion.Euler(0f, Time.time * orbitSpeed, 0f) * Vector3.forward * orbitRadius;

        // Smoothly rotate towards the orbit position
        Quaternion targetRotation = Quaternion.LookRotation(orbitPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothRotation * Time.deltaTime);

        // Move towards the orbit position while maintaining a minimum distance
        transform.position = Vector3.MoveTowards(transform.position, orbitPosition, orbitSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < minDistance)
        {
            transform.position = transform.position + (transform.position - target.position).normalized * minDistance;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    IEnumerator ShootRoutine()
    {
        // Loop indefinitely
        while (true)
        {
            // Check if shooting is allowed
            if (canShoot)
            {
                // Shoot
                Shoot();

                // Prevent shooting until next interval
                canShoot = false;

                // Wait for the specified time (1 / fireRate) before shooting again
                yield return new WaitForSeconds(1f / fireRate);

                // Allow shooting again
                canShoot = true;
            }
            else
            {
                // If shooting is not allowed, wait for a short time before checking again
                yield return null;
            }
        }
    }

    void Shoot()
    {
        // Calculate the direction the bullet should travel
        Vector3 direction = target.position - firePoint.position;

        // Calculate the rotation to look at the target
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.forward);

        // Instantiate a bullet at the firePoint position with the calculated rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);

        // Get the rigidbody of the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Check if the bullet has a rigidbody
        if (rb != null)
        {
            // Apply force in the direction of the rotation
            rb.velocity = bulletSpeed * direction.normalized;
        }
        else
        {
            Debug.LogWarning("Bullet prefab doesn't have a Rigidbody2D component.");
        }
    }


    void MoveTowardsTarget()
    {
        // Calculate the direction towards the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Move towards the target
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    

}