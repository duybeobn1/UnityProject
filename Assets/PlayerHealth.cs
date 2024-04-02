using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHeath = 10;
    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHeath;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0 )
        {
            Debug.Log("Die");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spider"))

        {
            TakeDamage(1);
        }
    }
}
