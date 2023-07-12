using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponParticles : MonoBehaviour
{
    [SerializeField] private MeleeWeapon weapon;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule ps_emission;
    private void Awake()
    {
        weapon = transform.parent.parent.GetComponentInParent<MeleeWeapon>();
        ps = GetComponent<ParticleSystem>();
        ps_emission = ps.emission;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (weapon.Swinging)
        {
            enableEmission();
        } else
        {
            disableEmission();
        }
    }

    private void enableEmission()
    {
        ps_emission.enabled = true;
    }

    private void disableEmission()
    {
        ps_emission.enabled = false;
    }
}
