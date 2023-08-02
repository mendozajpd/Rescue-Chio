using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralDeathRayParticles : MonoBehaviour
{
    private ParticleSystem.EmissionModule _emission;

    private void Awake()
    {
        _emission = GetComponent<ParticleSystem>().emission;
    }

    public void EnableParticles()
    {
        _emission.enabled = true;
    }

    public void DisableParticles()
    {
        _emission.enabled = false;
    }
}
