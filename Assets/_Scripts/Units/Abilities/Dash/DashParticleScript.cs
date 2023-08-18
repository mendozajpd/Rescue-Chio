using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashParticleScript : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private ParticleSystem dashParticles;
    private ParticleSystem.EmissionModule dashParticleEmission;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        dashParticles = GetComponent<ParticleSystem>();
        dashParticleEmission = dashParticles.emission;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        player.hasDashed += enableEmission;
        player.hasStoppedDashing += disableEmission;
    }
        
        
    private void OnDisable()
    {
        player.hasDashed -= enableEmission;
        player.hasStoppedDashing -= disableEmission;
    }


    private void enableEmission()
    {
        dashParticleEmission.enabled = true;
    }

    private void disableEmission()
    {
        dashParticleEmission.enabled = false;
    }



    // when enabled it will get the controller from the parent and reference it into a script
    // once getting a reference from the script, it will read if the player has dashed.
    // When the player dashes it will enable the emission of the dash
}
