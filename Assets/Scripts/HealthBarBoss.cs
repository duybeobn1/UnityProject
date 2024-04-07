using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBoss : MonoBehaviour
{
	public Slider healthSlider;
	public Slider easeHealthSlider;
	public float health;
	private float lerpSpeed = 0.05f;
	public WormBuilder creatureController;

	
    // Start is called before the first frame update
    void Start()
    {
    	health = creatureController.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthSlider.value != creatureController.currentHealth)
        {
        	healthSlider.value = creatureController.currentHealth;
        }
        
      
        if(healthSlider.value != easeHealthSlider.value)
        {
        	easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, creatureController.currentHealth, lerpSpeed);
        }
    }
    
    void takeDamage(float damage){
        health -= damage;
    }
}