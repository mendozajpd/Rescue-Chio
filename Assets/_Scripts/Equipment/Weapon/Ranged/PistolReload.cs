using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolReload : MonoBehaviour
{
    private RangedWeapon _pistol;
    private SpriteRenderer _sprite;

    // Magazine Animation Variables
    [SerializeField] private float defaultPosX = 0.1f;
    [SerializeField] private float defaultPosY = 0.39f;
    [SerializeField] private float targetEjectDistance = 0.5f;
    [SerializeField] private Vector2 targetEjectPosition;
    [SerializeField] private float currentEjectPosition;
    [SerializeField] private float ejectSpeed;
    [SerializeField] private float eject;
    [SerializeField] private bool ejecting;
    [SerializeField] private AnimationCurve curve;

    private void Awake()
    {
        _pistol = GetComponentInParent<RangedWeapon>();
        _sprite = GetComponent<SpriteRenderer>();

    }

    private void OnEnable()
    {
        _pistol.reloadTrigger += _ejectMagazine;
    }

    private void OnDisable()
    {
        _pistol.reloadTrigger -= _ejectMagazine;
    }

    void Start()
    {
        
    }

    void Update()
    {
        _calculateEjectTrajectory();
    }

    private void _calculateEjectTrajectory()
    {
        targetEjectPosition = new Vector2(targetEjectDistance, defaultPosY);
        Vector2 defaultEjectPosition = new Vector2(defaultPosX, defaultPosY);
        currentEjectPosition = Mathf.MoveTowards(currentEjectPosition, eject, ejectSpeed * Time.deltaTime);
        transform.localPosition = Vector2.Lerp(defaultEjectPosition, targetEjectPosition, curve.Evaluate(currentEjectPosition));
        Vector2 magazinePosition = transform.localPosition;

        _retractMagazine(magazinePosition);

        if (magazinePosition == defaultEjectPosition)
        {
            ejecting = false;
        }
    }

    private void _retractMagazine(Vector2 weaponPosition)
    {
        if (weaponPosition.x > targetEjectPosition.x - 0.1f && ejecting)
        {
            eject = 0;
            _sprite.enabled = true;
        }
    }

    private void _ejectMagazine()
    {
        if (ejecting) return;

        eject = eject == 0f ? 1 : 0f;
        ejecting = true;
        _sprite.enabled = false;
    }

    // Movement should only happen .5 of the audiolength
}
