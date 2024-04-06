using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class simpleController : MonoBehaviour
{
    public Rigidbody controller;
    public Transform cam;
    public float speed = 2f;
    public float turnSmoothTime = 0.1f;
    public float maxHealth = 100f;
    public float currentHealth;
    private GameObject player; // Reference to the player object
    public float attackRange = 30f; // Range within which the spider attacks


    public GameObject healthBarPrefab; // Reference to the health bar prefab
    private GameObject healthBarInstance; // Instance of the health bar

    private void Start()
    {
        StartCoroutine(RandomMovement());
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity, transform);

    }
    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Destroy(healthBarInstance);
            ScoreManager.scoreCount += 1;
        }
        if (transform.position.y < -1f)
        {
            // Destroy the spider

            Destroy(gameObject);
            Destroy(healthBarInstance);


        }
        else
        {
            healthBarInstance.transform.position = transform.position + Vector3.up * 2f;

        }
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
    }


    private IEnumerator RandomMovement()
    {
        while (true)
        {
            if (player != null && Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                // If player is within attack range, chase the player
                Vector3 direction = player.transform.position - transform.position;
                direction.y = 0; // Ensure spider doesn't move up or down
                direction.Normalize(); // Normalize direction to maintain consistent speed

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

                controller.AddForce(moveDir.normalized * speed, ForceMode.VelocityChange);
            }
            else
            {
                // If player is not within attack range, perform random movement
                Vector3 randomDirection = Random.insideUnitSphere;
                randomDirection.y = 0;
                randomDirection.Normalize();

                float targetAngle = Mathf.Atan2(randomDirection.x, randomDirection.z) * Mathf.Rad2Deg;
                Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

                float randomSpeed = Random.Range(speed * 0.5f, speed * 1.5f);
                controller.AddForce(moveDir.normalized * randomSpeed, ForceMode.VelocityChange);
            }

            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }
}
