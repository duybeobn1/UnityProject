using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class simpleController : MonoBehaviour
{
    public Rigidbody controller;
    public float speed = 2f;
    public float turnSmoothTime = 0.1f;
    public int maxHealth = 3;
    private int currentHealth;
    public float attackRange = 30f; // Range within which the spider attacks
    public int damage = 1; // Damage inflicted by the spider's attack

    private GameObject player; // Reference to the player object
    Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(RandomMovement());
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");


    }
    private void Update()
    {
        if (transform.position.y < -1f || currentHealth <= 0)
        {
            // Destroy the spider

            Destroy(gameObject);

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