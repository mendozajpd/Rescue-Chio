using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolReload : MonoBehaviour
{
    private RangedWeapon _pistol;
    private SpriteRenderer _sprite;

    // Magazine Animation Variables
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float targetEjectDistance = 0.5f;
    [SerializeField] private float ejectSpeed;
    private float _defaultPosX = 0.1f;
    private float _defaultPosY = 0.39f;
    private Vector2 _targetEjectPosition;
    private float _currentEjectPosition;
    private float _eject;
    private bool _ejecting;




    // Audio Variables
    public AudioClip[] magazineAudio;
    private AudioSource audioSource;
    [Range(0, 0.5f)]
    public float volumeChangeMultiplier;
    [Range(0, 0.5f)]
    public float pitchChangeMultiplier;
    [Range(0, 1f)]
    public float AudioVolume = 1;

    // Actions
    public System.Action magazineInserted;

    public float EjectSpeed { get => ejectSpeed; }

    private void Awake()
    {
        _pistol = GetComponentInParent<RangedWeapon>();
        _sprite = GetComponent<SpriteRenderer>();

        // Setting Audio
        audioSource = GetComponent<AudioSource>();

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
        StartCoroutine(setMagazineSpeed());
    }

    void Update()
    {
        _calculateEjectTrajectory();
    }

    IEnumerator setMagazineSpeed()
    {
        ejectSpeed = Mathf.Lerp(3, 10, (_pistol.ReloadSpeed - 1) / (0.3f - 1)); // Maps the speed 
        yield return new WaitForSeconds(1);
        StartCoroutine(setMagazineSpeed());
    }

    private void _calculateEjectTrajectory()
    {
        _targetEjectPosition = new Vector2(targetEjectDistance, _defaultPosY);
        Vector2 defaultEjectPosition = new Vector2(_defaultPosX, _defaultPosY);
        _currentEjectPosition = Mathf.MoveTowards(_currentEjectPosition, _eject, ejectSpeed * Time.deltaTime);
        transform.localPosition = Vector2.Lerp(defaultEjectPosition, _targetEjectPosition, curve.Evaluate(_currentEjectPosition));
        Vector2 magazinePosition = transform.localPosition;

        if (_pistol.ReloadTime < _pistol.ReloadSpeed * 0.8f && _eject != 0) _retractMagazine(magazinePosition);

        if (magazinePosition == defaultEjectPosition)
        {
            _ejecting = false;
        }
    }

    private void _retractMagazine(Vector2 weaponPosition)
    {
        if (weaponPosition.x > _targetEjectPosition.x - 0.1f && _ejecting)
        {
            Debug.Log("retracted!");
            _eject = 0;
            AudioHandler(1, true);
            magazineInserted.Invoke();
        }
    }

    private void _ejectMagazine()
    {
        if (_ejecting) return;

        _eject = _eject == 0f ? 1 : 0f;
        _ejecting = true;
        AudioHandler(0, true);
    }

    private void AudioHandler(int clipNumber, bool isOneShot)
    {
        audioSource.clip = magazineAudio[clipNumber];
        audioSource.volume = Random.Range(AudioVolume - volumeChangeMultiplier, AudioVolume);
        audioSource.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
        playAudio(isOneShot);
    }

    private void playAudio(bool isOneShot)
    {
        if (isOneShot)
        {
            audioSource.PlayOneShot(audioSource.clip);
            return;
        }
        audioSource.Play();
    }

    // Movement should only happen .5 of the audiolength
}
