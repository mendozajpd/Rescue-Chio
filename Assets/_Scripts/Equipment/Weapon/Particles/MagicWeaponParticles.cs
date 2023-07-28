using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class MagicWeaponParticles : MonoBehaviour
{
    private MagicWeapon magicWeapon;
    private ParticleSystem particles;
    private Light2D light2d;

    // Light variables
    [SerializeField] private float desiredIntensity;
    [SerializeField] private float currentIntensity;
    [SerializeField] private float decreaseLightSpeed;

    // Checks
    [SerializeField] private bool usingLights;

    private void Awake()
    {
        magicWeapon = GetComponentInParent<MagicWeapon>();
        particles = GetComponent<ParticleSystem>();

        if (usingLights) light2d = GetComponent<Light2D>();

    }

    private void OnEnable()
    {
        // Subscribes to the astralBeamSpell shot
        magicWeapon.castTrigger += playParticleSystem;

        if (usingLights) magicWeapon.castTrigger += turnOnLights;
    }

    private void OnDisable()
    {
        // Unsubscribes to the astralBeamSpell shot
        magicWeapon.castTrigger -= playParticleSystem;

        if (usingLights) magicWeapon.castTrigger -= turnOnLights;

    }

    void Start()
    {

    }

    void Update()
    {

        if (usingLights)
        {
            intensityDecreaser();
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
        if (currentIntensity > 0)
        {
            light2d.enabled = true;
            currentIntensity -= Time.deltaTime * decreaseLightSpeed;
            light2d.intensity = currentIntensity;
        }
        else
        {
            light2d.enabled = false;
        }


    }
}
