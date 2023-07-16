using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RangedWeaponParticles : MonoBehaviour
{
    private RangedWeapon weapon;
    private ParticleSystem particles;
    private Light2D light2d;

    // Light variables
    [SerializeField] private float desiredIntensity;
    [SerializeField] private float currentIntensity;
    [SerializeField] private float decreaseLightSpeed;

    // Checks
    [SerializeField] private bool flipSpriteOnMousePos;
    [SerializeField] private bool usingLights;
    [SerializeField] private bool isEjectCase;

    private void Awake()
    {
        weapon = GetComponentInParent<RangedWeapon>();
        particles = GetComponent<ParticleSystem>();

        if (usingLights) light2d = GetComponent<Light2D>();
        
    }

    private void OnEnable()
    {
        // Subscribes to the weapon shot
        weapon.shootTrigger += playParticleSystem;

        if (usingLights) weapon.shootTrigger += turnOnLights;
    }

    private void OnDisable()
    {
        // Unsubscribes to the weapon shot
        weapon.shootTrigger -= playParticleSystem;

        if (usingLights) weapon.shootTrigger -= turnOnLights;

    }

    void Start()
    {
        
    }

    void Update()
    {
        if (flipSpriteOnMousePos) flipParticleSystem();

        if (usingLights)
        {
            intensityDecreaser();
        }
        
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


    // Event invokes intensity to become 1, then it will slowly go down

    private void turnOnLights()
    {
        currentIntensity = desiredIntensity;
    }

    private void intensityDecreaser()
    {
        if (currentIntensity > 0 )
        {
            light2d.enabled = true;
            currentIntensity -= Time.deltaTime * decreaseLightSpeed;
            light2d.intensity = currentIntensity;
        } else
        {
            light2d.enabled = false;
        }


    }
    
}
