using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Explosion_Default : MonoBehaviour, IExplode
{
    private ParticleSystem _particles;
    private Light2D _light2D;

    [SerializeField] private float intensityFadeSpeed = 1f;
    [SerializeField] private float volumentricIntensityFadeSpeed = 3;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
        _light2D = GetComponent<Light2D>();
    }

    private void FixedUpdate()
    {
        if (_light2D.intensity > 0)
        {
            _light2D.intensity -= intensityFadeSpeed;
        } else
        {
            _light2D.intensity = 0;
        }
    }

    public void Explode()
    {
        _particles.Play();
        StartCoroutine(destroyObject(1));
    }

    IEnumerator destroyObject(float secondsUntilDestruction)
    {
        yield return new WaitForSeconds(secondsUntilDestruction);

        Destroy(gameObject);
    }

}
