using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

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
    public int currentHealth;
    public int maxHealth = 100;

    public GameObject healthBarPrefab; // Reference to the health bar prefab
    private GameObject healthBarInstance; // Instance of the health bar
    public GameObject explosionEffectPrefab; // Reference to the explosion effect prefab


    void Start()
    {
        if (Application.isPlaying)
        {
            templateDamp = (new GameObject("Damp")).AddComponent<DampedTransform>();
            templateDamp.data.dampRotation = 0.7f;
            templateDamp.data.maintainAim = true;
            SetupWorm();

            // Set the target to a predefined object
            SetTarget(GameObject.FindWithTag("Player").transform);
            healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity, transform);
            currentHealth = maxHealth;
        }

    }

    void Update()
    {
        
        StartCoroutine(ShootRoutine());
        MoveAroundTarget();

        if (transform.position.y < 20f)
        {
            transform.position = new Vector3(transform.position.x, 20f, transform.position.z);
        }
    }

    public void SetActiveState(bool state)
    {
        gameObject.SetActive(state);
    }


    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateWormParts();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            // Optionally, destroy the health bar instance if it's not automatically destroyed with the GameObject.
            if (healthBarInstance != null) Destroy(healthBarInstance);
            ScoreManager.scoreCount += 100; // Assuming you want to increase the score when the boss is defeated
        }
    }
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

    void UpdateWormParts()
    {
        int partsToRemain = currentHealth / 10;
        partsToRemain = Mathf.Clamp(partsToRemain, 0, pieces.Count);

        while (pieces.Count > partsToRemain)
        {
            WormPiece lastPiece = pieces[pieces.Count - 1];
            if (lastPiece != null)
            {
                // Instantiate the explosion effect at the part's position before destroying it
                if (explosionEffectPrefab != null)
                {
                    Instantiate(explosionEffectPrefab, lastPiece.gameObject.transform.position, Quaternion.identity);
                }

                if (lastPiece.damp != null) Destroy(lastPiece.damp.gameObject);
                Destroy(lastPiece.gameObject);
                pieces.RemoveAt(pieces.Count - 1);
            }
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
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Check if the bullet has a rigidbody
        if (rb != null)
        {
            rb.velocity = bulletSpeed * direction.normalized;
        }
    }


}
