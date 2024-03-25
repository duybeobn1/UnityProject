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


    private void Start()
    {
        StartCoroutine(RandomMovement());
        currentHealth = maxHealth;

            
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
            Vector3 randomDirection = Random.insideUnitSphere; // Get a random direction
            randomDirection.y = 0f; // Ensure the spider doesn't move up or down
            randomDirection.Normalize(); // Normalize the vector to maintain consistent speed

            float targetAngle = Mathf.Atan2(randomDirection.x, randomDirection.z) * Mathf.Rad2Deg;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            float randomSpeed = Random.Range(speed * 0.5f, speed * 1.5f); // Adjust speed randomly
            controller.AddForce(moveDir.normalized * randomSpeed, ForceMode.VelocityChange);

            yield return new WaitForSeconds(Random.Range(1f, 2f)); // Wait for random duration before next movement
        }
    }
}
