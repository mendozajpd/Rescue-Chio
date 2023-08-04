using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellChargeGlowHandler : MonoBehaviour
{
    private ParticleSystem _chargeGlow;
    private ParticleSystem.EmissionModule _chargeGlowEmission;
    [SerializeField] private float maxChargeGlowSize = 0.7f;


    private void Awake()
    {
        _chargeGlow = GetComponent<ParticleSystem>();
        _chargeGlowEmission = _chargeGlow.emission;
    }


    #region Charge Glow Functions
    public void EnableChargeGlow(float currentCharge)
    {
        if (!_chargeGlowEmission.enabled) _chargeGlowEmission.enabled = true;
        var mainModule = _chargeGlow.main;
        mainModule.startSize = currentCharge * maxChargeGlowSize;

    }
    public void DisableChargeGlow()
    {
        if (_chargeGlowEmission.enabled) _chargeGlowEmission.enabled = false;
    }
    #endregion
}
