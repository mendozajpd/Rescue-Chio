using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileScript : MonoBehaviour
{

    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 destinationPos;
    [SerializeField] private float speed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float height;
    [SerializeField] private float heightDividend = 4;
    [SerializeField] private float timePassed;
    [SerializeField] private bool underhand;

    [SerializeField] private Vector2 mousePos;


    [Header("Destination Offset")]
    [Range(0f,1f)]
    [SerializeField] private float offsetX;
    [Range(0f,1f)]
    [SerializeField] private float offsetY;

    [Header("Magic Missile Parts")]    
    // Missile Parts
    [SerializeField] private ParticleSystem sparkles;
    [SerializeField] private ParticleSystem.EmissionModule sparklesEmission;
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private SpriteRenderer core;

    public void Init(Vector2 mousePosition, Vector2 startPosition, bool isUnderhand)
    {
        mousePos = mousePosition;
        startPos = startPosition;
        underhand = isUnderhand;
    }
    private void Awake()
    {
        sparkles = GetComponent<ParticleSystem>();
        sparklesEmission = sparkles.emission;
        trail = GetComponentInChildren<ParticleSystem>();
        core = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        getTrajectory();
        getHeight(underhand);
    }

    void Update()
    {
        if (timePassed < 1)
        {
            travelTrajectory();
        }

        if (timePassed > 1)
        {
            despawnMissile();
        }
    }

    private void despawnMissile()
    {
        var isAlive = sparkles.IsAlive();
        if (core != null)
        {
            Destroy(core.gameObject);
            sparklesEmission.rateOverTime = 0;
        }

        if (sparkles.particleCount == 0)
        {
            Destroy(gameObject);
        }
    }

    private void travelTrajectory()
    {
        timePassed += Time.deltaTime + speedMultiplier;
        transform.position = MathParabola.Parabola(startPos, destinationPos, height, timePassed);
    }

    private void getHeight(bool isUnderhand)
    {
        height = Vector2.Distance(startPos, destinationPos) / (isUnderhand ? heightDividend : -heightDividend);
    }


    private void getTrajectory()
    {

        destinationPos = new Vector2(mousePos.x - Random.Range(0,offsetX), mousePos.y - Random.Range(0,offsetY));
    }


}
