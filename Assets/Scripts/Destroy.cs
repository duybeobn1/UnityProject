using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    void Start()
    {
        // Get the ParticleSystem attached to the game object
        ParticleSystem ps = GetComponent<ParticleSystem>();

        // Destroy the game object after the duration of the particle system
        // This assumes the particle system is not looping
        Destroy(gameObject, ps.main.duration);
    }
}