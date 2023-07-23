using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileDeathParticles : MonoBehaviour
{
    private ParticleSystem particles;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (!particles.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
