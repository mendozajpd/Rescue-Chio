using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponParticles : MonoBehaviour
{
    private RangedWeapon weapon;
    private ParticleSystem particles;

    private void Awake()
    {
        weapon = GetComponentInParent<RangedWeapon>();
        particles = GetComponent<ParticleSystem>();
      
        
    }

    private void OnEnable()
    {
        // Subscribes to the weapon shot
        weapon.shootTrigger += playParticleSystem;
    }

    private void OnDisable()
    {
        // Unsubscribes to the weapon shot
        weapon.shootTrigger -= playParticleSystem;
    }

    void Start()
    {
        
    }

    void Update()
    {
        flipParticleSystem();
    }

    private void flipParticleSystem()
    {
        Vector2 currentScale = transform.localScale;
        if (weapon.IsLookingLeft && currentScale.x != -1)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, 1);
        }

        if (!weapon.IsLookingLeft && currentScale.x != 1)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, 1);
        }
    }

    private void playParticleSystem()
    {
        particles.Play();
    }

    
}
