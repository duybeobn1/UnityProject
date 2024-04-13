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
    private GameObject player; 
    public float attackRange = 30f; 


    public GameObject healthBarPrefab; 
    private GameObject healthBarInstance; 


    private Renderer childRenderer; 
    private Color originalColor;

    public GameObject explosionEffectPrefab; 

    private void Start()
    {

        childRenderer = GetComponentInChildren<Renderer>();
        if (childRenderer != null)
        {
            originalColor = childRenderer.material.color;
        }
        else
        {
            Debug.Log("Renderer not found on any child objects.");
        }

        StartCoroutine(RandomMovement());
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity, transform);

    }
    private void Update()
    {
        if (transform.position.y < -1f)
        {
            Destroy(gameObject);
            Destroy(healthBarInstance);
        }
        else
        {
            healthBarInstance.transform.position = transform.position + Vector3.up * 2f;

        }
    }

    private bool hasExploded = false; // Add this line to your class variables

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0)
        {
            StartCoroutine(FlashRed());
        }
        else if (currentHealth <= 0 && !hasExploded)
        {
            hasExploded = true; // Ensure explosion only happens once
            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
            if (healthBarInstance != null) Destroy(healthBarInstance); // Check for null to avoid errors
            ScoreManager.scoreCount += 1;
        }
    }

    private IEnumerator FlashRed()
    {
        if (childRenderer != null)
        {
            childRenderer.material.color = Color.red; 
            yield return new WaitForSeconds(0.3f); 
            childRenderer.material.color = originalColor; 
        }
    }


    private IEnumerator RandomMovement()
    {
        while (true)
        {
            if (player != null && Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                Vector3 direction = player.transform.position - transform.position;
                direction.y = 0; 
                direction.Normalize(); 

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

                controller.AddForce(moveDir.normalized * speed, ForceMode.VelocityChange);
            }
            else
            {
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
