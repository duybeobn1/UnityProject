using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnBoss : MonoBehaviour
{   
    public GameObject boss;
    public WormBuilder wb;
    public GameObject healthbar;
    // Start is called before the first frame update
    void Start()
    {
        boss.SetActive(false);
        healthbar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.scoreCount >= 1)
        {
            boss.SetActive(true); // Active le boss et ses fonctionnalités
            healthbar.SetActive(true);
        }
    }
}
